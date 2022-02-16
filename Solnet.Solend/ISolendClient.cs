using Solnet.Programs.Models;
using Solnet.Rpc;
using Solnet.Rpc.Types;
using Solnet.Solend.Models;
using Solnet.Wallet;
using System.Collections.Generic;
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
        Task<ProgramAccountsResultWrapper<List<LendingMarket>>> GetLendingMarketsAsync(Commitment commitment = Commitment.Finalized);

        /// <summary>
        /// Gets the <see cref="LendingMarket"/>s.
        /// </summary>
        /// <param name="commitment">The confirmation commitment parameter for the RPC call.</param>
        /// <returns>The list of <see cref="LendingMarket"/>s or null in case an error occurred.</returns>
        ProgramAccountsResultWrapper<List<LendingMarket>> GetLendingMarkets(Commitment commitment = Commitment.Finalized);

        /// <summary>
        /// Gets the <see cref="Reserve"/>s for a given lending market. This is an asynchronous operation.
        /// </summary>
        /// <param name="commitment">The confirmation commitment parameter for the RPC call.</param>
        /// <param name="lendingMarket">The public key of the lending market.</param>
        /// <returns>The list of <see cref="Reserve"/>s or null in case an error occurred.</returns>
        Task<ProgramAccountsResultWrapper<List<Reserve>>> GetReservesAsync(PublicKey lendingMarket, Commitment commitment = Commitment.Finalized);

        /// <summary>
        /// Gets the <see cref="Reserve"/>s for a given lending market.
        /// </summary>
        /// <param name="commitment">The confirmation commitment parameter for the RPC call.</param>
        /// <param name="lendingMarket">The public key of the lending market.</param>
        /// <returns>The list of <see cref="Reserve"/>s or null in case an error occurred.</returns>
        ProgramAccountsResultWrapper<List<Reserve>> GetReserves(PublicKey lendingMarket, Commitment commitment = Commitment.Finalized);

        /// <summary>
        /// Gets the <see cref="Reserve"/>s with the given public key. This is an asynchronous operation.
        /// </summary>
        /// <param name="commitment">The confirmation commitment parameter for the RPC call.</param>
        /// <param name="reserve">The public key of the reserve.</param>
        /// <returns>The <see cref="Reserve"/>s or null in case an error occurred.</returns>
        Task<AccountResultWrapper<Reserve>> GetReserveAsync(PublicKey reserve, Commitment commitment = Commitment.Finalized);

        /// <summary>
        /// Gets the <see cref="Reserve"/>s with the given public key.
        /// </summary>
        /// <param name="commitment">The confirmation commitment parameter for the RPC call.</param>
        /// <param name="reserve">The public key of the reserve.</param>
        /// <returns>The <see cref="Reserve"/>s or null in case an error occurred.</returns>
        AccountResultWrapper<Reserve> GetReserve(PublicKey reserve, Commitment commitment = Commitment.Finalized);

        /// <summary>
        /// Gets the <see cref="Obligation"/> with the given public key. This is an asynchronous operation.
        /// </summary>
        /// <param name="commitment">The confirmation commitment parameter for the RPC call.</param>
        /// <param name="obligation">The public key of the obligation.</param>
        /// <returns>The <see cref="Obligation"/> or null in case an error occurred.</returns>
        Task<AccountResultWrapper<Obligation>> GetObligationAsync(PublicKey obligation, Commitment commitment = Commitment.Finalized);

        /// <summary>
        /// Gets the <see cref="Models.Obligation"/> with the given public key.
        /// </summary>
        /// <param name="commitment">The confirmation commitment parameter for the RPC call.</param>
        /// <param name="obligation">The public key of the obligation.</param>
        /// <returns>The <see cref="Obligation"/> or null in case an error occurred.</returns>
        AccountResultWrapper<Obligation> GetObligation(PublicKey obligation, Commitment commitment = Commitment.Finalized);

        /// <summary>
        /// Gets all <see cref="Models.Obligation"/>s with for the given owner. This is an asynchronous operation.
        /// </summary>
        /// <param name="commitment">The confirmation commitment parameter for the RPC call.</param>
        /// <param name="owner">The public key of the owner.</param>
        /// <param name="lendingMarket">The public key of the lending market.</param>
        /// <returns>The <see cref="Obligation"/>s or null in case an error occurred.</returns>
        Task<ProgramAccountsResultWrapper<List<Obligation>>> GetObligationsAsync(PublicKey owner = null, PublicKey lendingMarket = null, Commitment commitment = Commitment.Finalized);

        /// <summary>
        /// Gets all <see cref="Models.Obligation"/>s with for the given owner.
        /// </summary>
        /// <param name="commitment">The confirmation commitment parameter for the RPC call.</param>
        /// <param name="owner">The public key of the owner.</param>
        /// <param name="lendingMarket">The public key of the lending market.</param>
        /// <returns>The <see cref="Obligation"/>s or null in case an error occurred.</returns>
        ProgramAccountsResultWrapper<List<Obligation>> GetObligations(PublicKey owner = null, PublicKey lendingMarket = null, Commitment commitment = Commitment.Finalized);
    }
}
