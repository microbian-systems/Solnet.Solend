using Microsoft.Extensions.Logging;
using Solnet.Programs.Abstract;
using Solnet.Programs.Models;
using Solnet.Programs.TokenLending.Models;
using Solnet.Rpc;
using Solnet.Rpc.Models;
using Solnet.Rpc.Types;
using Solnet.Solend.Models;
using Solnet.Wallet;
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
        /// The program id.
        /// </summary>
        private readonly PublicKey _programId;

        /// <summary>
        /// Initialize the Solend client with the given RPC client instance and logger.
        /// </summary>
        /// <param name="rpcClient">The RPC client instance.</param>
        /// <param name="logger">The logger instance</param>
        internal SolendClient(ILogger logger = null, IRpcClient rpcClient = default,
            PublicKey programId = null) : base(rpcClient, logger: logger)
        {
            _programId = programId != null ? programId : SolendProgram.MainNetProgramIdKey;
        }

        /// <inheritdoc cref="ISolendClient.GetLendingMarkets(Commitment)"/>
        public ProgramAccountsResultWrapper<List<Models.LendingMarket>> GetLendingMarkets(Commitment commitment = Commitment.Confirmed)
        => GetLendingMarketsAsync(commitment).Result;

        /// <inheritdoc cref="ISolendClient.GetLendingMarketsAsync(Commitment)"/>
        public async Task<ProgramAccountsResultWrapper<List<Models.LendingMarket>>> GetLendingMarketsAsync(Commitment commitment = Commitment.Confirmed)
        {
            return await GetProgramAccounts<Models.LendingMarket>(_programId, null,
                Models.LendingMarket.ExtraLayout.Length, commitment);
        }

        /// <inheritdoc cref="ISolendClient.GetReserve(PublicKey, Commitment)"/>
        public AccountResultWrapper<Models.Reserve> GetReserve(PublicKey reserve, Commitment commitment = Commitment.Confirmed) 
            => GetReserveAsync(reserve, commitment).Result;

        /// <inheritdoc cref="ISolendClient.GetReserveAsync(PublicKey, Commitment)"/>
        public Task<AccountResultWrapper<Models.Reserve>> GetReserveAsync(PublicKey reserve, Commitment commitment = Commitment.Confirmed)
            => GetAccount<Models.Reserve>(reserve, commitment);

        /// <inheritdoc cref="ISolendClient.GetReserves(PublicKey, Commitment)"/>
        public ProgramAccountsResultWrapper<List<Models.Reserve>> GetReserves(PublicKey lendingMarket, Commitment commitment = Commitment.Confirmed)
        => GetReservesAsync(lendingMarket, commitment).Result;

        /// <inheritdoc cref="ISolendClient.GetReservesAsync(PublicKey, Commitment)"/>
        public async Task<ProgramAccountsResultWrapper<List<Models.Reserve>>> GetReservesAsync(PublicKey lendingMarket, Commitment commitment = Commitment.Confirmed)
        {
            var filters = new List<MemCmp>()
            {
                new MemCmp()
                {
                    Offset = Models.Reserve.Layout.LendingMarketOffset,
                    Bytes = lendingMarket.Key
                }
            };
            return await GetProgramAccounts<Models.Reserve>(_programId, filters,
                Models.Reserve.Layout.Length, commitment);
        }
    }
}
