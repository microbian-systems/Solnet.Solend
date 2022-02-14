using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using Solnet.Rpc;
using Solnet.Wallet;

namespace Solnet.Solend
{
    /// <summary>
    /// The client factory for the Solend Client.
    /// </summary>
    public static class ClientFactory
    {
        /// <summary>
        /// Instantiate the <see cref="SolendClient"/>.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="rpcClient">The RPC Client instance.</param>
        /// <param name="programId"
        /// <returns>The <see cref="SolendClient"/>.</returns>
        public static ISolendClient GetClient(IRpcClient rpcClient = null, PublicKey programId = null,
            ILogger logger = null)
        {
#if DEBUG
            //logger ??= GetDebugLogger();
#endif
            return new SolendClient(logger, rpcClient, programId);
        }

#if DEBUG
        /// <summary>
        /// Get a logger instance for use in debug mode.
        /// </summary>
        /// <returns>The logger.</returns>
        private static ILogger GetDebugLogger()
        {
            return LoggerFactory.Create(x =>
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
        }
#endif
    }
}
