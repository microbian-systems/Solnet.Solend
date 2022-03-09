using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using Solnet.Rpc;
using System;

namespace Solnet.Solend.Examples
{
    public class GetObligations : IRunnableExample
    {
        private readonly ILogger _logger;
        private IRpcClient RpcClient;
        private ISolendClient SolendClient;
        private Wallet.Wallet Wallet;

        public GetObligations()
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


            RpcClient = Rpc.ClientFactory.GetClient(Cluster.DevNet, _logger);
            SolendClient = ClientFactory.GetClient(RpcClient, SolendProgram.DevNetProgramIdKey);
        }

        public async void Run()
        {
            var obligations = await SolendClient.GetObligationsAsync(new("hoakwpFB8UoLnPpLC56gsjpY7XbVwaCuRQRMQzN5TVh"));

            foreach(var obligation in obligations.ParsedResult)
            {
                var reserves = await SolendClient.GetReservesAsync(obligation.LendingMarket);

                var position = obligation.CalculatePosition(reserves);

                foreach(var borrow in position.Borrows)
                {
                    Console.WriteLine($"Mint: {borrow.MintAddress,-50} Borrowed: {borrow.NativeAmountUi,-10:N4} - ${borrow.AmountUsd,-10:N4}");
                }

                foreach(var deposit in position.Deposits)
                {
                    Console.WriteLine($"Mint: {deposit.MintAddress,-50} Borrowed: {deposit.NativeAmountUi,-10:N4} - ${deposit.AmountUsd,-10:N4}");
                }

                Console.WriteLine($"Borrow Limit: {position.ObligationStats.BorrowLimit,-10:N4}\n" +
                    $"Utilization: {position.ObligationStats.BorrowUtilization,-10:N4}\n" +
                    $"Total Borrowed: {position.ObligationStats.UserTotalBorrow,-10:N4}\n" +
                    $"Liquidation Threshold: {position.ObligationStats.LiquidationThreshold,-10:N4}\n" +
                    $"Total Deposits: {position.ObligationStats.UserTotalDeposit,-10:N4}\n" +
                    $"Net Account Value: {position.ObligationStats.NetAccountValue,-10:N4}");
            }

            Console.ReadLine();
        }
    }
}
