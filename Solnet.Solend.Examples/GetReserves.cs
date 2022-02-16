using Solnet.Rpc;
using Solnet.Solend.Models;
using Solnet.Wallet;
using System;
using System.Threading.Tasks;

namespace Solnet.Solend.Examples
{
    public class GetReserves : IRunnableExample
    {
        private IRpcClient RpcClient;
        private ISolendClient SolendClient;

        public GetReserves()
        {
            RpcClient = Rpc.ClientFactory.GetClient(Cluster.DevNet);
            SolendClient = ClientFactory.GetClient(RpcClient, SolendProgram.DevNetProgramIdKey);
        }

        public async void Run()
        {
            var markets = await SolendClient.GetLendingMarketsAsync();
            if (!markets.WasSuccessful)
            {
                Console.WriteLine(markets.OriginalRequest.Reason);
                return;
            }
            Console.WriteLine($"{markets.ParsedResult.Count} Lending Markets found.");

            for(int i = 0; i < markets.OriginalRequest.Result.Count; i++)
            {
                Console.WriteLine($"\nLending Market: {markets.OriginalRequest.Result[i].PublicKey} " +
                    $"Quote: {markets.ParsedResult[i].QuoteCurrencyMint ?? markets.ParsedResult[i].QuoteCurrency}");

                var reserves = await SolendClient.GetReservesAsync(new(markets.OriginalRequest.Result[i].PublicKey));
                if (!reserves.WasSuccessful)
                {
                    Console.WriteLine($"\tCould not find reserves. Reason: {reserves.OriginalRequest.Reason}");
                    await Task.Delay(1000);
                    continue;
                }
                Console.WriteLine($"\t{reserves.ParsedResult.Count} Reserves found.");

                var assets = 0;
                var totalSupply = 0m;
                var totalBorrow = 0m;
                for(int j = 0; j < reserves.OriginalRequest.Result.Count; j++)
                {
                    var reserve = reserves.ParsedResult[j];
                    var utilization = reserve.CalculateUtilizationRatio();
                    var borrowApr = reserve.CalculateBorrowApr();
                    var borrowApy = reserve.CalculateBorrowApy();
                    var supplyApr = reserve.CalculateSupplyApr();
                    var supplyApy = reserve.CalculateSupplyApy();
                    var supply = reserve.GetTotalSupply();
                    var supplyUsd = reserve.GetTotalSupplyUsd();
                    var borrowUsd = reserve.GetTotalBorrowUsd();

                    if (supply < 1) continue;

                    Console.WriteLine($"\n\tReserve: {reserves.OriginalRequest.Result[j].PublicKey}\n" +
                        $"\t\tCollat. Mint: {reserve.Collateral.Mint}\n" +
                        $"\t\tCollat. Supply: {reserve.Collateral.Supply}\n" +
                        $"\t\tCollat. Total Supply: {supply:N4}\n" +
                        $"\t\tCollat. Total Supply: ${supplyUsd:N4}\n" +
                        $"\t\tLiq. Mint: {reserve.Liquidity.Mint}\n" +
                        $"\t\tLiq. Supply: {reserve.Liquidity.Supply}\n" +
                        $"\t\tLiq. Pyth Oracle: {reserve.Liquidity.PythOracle}\n" +
                        $"\t\tLiq. Switchboard Oracle: {reserve.Liquidity.SwitchboardOracle}\n" +
                        $"\t\tLiq. Market Price: ${reserve.GetMarketPrice():N4}\n" +
                        $"\t\tLiq. Mint Decimals: {reserve.Liquidity.Decimals}\n" +
                        $"\t\tLiq. Borrowed Amount: {reserve.GetTotalBorrow():N4}\n" +
                        $"\t\tLiq. Borrowed Amount: ${borrowUsd:N4}\n" +
                        $"\t\tLiq. Available Amount: {reserve.GetAvailableAmount():N4}");

                    Console.WriteLine($"\n\t\tUtilization: {utilization:P2} LTV: {reserve.Config.LoanToValueRatio / 100m:P2}\n" +
                        $"\t\tBorrow APR: {borrowApr:P2} " +
                        $"Borrow APY: {borrowApy:P2}\n" +
                        $"\t\tSupply APR: {supplyApr:P2} " +
                        $"Supply APY: {supplyApy:P2}");

                    assets++;
                    totalSupply += supplyUsd;
                    totalBorrow += borrowUsd;
                }

                Console.WriteLine($"\nAssets: {assets} " +
                    $"\nTotal Supply: ${totalSupply:N4} " +
                    $"\nTotal Borrow: ${totalBorrow:N4}" +
                    $"\nTVL: ${(totalSupply - totalBorrow):N4}");
            }
        }
    }
}
