using Microsoft.Extensions.Logging;
using Solnet.Programs.Abstract;
using Solnet.Programs.Models;
using Solnet.Rpc;
using Solnet.Rpc.Models;
using Solnet.Rpc.Types;
using Solnet.Solend.Models;
using Solnet.Wallet;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Solnet.Solend
{
    /// <summary>
    /// Implements functionality for Solend.
    /// </summary>
    public class SolendClient : BaseClient, ISolendClient
    {
        /// <summary>
        /// Initialize the Solend client with the given RPC client instance and logger.
        /// </summary>
        /// <param name="rpcClient">The RPC client instance.</param>
        /// <param name="logger">The logger instance.</param>
        /// <param name="programId">The program id.</param>
        internal SolendClient(ILogger logger = null, IRpcClient rpcClient = default,
            PublicKey programId = null) : base(rpcClient, null, programId ?? SolendProgram.MainNetProgramIdKey)
        {
        }

        /// <inheritdoc cref="ISolendClient.GetLendingMarkets(Commitment)"/>
        public ProgramAccountsResultWrapper<List<LendingMarket>> GetLendingMarkets(Commitment commitment = Commitment.Confirmed)
        => GetLendingMarketsAsync(commitment).Result;

        /// <inheritdoc cref="ISolendClient.GetLendingMarketsAsync(Commitment)"/>
        public async Task<ProgramAccountsResultWrapper<List<LendingMarket>>> GetLendingMarketsAsync(Commitment commitment = Commitment.Confirmed)
        {
            return await GetProgramAccounts<LendingMarket>(ProgramIdKey, null,
                LendingMarket.Layout.Length, commitment);
        }

        /// <inheritdoc cref="ISolendClient.GetObligation(PublicKey, Commitment)"/>
        public AccountResultWrapper<Obligation> GetObligation(PublicKey obligation, Commitment commitment = Commitment.Finalized)
            => GetObligationAsync(obligation, commitment).Result;

        /// <inheritdoc cref="ISolendClient.GetObligationAsync(PublicKey, Commitment)"/>
        public Task<AccountResultWrapper<Obligation>> GetObligationAsync(PublicKey obligation, Commitment commitment = Commitment.Finalized)
            => GetAccount<Obligation>(obligation, commitment);

        /// <inheritdoc cref="ISolendClient.GetObligations(PublicKey, PublicKey, Commitment)"/>
        public ProgramAccountsResultWrapper<List<Obligation>> GetObligations(PublicKey owner, PublicKey lendingMarket, Commitment commitment = Commitment.Finalized)
            => GetObligationsAsync(owner, lendingMarket, commitment).Result;

        /// <inheritdoc cref="ISolendClient.GetObligationsAsync(PublicKey, PublicKey, Commitment)"/>
        public async Task<ProgramAccountsResultWrapper<List<Obligation>>> GetObligationsAsync(PublicKey owner = null, PublicKey lendingMarket = null, Commitment commitment = Commitment.Finalized)
        {
            if (owner == null && lendingMarket == null)
                throw new ArgumentNullException(nameof(owner));

            var filters = new List<MemCmp>();

            if (owner != null)
                filters.Add(
                    new MemCmp()
                    {
                        Offset = Obligation.Layout.OwnerOffset,
                        Bytes = owner
                    });

            if(lendingMarket != null)
                filters.Add(
                    new MemCmp()
                    {
                        Offset = Obligation.Layout.LendingMarketOffset,
                        Bytes = lendingMarket
                    });
            return await GetProgramAccounts<Obligation>(ProgramIdKey, filters,
                Obligation.Layout.Length, commitment);
        }

        /// <inheritdoc cref="ISolendClient.GetReserve(PublicKey, Commitment)"/>
        public AccountResultWrapper<Reserve> GetReserve(PublicKey reserve, Commitment commitment = Commitment.Confirmed)
            => GetReserveAsync(reserve, commitment).Result;

        /// <inheritdoc cref="ISolendClient.GetReserveAsync(PublicKey, Commitment)"/>
        public Task<AccountResultWrapper<Reserve>> GetReserveAsync(PublicKey reserve, Commitment commitment = Commitment.Confirmed)
            => GetAccount<Reserve>(reserve, commitment);

        /// <inheritdoc cref="ISolendClient.GetLendingMarket(PublicKey, Commitment)"/>
        public AccountResultWrapper<LendingMarket> GetLendingMarket(PublicKey lendingMarket, Commitment commitment = Commitment.Confirmed)
            => GetLendingMarketAsync(lendingMarket, commitment).Result;

        /// <inheritdoc cref="ISolendClient.GetLendingMarketAsync(PublicKey, Commitment)"/>
        public Task<AccountResultWrapper<LendingMarket>> GetLendingMarketAsync(PublicKey lendingMarket, Commitment commitment = Commitment.Confirmed)
            => GetAccount<LendingMarket>(lendingMarket, commitment);

        /// <inheritdoc cref="ISolendClient.GetReserves(PublicKey, Commitment)"/>
        public ProgramAccountsResultWrapper<List<Reserve>> GetReserves(PublicKey lendingMarket, Commitment commitment = Commitment.Confirmed)
        => GetReservesAsync(lendingMarket, commitment).Result;

        /// <inheritdoc cref="ISolendClient.GetReservesAsync(PublicKey, Commitment)"/>
        public async Task<ProgramAccountsResultWrapper<List<Reserve>>> GetReservesAsync(PublicKey lendingMarket, Commitment commitment = Commitment.Confirmed)
        {
            var filters = new List<MemCmp>()
            {
                new MemCmp()
                {
                    Offset = Reserve.Layout.LendingMarketOffset,
                    Bytes = lendingMarket.Key
                }
            };
            return await GetProgramAccounts<Reserve>(ProgramIdKey, filters,
                Reserve.Layout.Length, commitment);
        }
    }
}
