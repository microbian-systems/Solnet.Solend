using Solnet.Programs.Models;
using Solnet.Programs.TokenLending.Models;
using Solnet.Rpc;
using Solnet.Rpc.Types;
using Solnet.Solend.Models;
using Solnet.Wallet;
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
        /// Gets the <see cref="LendingMarket"/>s. This is an asynchronous operation.
        /// </summary>
        /// <param name="commitment">The confirmation commitment parameter for the RPC call.</param>
        /// <returns>The list of <see cref="LendingMarket"/>s or null in case an error occurred.</returns>
        Task<ProgramAccountsResultWrapper<List<Models.LendingMarket>>> GetLendingMarketsAsync(Commitment commitment = Commitment.Finalized);

        /// <summary>
        /// Gets the <see cref="LendingMarket"/>s.
        /// </summary>
        /// <param name="commitment">The confirmation commitment parameter for the RPC call.</param>
        /// <returns>The list of <see cref="LendingMarket"/>s or null in case an error occurred.</returns>
        ProgramAccountsResultWrapper<List<Models.LendingMarket>> GetLendingMarkets(Commitment commitment = Commitment.Finalized);

        /// <summary>
        /// Gets the <see cref="Reserve"/>s for a given lending market. This is an asynchronous operation.
        /// </summary>
        /// <param name="commitment">The confirmation commitment parameter for the RPC call.</param>
        /// <param name="lendingMarket">The public key of the lending market.</param>
        /// <returns>The list of <see cref="Reserve"/>s or null in case an error occurred.</returns>
        Task<ProgramAccountsResultWrapper<List<Models.Reserve>>> GetReservesAsync(PublicKey lendingMarket, Commitment commitment = Commitment.Finalized);

        /// <summary>
        /// Gets the <see cref="Reserve"/>s for a given lending market.
        /// </summary>
        /// <param name="commitment">The confirmation commitment parameter for the RPC call.</param>
        /// <param name="lendingMarket">The public key of the lending market.</param>
        /// <returns>The list of <see cref="Reserve"/>s or null in case an error occurred.</returns>
        ProgramAccountsResultWrapper<List<Models.Reserve>> GetReserves(PublicKey lendingMarket, Commitment commitment = Commitment.Finalized);

        /// <summary>
        /// Gets the <see cref="Reserve"/>s fwith the given public key. This is an asynchronous operation.
        /// </summary>
        /// <param name="commitment">The confirmation commitment parameter for the RPC call.</param>
        /// <param name="reserve">The public key of the reserve.</param>
        /// <returns>The <see cref="Reserve"/>s or null in case an error occurred.</returns>
        Task<AccountResultWrapper<Models.Reserve>> GetReserveAsync(PublicKey reserve, Commitment commitment = Commitment.Finalized);

        /// <summary>
        /// Gets the <see cref="Reserve"/>s with the given public key.
        /// </summary>
        /// <param name="commitment">The confirmation commitment parameter for the RPC call.</param>
        /// <param name="reserve">The public key of the reserve.</param>
        /// <returns>The <see cref="Reserve"/>s or null in case an error occurred.</returns>
        AccountResultWrapper<Models.Reserve> GetReserve(PublicKey reserve, Commitment commitment = Commitment.Finalized);
    }
}
