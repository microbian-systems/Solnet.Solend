using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using Solnet.Extensions;
using Solnet.Programs.Utilities;
using Solnet.Rpc;
using Solnet.Wallet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solnet.Solend.Examples
{
    public class GetLendingMarkets : IRunnableExample
    {
        private readonly ILogger _logger;
        private static IRpcClient RpcClient;
        private static IStreamingRpcClient StreamingRpcClient;

        private static PublicKey Owner = new("hoakwpFB8UoLnPpLC56gsjpY7XbVwaCuRQRMQzN5TVh");

        private readonly ISolendClient SolendClient;

        public GetLendingMarkets()
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

            RpcClient = Rpc.ClientFactory.GetClient("https://ssc-dao.genesysgo.net", _logger);
            StreamingRpcClient = Rpc.ClientFactory.GetStreamingClient(Cluster.MainNet);
            SolendClient = ClientFactory.GetClient(RpcClient, SolendProgram.MainNetProgramIdKey);
        }

        public async void Run()
        {
            var markets = await SolendClient.GetLendingMarketsAsync();

            var tokenMintResolver = await TokenMintResolver.LoadAsync();

            var solendSupply = 0m;
            var solendBorrow = 0m;
            var solendAssets = new List<PublicKey>();

            foreach (var market in markets.OriginalRequest.Result)
            {
                var lendingMarket = await SolendClient.GetLendingMarketAsync(new(market.PublicKey));

                var lendingMarketQuote = lendingMarket.ParsedResult.QuoteCurrencyMint ?? lendingMarket.ParsedResult.QuoteCurrency;

                Console.WriteLine($"\nLending Market: {market.PublicKey}\n" +
                    $"Address: {market}\n" +
                    $"Quote: {lendingMarketQuote}");

                var reserves = await SolendClient.GetReservesAsync(new(market.PublicKey));
                if (!reserves.WasSuccessful)
                {
                    Console.WriteLine($"\tCould not find reserves. Reason: {reserves.OriginalRequest.Reason}");
                    await Task.Delay(1000);
                    continue;
                }
                Console.WriteLine($"\t{reserves.ParsedResult.Count} Reserves found.");

                var reserveAssets = 0;
                var totalSupply = 0m;
                var totalBorrow = 0m;
                for (int j = 0; j < reserves.OriginalRequest.Result.Count; j++)
                {
                    var reserve = reserves.ParsedResult[j];
                    _ = tokenMintResolver.KnownTokens.TryGetValue(reserve.Liquidity.Mint, out var liqToken);
                    _ = tokenMintResolver.KnownTokens.TryGetValue(reserve.Collateral.Mint, out var collatToken);
                    var utilization = reserve.CalculateUtilizationRatio();
                    var borrowApr = reserve.CalculateBorrowApr();
                    var borrowApy = reserve.CalculateBorrowApy();
                    var supplyApr = reserve.CalculateSupplyApr();
                    var supplyApy = reserve.CalculateSupplyApy();
                    var supply = reserve.GetTotalSupply();
                    var supplyUsd = reserve.GetTotalSupplyUsd();
                    var borrow = reserve.GetTotalBorrow();
                    var borrowUsd = reserve.GetTotalBorrowUsd();
                    var available = reserve.GetAvailableAmount();
                    var availableUsd = reserve.GetAvailableAmountUsd();

                    if (supply < 1) continue;

                    Console.WriteLine($"\n\tReserve: {reserves.OriginalRequest.Result[j].PublicKey} ({liqToken?.Symbol}) ({collatToken?.Symbol})\n" +
                        $"\t\tCollat. Total Supply: {supply:N4}\n" +
                        $"\t\tCollat. Total Supply: ${supplyUsd:N4}\n" +
                        $"\t\tLiq. Borrowed Amount: {borrow:N4}\n" +
                        $"\t\tLiq. Borrowed Amount: ${borrowUsd:N4}\n" +
                        $"\t\tLiq. Available Amount: {available:N4}\n" +
                        $"\t\tLiq. Available Amount: ${availableUsd:N4}");

                    Console.WriteLine($"\n\t\tUtilization: {utilization:P2} LTV: {reserve.Config.LoanToValueRatio / 100m:P2}\n" +
                        $"\t\tBorrow APR: {borrowApr:P2} " +
                        $"Borrow APY: {borrowApy:P2}\n" +
                        $"\t\tSupply APR: {supplyApr:P2} " +
                        $"Supply APY: {supplyApy:P2}");

                    reserveAssets++;
                    totalSupply += supplyUsd;
                    totalBorrow += borrowUsd;
                    solendSupply += supplyUsd;
                    solendBorrow += borrowUsd;
                    if (!solendAssets.Contains(reserve.Liquidity.Mint)) solendAssets.Add(reserve.Liquidity.Mint);
                }
                Console.WriteLine($"\nAssets: {reserveAssets} " +
                    $"\nTotal Supply: ${totalSupply:N4} " +
                    $"\nTotal Borrow: ${totalBorrow:N4}" +
                    $"\nTVL: ${(totalSupply - totalBorrow):N4}");
            }
            Console.WriteLine($"\nAssets: {solendAssets.Count} " +
                $"\nTotal Supply: ${solendSupply:N4} " +
                $"\nTotal Borrow: ${solendBorrow:N4}" +
                $"\nTVL: ${(solendSupply - solendBorrow):N4}");
        }
    }
}
