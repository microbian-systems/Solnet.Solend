using Solnet.Rpc;
using Solnet.Wallet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solnet.Solend.Examples
{
    public class GetReserves : IRunnableExample
    {
        private static IRpcClient RpcClient;
        private static IStreamingRpcClient StreamingRpcClient;

        private static PublicKey Owner = new("hoakwpFB8UoLnPpLC56gsjpY7XbVwaCuRQRMQzN5TVh");

        private readonly ISolendClient SolendClient;

        public GetReserves()
        {
            RpcClient = Rpc.ClientFactory.GetClient(Cluster.DevNet);
            StreamingRpcClient = Rpc.ClientFactory.GetStreamingClient(Cluster.DevNet);
            SolendClient = ClientFactory.GetClient(RpcClient, SolendProgram.DevNetProgramIdKey);
        }

        public async void Run()
        {

            var reserve = await SolendClient.GetReserveAsync(new ("5VVLD7BQp8y3bTgyF5ezm1ResyMTR3PhYsT4iHFU8Sxz"));

            var utilization = reserve.ParsedResult.CalculateUtilizationRatio();
            var borrowApr = reserve.ParsedResult.CalculateBorrowApr();
            var borrowApy = reserve.ParsedResult.CalculateBorrowApy();
            var supplyApr = reserve.ParsedResult.CalculateSupplyApr();
            var supplyApy = reserve.ParsedResult.CalculateSupplyApy();

            Console.WriteLine($"Utilization: {utilization:P4} Borrow APR: {borrowApr:P4} Borrow APY: {borrowApy:P4} Supply APR: {supplyApr:P4} Supply APY: {supplyApy:P4}");

            Console.ReadLine();

            var markets = await SolendClient.GetLendingMarketsAsync();
            Console.WriteLine($"{markets.ParsedResult.Count} Lending Markets found.");

            foreach(var market in markets.OriginalRequest.Result)
            {
                var reserves = await SolendClient.GetReservesAsync(new(market.PublicKey));

                Console.WriteLine($"{reserves.ParsedResult.Count} Reserves found for {market.PublicKey}.");
            }

        }
    }
}
