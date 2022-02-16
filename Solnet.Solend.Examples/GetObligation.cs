using Solnet.Rpc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solnet.Solend.Examples
{
    public class GetObligation : IRunnableExample
    {
        private IRpcClient RpcClient;
        private ISolendClient SolendClient;
        private Wallet.Wallet Wallet;

        public GetObligation()
        {
            RpcClient = Rpc.ClientFactory.GetClient(Cluster.DevNet);
            SolendClient = ClientFactory.GetClient(RpcClient, SolendProgram.DevNetProgramIdKey);
        }

        public async void Run()
        {
            var obligations = await SolendClient.GetObligationsAsync(new("hoakwpFB8UoLnPpLC56gsjpY7XbVwaCuRQRMQzN5TVh"));

            Console.ReadLine();
        }
    }
}
