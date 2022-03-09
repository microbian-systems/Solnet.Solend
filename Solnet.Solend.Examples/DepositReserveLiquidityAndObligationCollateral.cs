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
    public class DepositReserveLiquidityAndObligationCollateral : IRunnableExample
    {
        private readonly ILogger _logger;
        private readonly IRpcClient RpcClient;
        private readonly ISolendClient SolendClient;
        private readonly Wallet.Wallet Wallet;
        private readonly SolendProgram Solend;
        private readonly SolanaKeyStoreService SolanaKeyStoreService;

        public DepositReserveLiquidityAndObligationCollateral()
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
            // the process below assumes that in case the deposit is NOT wrapped SOL, that a token account with enough balance to deposit 1 token exists
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

            // get all obligations for this market and user, in case one doesn't exist we'll need to create
            var obligations = await SolendClient.GetObligationsAsync(Wallet.Account, lendingMarketPubkey);

            // get a recent blockhash and the rent exemption for a token account
            var blockHash = await RpcClient.GetRecentBlockHashAsync();
            var tokenAccountRent = await RpcClient.GetMinimumBalanceForRentExemptionAsync(TokenProgram.TokenAccountDataSize);

            // the atas we're going to use
            var liquidityAta = AssociatedTokenAccountProgram.DeriveAssociatedTokenAccount(Wallet.Account, tokenMint);
            var collateralAta = AssociatedTokenAccountProgram.DeriveAssociatedTokenAccount(Wallet.Account, reserve.Collateral.Mint);

            // get the collateral token account
            var collateralTokenAccount = await RpcClient.GetTokenAccountInfoAsync(collateralAta);

            // we want to deposit 1 SOL
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

            // check if obligations exist
            if (obligations.ParsedResult.Count == 0)
            {
                var obligationRentExemption = await RpcClient.GetMinimumBalanceForRentExemptionAsync(Obligation.Layout.Length);

                // obligation doesn't exist so we have to create it in the instructions below
                txBuilder
                    .AddInstruction(SystemProgram.CreateAccountWithSeed(
                        Wallet.Account,
                        obligationPubkey,
                        Wallet.Account,
                        lendingMarketPubkey.Key[..32],
                        obligationRentExemption.Result,
                        Obligation.Layout.Length,
                        Solend.ProgramIdKey
                        ))
                    .AddInstruction(Solend.InitializeObligation(
                        obligationPubkey,
                        lendingMarketPubkey,
                        Wallet.Account));
            }

            // check if we're depositing wrapped sol so that we wrap SOL before depositing
            if (tokenMint == WellKnownTokens.WrappedSOL.TokenMint)
            {
                txBuilder
                    .AddInstruction(SystemProgram.Transfer(
                        Wallet.Account,
                        liquidityAta,
                        nativeDepositAmount + tokenAccountRent.Result
                        ))
                    .AddInstruction(AssociatedTokenAccountProgram.CreateAssociatedTokenAccount(
                        Wallet.Account,
                        Wallet.Account,
                        tokenMint));
            }

            // check if the collateral token account exists, otherwise we'll need to create it
            if (collateralTokenAccount.Result.Value == null)
            {
                txBuilder
                    .AddInstruction(AssociatedTokenAccountProgram.CreateAssociatedTokenAccount(
                        Wallet.Account,
                        Wallet.Account,
                        reserve.Collateral.Mint));
            }

            txBuilder
                .AddInstruction(Solend.DepositReserveLiquidityAndObligationCollateral(
                    nativeDepositAmount,
                    liquidityAta,
                    collateralAta,
                    new(reservePubkey),
                    reserve.Liquidity.Supply,
                    reserve.Collateral.Mint,
                    lendingMarketPubkey,
                    reserve.Collateral.Supply,
                    obligationPubkey,
                    Wallet.Account,
                    reserve.Liquidity.PythOracle,
                    reserve.Liquidity.SwitchboardOracle,
                    Wallet.Account
                    ));

            // in case we're depositing SOL, close the wrapped SOL ata
            if (tokenMint == WellKnownTokens.WrappedSOL.TokenMint)
            {
                txBuilder
                    .AddInstruction(TokenProgram.CloseAccount(
                        liquidityAta,
                        Wallet.Account,
                        Wallet.Account,
                        TokenProgram.ProgramIdKey));
            }

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
