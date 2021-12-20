using Microsoft.Extensions.Logging;
using Solnet.Programs.Abstract;
using Solnet.Programs.Models;
using Solnet.Rpc;
using Solnet.Rpc.Types;
using Solnet.Solend.Models;
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
        /// <param name="logger">The logger instance</param>
        public SolendClient(IRpcClient rpcClient, ILogger logger = null) 
            : base(rpcClient, logger: logger) { }

        /// <inheritdoc cref="ISolendClient.GetLendingMarkets(Commitment)"/>
        public ProgramAccountsResultWrapper<List<LendingMarket>> GetLendingMarkets(Commitment commitment = Commitment.Finalized)
            => GetLendingMarketsAsync(commitment).Result;

        /// <inheritdoc cref="ISolendClient.GetLendingMarketsAsync(Commitment)"/>
        public async Task<ProgramAccountsResultWrapper<List<LendingMarket>>> GetLendingMarketsAsync(Commitment commitment = Commitment.Finalized)
        {
            return await GetProgramAccounts<LendingMarket>(SolendProgram.ProgramIdKey, null,
                LendingMarket.ExtraLayout.Length, commitment);
        }
    }
}
