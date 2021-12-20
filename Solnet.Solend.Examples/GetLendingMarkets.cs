using Solnet.Rpc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solnet.Solend.Examples
{
    public class GetLendingMarkets : IRunnableExample
    {
        private static readonly IRpcClient RpcClient = Solnet.Rpc.ClientFactory.GetClient(Cluster.MainNet);
        private static readonly IStreamingRpcClient StreamingRpcClient =
            Solnet.Rpc.ClientFactory.GetStreamingClient(Cluster.MainNet);

        private readonly ISolendClient SolendClient;

        public GetLendingMarkets()
        {
            SolendClient = ClientFactory.GetClient(RpcClient, StreamingRpcClient);
        }

        public void Run()
        {
            var markets = SolendClient.GetLendingMarkets();

            Console.WriteLine($"{markets.ParsedResult.Count} Lending Markets found.");
        }
    }
}
