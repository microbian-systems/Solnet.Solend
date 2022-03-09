using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using Solnet.Extensions;
using Solnet.KeyStore;
using Solnet.Programs;
using Solnet.Programs.Utilities;
using Solnet.Rpc;
using Solnet.Rpc.Builders;
using Solnet.Solend.Models;
using Solnet.Wallet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solnet.Solend.Examples
{
    public class WithdrawObligationCollateral : IRunnableExample
    {
        private readonly ILogger _logger;
        private readonly IRpcClient RpcClient;
        private readonly ISolendClient SolendClient;
        private readonly Wallet.Wallet Wallet;
        private readonly SolendProgram Solend;
        private readonly SolanaKeyStoreService SolanaKeyStoreService;

        public WithdrawObligationCollateral()
        {
            _logger = LoggerFactory.Create(x =>
            {
                x.AddSimpleConsole(o =>
                {
                    o.UseUtcTimestamp = true;
                    o.IncludeScopes = true;
                    o.ColorBehavior = LoggerColorBehavior.Enabled;
                    o.TimestampFormat = "HH:mm:ss ";
                })
                .SetMinimumLevel(LogLevel.Debug);
            }).CreateLogger<IRpcClient>();

            SolanaKeyStoreService = new();
            Wallet = SolanaKeyStoreService.RestoreKeystoreFromFile("C:\\Users\\warde\\.config\\solana\\t2.json");

            Solend = SolendProgram.CreateDevNet();

            RpcClient = Rpc.ClientFactory.GetClient(Cluster.DevNet, _logger);
            SolendClient = ClientFactory.GetClient(RpcClient, Solend.ProgramIdKey);
        }

        public async void Run()
        {
            // pick a token mint, in this case we're using wrapped sol but any of the available liquidity reserve mints can be used
            // the process below assumes that an obligation with enough deposits to withdraw 1 cToken exists
            var tokenMint = new PublicKey("So11111111111111111111111111111111111111112");
            var lendingMarketPubkey = Constants.DevNetLendingMarkets.Values.First();

            // get the reserves for the relevant lending market
            var reserves = await SolendClient.GetReservesAsync(lendingMarketPubkey);

            // get the SOL reserve
            var reserve = reserves.ParsedResult.First(x => x.Liquidity.Mint.Equals(tokenMint));
            var reserveIndex = reserves.ParsedResult.IndexOf(reserve);
            var reservePubkey = reserves.OriginalRequest.Result[reserveIndex].PublicKey;

            // derive the obligation for this market and user
            var obligationPubkey = Solend.DeriveObligationAddress(Wallet.Account, lendingMarketPubkey);

            // get the obligation account and calculate the user's positions
            var obligation = await SolendClient.GetObligationAsync(obligationPubkey);
            var position = obligation.ParsedResult.CalculatePosition(reserves);

            // get a recent blockhash and the rent exemption for a token account
            var blockHash = await RpcClient.GetRecentBlockHashAsync();

            // the atas we're going to use
            var liquidityAta = AssociatedTokenAccountProgram.DeriveAssociatedTokenAccount(Wallet.Account, tokenMint);
            var collateralAta = AssociatedTokenAccountProgram.DeriveAssociatedTokenAccount(Wallet.Account, reserve.Collateral.Mint);

            // we want to deposit 1 cSOL
            var nativeWithdrawAmount = 0ul;

            if (tokenMint == WellKnownTokens.WrappedSOL.TokenMint)
            {
                nativeWithdrawAmount = SolHelper.ConvertToLamports(1m);
            }
            else
            {
                nativeWithdrawAmount = (ulong)(1m * (decimal)Math.Pow(10, reserve.Liquidity.Decimals));
            }

            // init tx builder before actually adding instructions
            var txBuilder = new TransactionBuilder()
                .SetRecentBlockHash(blockHash.Result.Value.Blockhash)
                .SetFeePayer(Wallet.Account);

            // we need to refresh the reserves in which we have positions
            // and pass the pubkeys of those reserves into `RefreshObligation`
            var obligationReserves = new List<PublicKey>();

            foreach (var deposit in position.Deposits)
            {
                var depositReserve = reserves.ParsedResult.FirstOrDefault(x => x.Liquidity.Mint.Equals(deposit.MintAddress));
                if (depositReserve != null)
                {
                    var depositReserveIndex = reserves.ParsedResult.IndexOf(depositReserve);
                    var depositReservePubkey = reserves.OriginalRequest.Result[depositReserveIndex].PublicKey;
                    txBuilder.AddInstruction(Solend.RefreshReserve(
                        new(depositReservePubkey),
                        depositReserve.Liquidity.PythOracle,
                        depositReserve.Liquidity.SwitchboardOracle));
                    obligationReserves.Add(new(depositReservePubkey));
                }
            }

            foreach(var borrow in position.Borrows)
            {
                var borrowReserve = reserves.ParsedResult.FirstOrDefault(x => x.Liquidity.Mint.Equals(borrow.MintAddress));
                if (borrowReserve != null)
                {
                    var borrowReserveIndex = reserves.ParsedResult.IndexOf(borrowReserve);
                    var borrowReservePubkey = reserves.OriginalRequest.Result[borrowReserveIndex].PublicKey;
                    if (!obligationReserves.Contains(new(borrowReservePubkey)))
                        txBuilder.AddInstruction(Solend.RefreshReserve(
                            new(borrowReservePubkey),
                            borrowReserve.Liquidity.PythOracle,
                            borrowReserve.Liquidity.SwitchboardOracle));
                    obligationReserves.Add(new(borrowReservePubkey));
                }
            }

            // refresh the reserve and withdraw the cToken from the reserve's collateral supply pool
            txBuilder.AddInstruction(Solend.RefreshObligation(
                    obligationPubkey,
                    obligationReserves))
                .AddInstruction(Solend.WithdrawObligationCollateral(
                    nativeWithdrawAmount,
                    reserve.Collateral.Supply,
                    collateralAta,
                    new(reservePubkey),
                    obligationPubkey,
                    lendingMarketPubkey,
                    Wallet.Account));

            var msg = txBuilder.CompileMessage();

            ExampleHelpers.DecodeAndLogMessage(msg);

            var signature = Wallet.Account.Sign(msg);

            txBuilder.AddSignature(signature);

            var txSig = ExampleHelpers.SubmitTxSendAndLog(RpcClient, txBuilder.Serialize());

            await ExampleHelpers.PollTx(RpcClient, txSig, Rpc.Types.Commitment.Confirmed);

            Console.ReadLine();
        }
    }
}