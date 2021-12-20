using Solnet.Programs.Models;
using Solnet.Rpc;
using Solnet.Rpc.Types;
using Solnet.Solend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solnet.Solend
{
    /// <summary>
    /// Specifies functionality for the Solend Client.
    /// </summary>
    public interface ISolendClient
    {
        /// <summary>
        /// The <see cref="IRpcClient"/> instance.
        /// </summary>
        IRpcClient RpcClient { get; }

        /// <summary>
        /// The <see cref="IStreamingRpcClient"/> instance.
        /// </summary>
        IStreamingRpcClient StreamingRpcClient { get; }

        /// <summary>
        /// Gets the Lending Markets. This is an asynchronous operation.
        /// </summary>
        /// <param name="commitment">The confirmation commitment parameter for the RPC call.</param>
        /// <returns>The list of <see cref="LendingMarket"/>s or null in case an error occurred.</returns>
        Task<ProgramAccountsResultWrapper<List<LendingMarket>>> GetLendingMarketsAsync(Commitment commitment = Commitment.Finalized);

        /// <summary>
        /// Gets the Lending Markets.
        /// </summary>
        /// <param name="commitment">The confirmation commitment parameter for the RPC call.</param>
        /// <returns>The list of <see cref="LendingMarket"/>s or null in case an error occurred.</returns>
        ProgramAccountsResultWrapper<List<LendingMarket>> GetLendingMarkets(Commitment commitment = Commitment.Finalized);
    }
}
