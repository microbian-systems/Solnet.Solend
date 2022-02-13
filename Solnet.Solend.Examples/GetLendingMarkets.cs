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
        private static IRpcClient RpcClient;
        private static IStreamingRpcClient StreamingRpcClient;

        private static PublicKey Owner = new("hoakwpFB8UoLnPpLC56gsjpY7XbVwaCuRQRMQzN5TVh");

        private readonly ISolendClient SolendClient;

        public GetLendingMarkets()
        {
            RpcClient = Rpc.ClientFactory.GetClient(Cluster.DevNet);
            StreamingRpcClient = Rpc.ClientFactory.GetStreamingClient(Cluster.DevNet);
            SolendClient = ClientFactory.GetClient(RpcClient, SolendProgram.DevNetProgramIdKey);
        }

        public async void Run()
        {
            var markets = await SolendClient.GetLendingMarketsAsync();

            Console.WriteLine($"{markets.ParsedResult.Count} Lending Markets found.");
        }
    }
}
