using System.Collections.Generic;

namespace Solnet.Solend
{
    /// <summary>
    /// Represents the instruction types for the <see cref="SolendProgram"/> along with a friendly name so as not to use reflection.
    /// <remarks>
    /// For more information see:
    /// https://solend.fi
    /// https://github.com/solend-protocol/solana-program-library/tree/master/token-lending
    /// </remarks>
    /// </summary>
    internal static class SolendProgramInstructions
    {
        /// <summary>
        /// Represents the user-friendly names for the instruction types for the <see cref="SolendProgram"/>.
        /// </summary>
        internal static readonly Dictionary<Values, string> Names = new()
        {
            { Values.DepositReserveLiquidityAndObligationCollateral, "Deposit Reserve Liquidity And Obligation Collateral" },
            { Values.WithdrawObligationCollateralAndRedeemReserveCollateral, "Withdraw Obligation Collateral And Redeem Reserve Collateral" },
            { Values.UpdateReserveConfig, "Update Reserve Config" },
        };

        /// <summary>
        /// Represents the instruction types for the <see cref="SolendProgram"/>.
        /// </summary>
        internal enum Values : byte
        {
            /// <summary>
            /// Combines DepositReserveLiquidity and DepositObligationCollateral.
            /// </summary>
            DepositReserveLiquidityAndObligationCollateral = 14,

            /// <summary>
            /// Combines WithdrawObligationCollateral and RedeemReserveCollateral.
            /// </summary>
            WithdrawObligationCollateralAndRedeemReserveCollateral = 15,

            /// <summary>
            /// Updates a reserves config and a reserve price oracle pubkeys.
            /// </summary>
            UpdateReserveConfig = 16
        }
    }
}
