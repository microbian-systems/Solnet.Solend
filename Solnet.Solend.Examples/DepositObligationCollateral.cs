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
using System.Linq;

namespace Solnet.Solend.Examples
{
    public class DepositObligationCollateral : IRunnableExample
    {
        private readonly ILogger _logger;
        private readonly IRpcClient RpcClient;
        private readonly ISolendClient SolendClient;
        private readonly Wallet.Wallet Wallet;
        private readonly SolendProgram Solend;
        private readonly SolanaKeyStoreService SolanaKeyStoreService;

        public DepositObligationCollateral()
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
            // the process below assumes that a token account with enough balance to deposit 1 cToken exists
            var tokenMint = new PublicKey("So11111111111111111111111111111111111111112");
            var lendingMarketPubkey = Constants.DevNetLendingMarkets.Values.First();

            // get the reserves for the relevant lending market
            var reserves = await SolendClient.GetReservesAsync(lendingMarketPubkey);

            // get the SOL reserve
            var reserve = reserves.ParsedResult.First(x => x.Liquidity.Mint.Equals(tokenMint));
            var reserveIndex = reserves.ParsedResult.IndexOf(reserve);
            var reservePubkey = reserves.OriginalRequest.Result[reserveIndex].PublicKey;

            // derive the obligation pubkey for this market and user
            var obligationPubkey = Solend.DeriveObligationAddress(Wallet.Account, lendingMarketPubkey);

            // get a recent blockhash and the rent exemption for a token account
            var blockHash = await RpcClient.GetRecentBlockHashAsync();
            var tokenAccountRent = await RpcClient.GetMinimumBalanceForRentExemptionAsync(TokenProgram.TokenAccountDataSize);

            // the atas we're going to use
            var liquidityAta = AssociatedTokenAccountProgram.DeriveAssociatedTokenAccount(Wallet.Account, tokenMint);
            var collateralAta = AssociatedTokenAccountProgram.DeriveAssociatedTokenAccount(Wallet.Account, reserve.Collateral.Mint);

            // we want to deposit 1 cSOL
            var nativeDepositAmount = 0ul;

            if (tokenMint == WellKnownTokens.WrappedSOL.TokenMint)
            {
                nativeDepositAmount = SolHelper.ConvertToLamports(1m);
            }
            else
            {
                nativeDepositAmount = (ulong)(1m * (decimal)Math.Pow(10, reserve.Liquidity.Decimals));
            }

            // init tx builder before actually adding instructions
            var txBuilder = new TransactionBuilder()
                .SetRecentBlockHash(blockHash.Result.Value.Blockhash)
                .SetFeePayer(Wallet.Account);

            // refresh the reserve and redeem the cToken in exchange for the underlying liquidity
            txBuilder.AddInstruction(Solend.RefreshReserve(
                    new(reservePubkey),
                    reserve.Liquidity.PythOracle,
                    reserve.Liquidity.SwitchboardOracle));
            txBuilder.AddInstruction(Solend.DepositObligationCollateral(
                    nativeDepositAmount,
                    collateralAta,
                    reserve.Collateral.Supply,
                    new(reservePubkey),
                    obligationPubkey,
                    lendingMarketPubkey,
                    Wallet.Account,
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
