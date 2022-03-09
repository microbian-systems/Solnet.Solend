using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
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

            RpcClient = Rpc.ClientFactory.GetClient(Cluster.MainNet, _logger);
            StreamingRpcClient = Rpc.ClientFactory.GetStreamingClient(Cluster.MainNet);
            SolendClient = ClientFactory.GetClient(RpcClient, SolendProgram.MainNetProgramIdKey);
        }

        public async void Run()
        {
            var markets = await SolendClient.GetLendingMarketsAsync();

            Console.WriteLine($"{markets.ParsedResult.Count} Lending Markets found.");
        }
    }
}
