using Solnet.Programs;
using Solnet.Programs.TokenLending;
using Solnet.Rpc.Models;
using Solnet.Wallet;
using System.Collections.Generic;

namespace Solnet.Solend
{
    /// <summary>
    /// Implements the Solend Program methods.
    /// <remarks>
    /// For more information see:
    /// https://solend.fi
    /// https://github.com/solend-protocol/solana-program-library/tree/master/token-lending
    /// </remarks>
    /// </summary>
    public class SolendProgram : TokenLendingProgram
    {

        /// <summary>
        /// Solend Program Mainnet Program ID
        /// </summary>
        public static new readonly PublicKey ProgramIdKey =
            new PublicKey("So1endDq2YkqhipRh3WViPa8hdiSpxWy6z3Z6tMCpAo");

        /// <summary>
        /// Solend Program Name.
        /// </summary>
        public static new readonly string ProgramName = "Solend Program";

        /// <summary>
        /// 
        /// </summary>
        /// <param name="liquidityAmount"></param>
        /// <param name="sourceLiquidity"></param>
        /// <param name="destinationCollateral"></param>
        /// <param name="reserve"></param>
        /// <param name="reserveLiquiditySupply"></param>
        /// <param name="reserveCollateralMint"></param>
        /// <param name="lendingMarket"></param>
        /// <param name="destinationDepositCollateral"></param>
        /// <param name="obligation"></param>
        /// <param name="obligationOwner"></param>
        /// <param name="reserveLiquidityPyth"></param>
        /// <param name="reserveLiquiditySwitchboard"></param>
        /// <param name="userTransferAuthority"></param>
        /// <returns></returns>
        public TransactionInstruction DepositReserveLiquidityAndObligationCollateral(ulong liquidityAmount,
            PublicKey sourceLiquidity, PublicKey destinationCollateral, PublicKey reserve, PublicKey reserveLiquiditySupply,
            PublicKey reserveCollateralMint, PublicKey lendingMarket, PublicKey destinationDepositCollateral,
            PublicKey obligation, PublicKey obligationOwner, PublicKey reserveLiquidityPyth, PublicKey reserveLiquiditySwitchboard,
            PublicKey userTransferAuthority)
        {
            PublicKey lendingMarketAuthority = TokenLendingProgramData.DeriveLendingMarketAuthority(lendingMarket, ProgramIdKey);
            List<AccountMeta> keys = new()
            {
                AccountMeta.Writable(sourceLiquidity, false),
                AccountMeta.Writable(destinationCollateral, false),
                AccountMeta.Writable(reserve, false),
                AccountMeta.Writable(reserveLiquiditySupply, false),
                AccountMeta.Writable(reserveCollateralMint, false),
                AccountMeta.ReadOnly(lendingMarket, false),
                AccountMeta.ReadOnly(lendingMarketAuthority, false),
                AccountMeta.Writable(destinationDepositCollateral, false),
                AccountMeta.Writable(obligation, false),
                AccountMeta.Writable(obligationOwner, true),
                AccountMeta.ReadOnly(reserveLiquidityPyth, false),
                AccountMeta.ReadOnly(reserveLiquiditySwitchboard, false),
                AccountMeta.ReadOnly(userTransferAuthority, true),
                AccountMeta.ReadOnly(SystemProgram.SysVarClockKey, false),
                AccountMeta.ReadOnly(TokenProgram.ProgramIdKey, false)
            };
            return new TransactionInstruction
            {
                ProgramId = ProgramIdKey.KeyBytes,
                Keys = keys,
                Data = SolendProgramData.EncodeDepositReserveLiquidityAndObligationCollateralData(liquidityAmount)
            };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="collateralAmount"></param>
        /// <param name="sourceCollateral"></param>
        /// <param name="destinationCollateral"></param>
        /// <param name="withdrawReserve"></param>
        /// <param name="obligation"></param>
        /// <param name="lendingMarket"></param>
        /// <param name="destinationLiquidity"></param>
        /// <param name="reserveCollateralMint"></param>
        /// <param name="reserveLiquiditySupply"></param>
        /// <param name="obligationOwner"></param>
        /// <param name="userTransferAuthority"></param>
        /// <returns></returns>
        public TransactionInstruction WithdrawObligationCollateralAndRedeemReserveCollateral(ulong collateralAmount,
            PublicKey sourceCollateral, PublicKey destinationCollateral, PublicKey withdrawReserve, PublicKey obligation,
            PublicKey lendingMarket, PublicKey destinationLiquidity, PublicKey reserveCollateralMint, PublicKey reserveLiquiditySupply,
            PublicKey obligationOwner, PublicKey userTransferAuthority)
        {
            PublicKey lendingMarketAuthority = TokenLendingProgramData.DeriveLendingMarketAuthority(lendingMarket, ProgramIdKey);
            List<AccountMeta> keys = new()
            {
                AccountMeta.Writable(sourceCollateral, false),
                AccountMeta.Writable(destinationCollateral, false),
                AccountMeta.Writable(withdrawReserve, false),
                AccountMeta.Writable(obligation, false),
                AccountMeta.ReadOnly(lendingMarket, false),
                AccountMeta.ReadOnly(lendingMarketAuthority, false),
                AccountMeta.Writable(destinationLiquidity, false),
                AccountMeta.Writable(reserveCollateralMint, false),
                AccountMeta.Writable(reserveLiquiditySupply, false),
                AccountMeta.Writable(obligationOwner, true),
                AccountMeta.ReadOnly(userTransferAuthority, true),
                AccountMeta.ReadOnly(SystemProgram.SysVarClockKey, false),
                AccountMeta.ReadOnly(TokenProgram.ProgramIdKey, false)
            };
            return new TransactionInstruction
            {
                ProgramId = ProgramIdKey.KeyBytes,
                Keys = keys,
                Data = SolendProgramData.EncodeDepositReserveLiquidityAndObligationCollateralData(collateralAmount)
            };
        }
    }
}
