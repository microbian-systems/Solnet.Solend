using Solnet.Programs;
using Solnet.Programs.Abstract;
using Solnet.Programs.Utilities;
using Solnet.Rpc;
using Solnet.Rpc.Models;
using Solnet.Wallet;
using System;
using System.Collections.Generic;
using System.Linq;

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
    public class SolendProgram : BaseProgram
    {
        /// <summary>
        /// Solend Program MainNet Program ID.
        /// </summary>
        public static readonly PublicKey MainNetProgramIdKey = new("So1endDq2YkqhipRh3WViPa8hdiSpxWy6z3Z6tMCpAo");

        /// <summary>
        /// Solend Program DevNet Program ID.
        /// </summary>
        public static readonly PublicKey DevNetProgramIdKey = new("ALend7Ketfx5bxh6ghsCDXAoDrhvEmsXT3cynB6aPLgx");

        /// <summary>
        /// Solend Program Name.
        /// </summary>
        public const string DefaultProgramName = "Solend Program";

        /// <summary>
        /// Initialize the <see cref="SolendProgram"/> with the given program id key and program name.
        /// </summary>
        /// <param name="programIdKey">The program id key.</param>
        /// <param name="programName">The program name.</param>
        public SolendProgram(PublicKey programIdKey, string programName = DefaultProgramName)
            : base(programIdKey, programName) { }

        /// <summary>
        /// Initialize the <see cref="SolendProgram"/> for <see cref="Cluster.DevNet"/>.
        /// </summary>
        /// <returns>The <see cref="SolendProgram"/> instance.</returns>
        public static SolendProgram CreateDevNet() => new(DevNetProgramIdKey);

        /// <summary>
        /// Initialize the <see cref="SolendProgram"/> for <see cref="Cluster.MainNet"/>.
        /// </summary>
        /// <returns>The <see cref="SolendProgram"/> instance.</returns>
        public static SolendProgram CreateMainNet() => new(MainNetProgramIdKey);

        /// <summary>
        /// Initialize a new <see cref="TransactionInstruction"/> for the <see cref="SolendProgramInstructions.Values.RefreshReserve"/> method.
        /// </summary>
        /// <param name="programIdKey">The public key of the program.</param>
        /// <param name="reserve">The reserve.</param>
        /// <param name="reserveLiquidityPythOracle">The reserve liquidity pyth oracle.</param>
        /// <param name="reserveLiquiditySwitchboardOracle">The reserve liquidity switchboard oracle.</param>
        /// <returns>The <see cref="TransactionInstruction"/>.</returns>
        public static TransactionInstruction RefreshReserve(PublicKey programIdKey, PublicKey reserve,
            PublicKey reserveLiquidityPythOracle, PublicKey reserveLiquiditySwitchboardOracle)
        {

            List<AccountMeta> keys = new()
            {
                AccountMeta.Writable(reserve, false),
                AccountMeta.ReadOnly(reserveLiquidityPythOracle, false),
                AccountMeta.ReadOnly(reserveLiquiditySwitchboardOracle, false),
                AccountMeta.ReadOnly(SysVars.ClockKey, false),
            };
            return new TransactionInstruction
            {
                ProgramId = programIdKey,
                Data = SolendProgramData.EncodeRefreshReserveData(),
                Keys = keys
            };
        }

        /// <summary>
        /// Initialize a new <see cref="TransactionInstruction"/> for the <see cref="SolendProgramInstructions.Values.RefreshReserve"/> method.
        /// </summary>
        /// <param name="reserve">The reserve.</param>
        /// <param name="reserveLiquidityPythOracle">The reserve liquidity pyth oracle.</param>
        /// <param name="reserveLiquiditySwitchboardOracle">The reserve liquidity switchboard oracle.</param>
        /// <returns>The <see cref="TransactionInstruction"/>.</returns>
        public TransactionInstruction RefreshReserve(PublicKey reserve,
            PublicKey reserveLiquidityPythOracle, PublicKey reserveLiquiditySwitchboardOracle) 
            => RefreshReserve(ProgramIdKey, reserve, reserveLiquidityPythOracle, reserveLiquiditySwitchboardOracle);

        /// <summary>
        /// Initialize a new <see cref="TransactionInstruction"/> for the <see cref="SolendProgramInstructions.Values.DepositReserveLiquidity"/> method.
        /// </summary>
        /// <param name="programIdKey">The public key of the program.</param>
        /// <param name="liquidityAmount">The liquidity amount.</param>
        /// <param name="sourceLiquidity">The source liquidity token account.</param>
        /// <param name="destinationCollateral">The destination collateral token account.</param>
        /// <param name="reserve">The reserve.</param>
        /// <param name="reserveLiquiditySupply">The reserve liquidity supply.</param>
        /// <param name="reserveCollateralMint">The reserve collateral mint.</param>
        /// <param name="lendingMarket">The lending market.</param>
        /// <param name="userTransferAuthority">The user transfer authority.</param>
        /// <returns>The <see cref="TransactionInstruction"/>.</returns>
        public static TransactionInstruction DepositReserveLiquidity(PublicKey programIdKey, ulong liquidityAmount,
            PublicKey sourceLiquidity, PublicKey destinationCollateral, PublicKey reserve, PublicKey reserveLiquiditySupply,
            PublicKey reserveCollateralMint, PublicKey lendingMarket, PublicKey userTransferAuthority)
        {
            PublicKey lendingMarketAuthority = SolendProgramData.DeriveLendingMarketAuthority(lendingMarket, programIdKey);

            List<AccountMeta> keys = new()
            {
                AccountMeta.Writable(sourceLiquidity, false),
                AccountMeta.Writable(destinationCollateral, false),
                AccountMeta.Writable(reserve, false),
                AccountMeta.Writable(reserveLiquiditySupply, false),
                AccountMeta.Writable(reserveCollateralMint, false),
                AccountMeta.ReadOnly(lendingMarket, false),
                AccountMeta.ReadOnly(lendingMarketAuthority, false),
                AccountMeta.ReadOnly(userTransferAuthority, true),
                AccountMeta.ReadOnly(SysVars.ClockKey, false),
                AccountMeta.ReadOnly(TokenProgram.ProgramIdKey, false)
            };
            return new TransactionInstruction
            {
                ProgramId = programIdKey,
                Data = SolendProgramData.EncodeDepositReserveLiquidityData(liquidityAmount),
                Keys = keys
            };
        }

        /// <summary>
        /// Initialize a new <see cref="TransactionInstruction"/> for the <see cref="SolendProgramInstructions.Values.DepositReserveLiquidity"/> method.
        /// </summary>
        /// <param name="liquidityAmount">The liquidity amount.</param>
        /// <param name="sourceLiquidity">The source liquidity token account.</param>
        /// <param name="destinationCollateral">The destination collateral token account.</param>
        /// <param name="reserve">The reserve.</param>
        /// <param name="reserveLiquiditySupply">The reserve liquidity supply.</param>
        /// <param name="reserveCollateralMint">The reserve collateral mint.</param>
        /// <param name="lendingMarket">The lending market.</param>
        /// <param name="userTransferAuthority">The user transfer authority.</param>
        /// <returns>The <see cref="TransactionInstruction"/>.</returns>
        public TransactionInstruction DepositReserveLiquidity(ulong liquidityAmount,
            PublicKey sourceLiquidity, PublicKey destinationCollateral, PublicKey reserve, PublicKey reserveLiquiditySupply,
            PublicKey reserveCollateralMint, PublicKey lendingMarket, PublicKey userTransferAuthority)
            => DepositReserveLiquidity(ProgramIdKey, liquidityAmount, sourceLiquidity, destinationCollateral, reserve,
                reserveLiquiditySupply, reserveCollateralMint, lendingMarket, userTransferAuthority);

        /// <summary>
        /// Initialize a new <see cref="TransactionInstruction"/> for the <see cref="SolendProgramInstructions.Values.RedeemReserveCollateral"/> method.
        /// </summary>
        /// <param name="programIdKey">The public key of the program.</param>
        /// <param name="collateralAmount">The collateral amount.</param>
        /// <param name="sourceCollateral">The source collateral token account.</param>
        /// <param name="destinationLiquidity">The destination collateral token account.</param>
        /// <param name="reserve">The reserve.</param>
        /// <param name="reserveCollateralMint">The reserve collateral mint.</param>
        /// <param name="reserveLiquiditySupply">The reserve liquidity supply.</param>
        /// <param name="lendingMarket">The lending market.</param>
        /// <param name="userTransferAuthority">The user transfer authority.</param>
        /// <returns>The <see cref="TransactionInstruction"/>.</returns>
        public static TransactionInstruction RedeemReserveCollateral(PublicKey programIdKey, ulong collateralAmount,
            PublicKey sourceCollateral, PublicKey destinationLiquidity, PublicKey reserve, PublicKey reserveCollateralMint,
            PublicKey reserveLiquiditySupply, PublicKey lendingMarket, PublicKey userTransferAuthority)
        {
            PublicKey lendingMarketAuthority = SolendProgramData.DeriveLendingMarketAuthority(lendingMarket, programIdKey);

            List<AccountMeta> keys = new()
            {
                AccountMeta.Writable(sourceCollateral, false),
                AccountMeta.Writable(destinationLiquidity, false),
                AccountMeta.Writable(reserve, false),
                AccountMeta.Writable(reserveCollateralMint, false),
                AccountMeta.Writable(reserveLiquiditySupply, false),
                AccountMeta.ReadOnly(lendingMarket, false),
                AccountMeta.ReadOnly(lendingMarketAuthority, false),
                AccountMeta.ReadOnly(userTransferAuthority, true),
                AccountMeta.ReadOnly(SysVars.ClockKey, false),
                AccountMeta.ReadOnly(TokenProgram.ProgramIdKey, false)
            };
            return new TransactionInstruction
            {
                ProgramId = programIdKey,
                Data = SolendProgramData.EncodeRedeemReserveCollateralData(collateralAmount),
                Keys = keys
            };
        }

        /// <summary>
        /// Initialize a new <see cref="TransactionInstruction"/> for the <see cref="SolendProgramInstructions.Values.RedeemReserveCollateral"/> method.
        /// </summary>
        /// <param name="collateralAmount">The collateral amount.</param>
        /// <param name="sourceCollateral">The source collateral token account.</param>
        /// <param name="destinationLiquidity">The destination collateral token account.</param>
        /// <param name="reserve">The reserve.</param>
        /// <param name="reserveCollateralMint">The reserve collateral mint.</param>
        /// <param name="reserveLiquiditySupply">The reserve liquidity supply.</param>
        /// <param name="lendingMarket">The lending market.</param>
        /// <param name="userTransferAuthority">The user transfer authority.</param>
        /// <returns>The <see cref="TransactionInstruction"/>.</returns>
        public TransactionInstruction RedeemReserveCollateral(ulong collateralAmount,
            PublicKey sourceCollateral, PublicKey destinationLiquidity, PublicKey reserve, PublicKey reserveCollateralMint,
            PublicKey reserveLiquiditySupply, PublicKey lendingMarket, PublicKey userTransferAuthority)
            => RedeemReserveCollateral(ProgramIdKey, collateralAmount, sourceCollateral, destinationLiquidity, reserve,
                reserveCollateralMint, reserveLiquiditySupply, lendingMarket, userTransferAuthority);

        /// <summary>
        /// Initialize a new <see cref="TransactionInstruction"/> for the <see cref="SolendProgramInstructions.Values.InitializeObligation"/> method.
        /// </summary>
        /// <param name="programIdKey">The public key of the program.</param>
        /// <param name="obligation">The obligation.</param>
        /// <param name="lendingMarket">The lending market.</param>
        /// <param name="obligationOwner">The obligation owner.</param>
        /// <returns>The <see cref="TransactionInstruction"/>.</returns>
        public static TransactionInstruction InitializeObligation(PublicKey programIdKey, PublicKey obligation,
            PublicKey lendingMarket, PublicKey obligationOwner)
        {
            List<AccountMeta> keys = new()
            {
                AccountMeta.Writable(obligation, false),
                AccountMeta.ReadOnly(lendingMarket, false),
                AccountMeta.ReadOnly(obligationOwner, true),
                AccountMeta.ReadOnly(SysVars.ClockKey, false),
                AccountMeta.ReadOnly(SysVars.RentKey, false),
                AccountMeta.ReadOnly(TokenProgram.ProgramIdKey, false)
            };
            return new TransactionInstruction
            {
                ProgramId = programIdKey,
                Data = SolendProgramData.EncodeInitializeObligationData(),
                Keys = keys
            };
        }

        /// <summary>
        /// Initialize a new <see cref="TransactionInstruction"/> for the <see cref="SolendProgramInstructions.Values.InitializeObligation"/> method.
        /// </summary>
        /// <param name="obligation">The obligation.</param>
        /// <param name="lendingMarket">The lending market.</param>
        /// <param name="obligationOwner">The obligation owner.</param>
        /// <returns>The <see cref="TransactionInstruction"/>.</returns>
        public TransactionInstruction InitializeObligation(PublicKey obligation,
            PublicKey lendingMarket, PublicKey obligationOwner)
            => InitializeObligation(ProgramIdKey, obligation, lendingMarket, obligationOwner);

        /// <summary>
        /// Initialize a new <see cref="TransactionInstruction"/> for the <see cref="SolendProgramInstructions.Values.RefreshObligation"/> method.
        /// </summary>
        /// <param name="programIdKey">The public key of the program.</param>
        /// <param name="obligation">The obligation.</param>
        /// <param name="reserves">The reserves.</param>
        /// <returns>The <see cref="TransactionInstruction"/>.</returns>
        public static TransactionInstruction RefreshObligation(PublicKey programIdKey, PublicKey obligation,
            IList<PublicKey> reserves)
        {
            List<AccountMeta> keys = new()
            {
                AccountMeta.Writable(obligation, false),
                AccountMeta.ReadOnly(SysVars.ClockKey, false),
            };

            keys.AddRange(reserves.Select(x => AccountMeta.ReadOnly(x, false)));

            return new TransactionInstruction
            {
                ProgramId = programIdKey,
                Data = SolendProgramData.EncodeRefreshObligationData(),
                Keys = keys
            };
        }

        /// <summary>
        /// Initialize a new <see cref="TransactionInstruction"/> for the <see cref="SolendProgramInstructions.Values.RefreshObligation"/> method.
        /// </summary>
        /// <param name="obligation">The obligation.</param>
        /// <param name="reserves">The reserves.</param>
        /// <returns>The <see cref="TransactionInstruction"/>.</returns>
        public TransactionInstruction RefreshObligation(PublicKey obligation,
            IList<PublicKey> reserves)
            => RefreshObligation(ProgramIdKey, obligation, reserves);

        /// <summary>
        /// Initialize a new <see cref="TransactionInstruction"/> for the <see cref="SolendProgramInstructions.Values.DepositObligationCollateral"/> method.
        /// </summary>
        /// <param name="programIdKey">The public key of the program.</param>
        /// <param name="collateralAmount">The collateral amount.</param>
        /// <param name="sourceCollateral">The source collateral token account.</param>
        /// <param name="destinationCollateral">The destination collateral token account.</param>
        /// <param name="depositReserve">The deposit reserve.</param>
        /// <param name="obligation">The obligation.</param>
        /// <param name="lendingMarket">The lending market.</param>
        /// <param name="obligationOwner">The obligation owner.</param>
        /// <param name="userTransferAuthority">The user transfer authority.</param>
        /// <returns>The <see cref="TransactionInstruction"/>.</returns>
        public static TransactionInstruction DepositObligationCollateral(PublicKey programIdKey, ulong collateralAmount,
            PublicKey sourceCollateral, PublicKey destinationCollateral, PublicKey depositReserve, PublicKey obligation,
            PublicKey lendingMarket, PublicKey obligationOwner, PublicKey userTransferAuthority)
        {
            List<AccountMeta> keys = new()
            {
                AccountMeta.Writable(sourceCollateral, false),
                AccountMeta.Writable(destinationCollateral, false),
                AccountMeta.ReadOnly(depositReserve, false),
                AccountMeta.Writable(obligation, false),
                AccountMeta.ReadOnly(lendingMarket, false),
                AccountMeta.ReadOnly(obligationOwner, true),
                AccountMeta.ReadOnly(userTransferAuthority, true),
                AccountMeta.ReadOnly(SysVars.ClockKey, false),
                AccountMeta.ReadOnly(TokenProgram.ProgramIdKey, false)
            };

            return new TransactionInstruction
            {
                ProgramId = programIdKey,
                Data = SolendProgramData.EncodeDepositObligationCollateralData(collateralAmount),
                Keys = keys
            };
        }

        /// <summary>
        /// Initialize a new <see cref="TransactionInstruction"/> for the <see cref="SolendProgramInstructions.Values.DepositObligationCollateral"/> method.
        /// </summary>
        /// <param name="collateralAmount">The collateral amount.</param>
        /// <param name="sourceCollateral">The source collateral token account.</param>
        /// <param name="destinationCollateral">The destination collateral token account.</param>
        /// <param name="depositReserve">The deposit reserve.</param>
        /// <param name="obligation">The obligation.</param>
        /// <param name="lendingMarket">The lending market.</param>
        /// <param name="obligationOwner">The obligation owner.</param>
        /// <param name="userTransferAuthority">The user transfer authority.</param>
        /// <returns>The <see cref="TransactionInstruction"/>.</returns>
        public TransactionInstruction DepositObligationCollateral(ulong collateralAmount,
            PublicKey sourceCollateral, PublicKey destinationCollateral, PublicKey depositReserve, PublicKey obligation,
            PublicKey lendingMarket, PublicKey obligationOwner, PublicKey userTransferAuthority)
            => DepositObligationCollateral(ProgramIdKey, collateralAmount, sourceCollateral, destinationCollateral,
                depositReserve, obligation, lendingMarket, obligationOwner, userTransferAuthority);

        /// <summary>
        /// Initialize a new <see cref="TransactionInstruction"/> for the <see cref="SolendProgramInstructions.Values.WithdrawObligationCollateral"/> method.
        /// </summary>
        /// <param name="programIdKey">The public key of the program.</param>
        /// <param name="collateralAmount">The collateral amount.</param>
        /// <param name="sourceCollateral">The source collateral token account.</param>
        /// <param name="destinationCollateral">The destination collateral token account.</param>
        /// <param name="withdrawReserve">The withdraw reserve.</param>
        /// <param name="obligation">The obligation.</param>
        /// <param name="lendingMarket">The lending market.</param>
        /// <param name="obligationOwner">The obligation owner.</param>
        /// <returns>The <see cref="TransactionInstruction"/>.</returns>
        public static TransactionInstruction WithdrawObligationCollateral(PublicKey programIdKey, ulong collateralAmount,
            PublicKey sourceCollateral, PublicKey destinationCollateral, PublicKey withdrawReserve, PublicKey obligation,
            PublicKey lendingMarket, PublicKey obligationOwner)
        {
            PublicKey lendingMarketAuthority = SolendProgramData.DeriveLendingMarketAuthority(lendingMarket, programIdKey);

            List<AccountMeta> keys = new()
            {
                AccountMeta.Writable(sourceCollateral, false),
                AccountMeta.Writable(destinationCollateral, false),
                AccountMeta.ReadOnly(withdrawReserve, false),
                AccountMeta.Writable(obligation, false),
                AccountMeta.ReadOnly(lendingMarket, false),
                AccountMeta.ReadOnly(lendingMarketAuthority, false),
                AccountMeta.ReadOnly(obligationOwner, true),
                AccountMeta.ReadOnly(SysVars.ClockKey, false),
                AccountMeta.ReadOnly(TokenProgram.ProgramIdKey, false)
            };

            return new TransactionInstruction
            {
                ProgramId = programIdKey,
                Data = SolendProgramData.EncodeWithdrawObligationCollateralData(collateralAmount),
                Keys = keys
            };
        }

        /// <summary>
        /// Initialize a new <see cref="TransactionInstruction"/> for the <see cref="SolendProgramInstructions.Values.WithdrawObligationCollateral"/> method.
        /// </summary>
        /// <param name="collateralAmount">The collateral amount.</param>
        /// <param name="sourceCollateral">The source collateral token account.</param>
        /// <param name="destinationCollateral">The destination collateral token account.</param>
        /// <param name="withdrawReserve">The withdraw reserve.</param>
        /// <param name="obligation">The obligation.</param>
        /// <param name="lendingMarket">The lending market.</param>
        /// <param name="obligationOwner">The obligation owner.</param>
        /// <returns>The <see cref="TransactionInstruction"/>.</returns>
        public TransactionInstruction WithdrawObligationCollateral(ulong collateralAmount,
            PublicKey sourceCollateral, PublicKey destinationCollateral, PublicKey withdrawReserve, PublicKey obligation,
            PublicKey lendingMarket, PublicKey obligationOwner)
            => WithdrawObligationCollateral(ProgramIdKey, collateralAmount, sourceCollateral, destinationCollateral,
                withdrawReserve, obligation, lendingMarket, obligationOwner);

        /// <summary>
        /// Initialize a new <see cref="TransactionInstruction"/> for the <see cref="SolendProgramInstructions.Values.BorrowObligationLiquidity"/> method.
        /// </summary>
        /// <param name="programIdKey">The public key of the program.</param>
        /// <param name="liquidityAmount">The liquidity amount.</param>
        /// <param name="sourceLiquidity">The source liquidity token account.</param>
        /// <param name="destinationLiquidity">The destination liquidity token account.</param>
        /// <param name="borrowReserve">The borrow reserve.</param>
        /// <param name="borrowReserveLiquidityFeeReceiver">The borrow reserve liquidity fee receiver.</param>
        /// <param name="obligation">The obligation.</param>
        /// <param name="lendingMarket">The lending market.</param>
        /// <param name="obligationOwner">The obligation owner.</param>
        /// <param name="hostFeeReceiver">The fee receiver.</param>
        /// <returns>The <see cref="TransactionInstruction"/>.</returns>
        public static TransactionInstruction BorrowObligationLiquidity(PublicKey programIdKey, ulong liquidityAmount,
            PublicKey sourceLiquidity, PublicKey destinationLiquidity, PublicKey borrowReserve,
            PublicKey borrowReserveLiquidityFeeReceiver, PublicKey obligation, PublicKey lendingMarket,
            PublicKey obligationOwner, PublicKey hostFeeReceiver = null)
        {
            PublicKey lendingMarketAuthority = SolendProgramData.DeriveLendingMarketAuthority(lendingMarket, programIdKey);

            List<AccountMeta> keys = new()
            {
                AccountMeta.Writable(sourceLiquidity, false),
                AccountMeta.Writable(destinationLiquidity, false),
                AccountMeta.Writable(borrowReserve, false),
                AccountMeta.Writable(borrowReserveLiquidityFeeReceiver, false),
                AccountMeta.Writable(obligation, false),
                AccountMeta.ReadOnly(lendingMarket, false),
                AccountMeta.ReadOnly(lendingMarketAuthority, false),
                AccountMeta.ReadOnly(obligationOwner, true),
                AccountMeta.ReadOnly(SysVars.ClockKey, false),
                AccountMeta.ReadOnly(TokenProgram.ProgramIdKey, false)
            };

            if (hostFeeReceiver != null) keys.Add(AccountMeta.Writable(hostFeeReceiver, false));

            return new TransactionInstruction
            {
                ProgramId = programIdKey,
                Data = SolendProgramData.EncodeBorrowObligationLiquidityData(liquidityAmount),
                Keys = keys
            };
        }

        /// <summary>
        /// Initialize a new <see cref="TransactionInstruction"/> for the <see cref="SolendProgramInstructions.Values.BorrowObligationLiquidity"/> method.
        /// </summary>
        /// <param name="liquidityAmount">The liquidity amount.</param>
        /// <param name="sourceLiquidity">The source liquidity token account.</param>
        /// <param name="destinationLiquidity">The destination liquidity token account.</param>
        /// <param name="borrowReserve">The borrow reserve.</param>
        /// <param name="borrowReserveLiquidityFeeReceiver">The borrow reserve liquidity fee receiver.</param>
        /// <param name="obligation">The obligation.</param>
        /// <param name="lendingMarket">The lending market.</param>
        /// <param name="obligationOwner">The obligation owner.</param>
        /// <param name="hostFeeReceiver">The fee receiver.</param>
        /// <returns>The <see cref="TransactionInstruction"/>.</returns>
        public TransactionInstruction BorrowObligationLiquidity(ulong liquidityAmount,
            PublicKey sourceLiquidity, PublicKey destinationLiquidity, PublicKey borrowReserve,
            PublicKey borrowReserveLiquidityFeeReceiver, PublicKey obligation, PublicKey lendingMarket,
            PublicKey obligationOwner, PublicKey hostFeeReceiver = null)
            => BorrowObligationLiquidity(ProgramIdKey, liquidityAmount, sourceLiquidity, destinationLiquidity, borrowReserve,
                borrowReserveLiquidityFeeReceiver, obligation, lendingMarket, obligationOwner, hostFeeReceiver);

        /// <summary>
        /// Initialize a new <see cref="TransactionInstruction"/> for the <see cref="SolendProgramInstructions.Values.RepayObligationLiquidity"/> method.
        /// </summary>
        /// <param name="programIdKey">The public key of the program.</param>
        /// <param name="liquidityAmount">The liquidity amount.</param>
        /// <param name="sourceLiquidity">The source liquidity token account.</param>
        /// <param name="destinationLiquidity">The destination liquidity token account.</param>
        /// <param name="repayReserve">The repay reserve.</param>
        /// <param name="obligation">The obligation.</param>
        /// <param name="lendingMarket">The lending market.</param>
        /// <param name="userTransferAuthority">The user transfer authority.</param>
        /// <returns>The <see cref="TransactionInstruction"/>.</returns>
        public static TransactionInstruction RepayObligationLiduidity(PublicKey programIdKey, ulong liquidityAmount,
            PublicKey sourceLiquidity, PublicKey destinationLiquidity, PublicKey repayReserve, PublicKey obligation,
            PublicKey lendingMarket, PublicKey userTransferAuthority)
        {
            List<AccountMeta> keys = new()
            {
                AccountMeta.Writable(sourceLiquidity, false),
                AccountMeta.Writable(destinationLiquidity, false),
                AccountMeta.Writable(repayReserve, false),
                AccountMeta.Writable(obligation, false),
                AccountMeta.ReadOnly(lendingMarket, false),
                AccountMeta.ReadOnly(userTransferAuthority, true),
                AccountMeta.ReadOnly(SysVars.ClockKey, false),
                AccountMeta.ReadOnly(TokenProgram.ProgramIdKey, false)
            };

            return new TransactionInstruction
            {
                ProgramId = programIdKey,
                Data = SolendProgramData.EncodeRepayObligationLiduidityData(liquidityAmount),
                Keys = keys
            };
        }

        /// <summary>
        /// Initialize a new <see cref="TransactionInstruction"/> for the <see cref="SolendProgramInstructions.Values.RepayObligationLiquidity"/> method.
        /// </summary>
        /// <param name="liquidityAmount">The liquidity amount.</param>
        /// <param name="sourceLiquidity">The source liquidity token account.</param>
        /// <param name="destinationLiquidity">The destination liquidity token account.</param>
        /// <param name="repayReserve">The repay reserve.</param>
        /// <param name="obligation">The obligation.</param>
        /// <param name="lendingMarket">The lending market.</param>
        /// <param name="userTransferAuthority">The user transfer authority.</param>
        /// <returns>The <see cref="TransactionInstruction"/>.</returns>
        public TransactionInstruction RepayObligationLiduidity(ulong liquidityAmount,
            PublicKey sourceLiquidity, PublicKey destinationLiquidity, PublicKey repayReserve, PublicKey obligation,
            PublicKey lendingMarket, PublicKey userTransferAuthority)
            => RepayObligationLiduidity(ProgramIdKey, liquidityAmount, sourceLiquidity, destinationLiquidity,
                repayReserve, obligation, lendingMarket, userTransferAuthority);

        /// <summary>
        /// Initialize a new <see cref="TransactionInstruction"/> for the <see cref="SolendProgramInstructions.Values.DepositReserveLiquidityAndObligationCollateral"/> method.
        /// </summary>
        /// <param name="programIdKey">The public key of the program.</param>
        /// <param name="liquidityAmount">The liquidity amount.</param>
        /// <param name="sourceLiquidity">The source liquidity token account.</param>
        /// <param name="destinationCollateral">The destination collateral token account.</param>
        /// <param name="reserve">The reserve.</param>
        /// <param name="reserveLiquiditySupply">The reserve liquidity supply.</param>
        /// <param name="reserveCollateralMint">The reserve collateral mint.</param>
        /// <param name="lendingMarket">The lending market.</param>
        /// <param name="destinationDepositCollateral">The destination deposit collateral.</param>
        /// <param name="obligation">The obligation.</param>
        /// <param name="obligationOwner">The obligation owner.</param>
        /// <param name="reserveLiquidityPyth">The reserve liquidity pyth oracle.</param>
        /// <param name="reserveLiquiditySwitchboard">The reserve liquidity switchboard oracle.</param>
        /// <param name="userTransferAuthority">The user transfer authority.</param>
        /// <returns>The <see cref="TransactionInstruction"/>.</returns>
        public static TransactionInstruction DepositReserveLiquidityAndObligationCollateral(PublicKey programIdKey, ulong liquidityAmount,
            PublicKey sourceLiquidity, PublicKey destinationCollateral, PublicKey reserve, PublicKey reserveLiquiditySupply,
            PublicKey reserveCollateralMint, PublicKey lendingMarket, PublicKey destinationDepositCollateral,
            PublicKey obligation, PublicKey obligationOwner, PublicKey reserveLiquidityPyth, PublicKey reserveLiquiditySwitchboard,
            PublicKey userTransferAuthority)
        {
            PublicKey lendingMarketAuthority = SolendProgramData.DeriveLendingMarketAuthority(lendingMarket, programIdKey);

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
                AccountMeta.ReadOnly(SysVars.ClockKey, false),
                AccountMeta.ReadOnly(TokenProgram.ProgramIdKey, false)
            };
            return new TransactionInstruction
            {
                ProgramId = programIdKey,
                Keys = keys,
                Data = SolendProgramData.EncodeDepositReserveLiquidityAndObligationCollateralData(liquidityAmount)
            };
        }

        /// <summary>
        /// Initialize a new <see cref="TransactionInstruction"/> for the <see cref="SolendProgramInstructions.Values.DepositReserveLiquidityAndObligationCollateral"/> method.
        /// </summary>
        /// <param name="liquidityAmount">The liquidity amount.</param>
        /// <param name="sourceLiquidity">The source liquidity token account.</param>
        /// <param name="destinationCollateral">The destination collateral token account.</param>
        /// <param name="reserve">The reserve.</param>
        /// <param name="reserveLiquiditySupply">The reserve liquidity supply.</param>
        /// <param name="reserveCollateralMint">The reserve collateral mint.</param>
        /// <param name="lendingMarket">The lending market.</param>
        /// <param name="destinationDepositCollateral">The destination deposit collateral.</param>
        /// <param name="obligation">The obligation.</param>
        /// <param name="obligationOwner">The obligation owner.</param>
        /// <param name="reserveLiquidityPyth">The reserve liquidity pyth oracle.</param>
        /// <param name="reserveLiquiditySwitchboard">The reserve liquidity switchboard oracle.</param>
        /// <param name="userTransferAuthority">The user transfer authority.</param>
        /// <returns>The <see cref="TransactionInstruction"/>.</returns>
        public TransactionInstruction DepositReserveLiquidityAndObligationCollateral(ulong liquidityAmount,
            PublicKey sourceLiquidity, PublicKey destinationCollateral, PublicKey reserve, PublicKey reserveLiquiditySupply,
            PublicKey reserveCollateralMint, PublicKey lendingMarket, PublicKey destinationDepositCollateral,
            PublicKey obligation, PublicKey obligationOwner, PublicKey reserveLiquidityPyth, PublicKey reserveLiquiditySwitchboard,
            PublicKey userTransferAuthority)
            => DepositReserveLiquidityAndObligationCollateral(ProgramIdKey, liquidityAmount, sourceLiquidity, destinationCollateral,
                reserve, reserveLiquiditySupply, reserveCollateralMint, lendingMarket, destinationDepositCollateral, obligation,
                obligationOwner, reserveLiquidityPyth, reserveLiquiditySwitchboard, userTransferAuthority);

        /// <summary>
        /// Initialize a new <see cref="TransactionInstruction"/> for the <see cref="SolendProgramInstructions.Values.WithdrawObligationCollateralAndRedeemReserveCollateral"/> method.
        /// </summary>
        /// <param name="programIdKey">The public key of the program.</param>
        /// <param name="collateralAmount">The collateral amount.</param>
        /// <param name="sourceCollateral">The source collateral token account.</param>
        /// <param name="destinationCollateral">The destination collateral token account.</param>
        /// <param name="withdrawReserve">The reserve.</param>
        /// <param name="obligation">The obligation.</param>
        /// <param name="lendingMarket">The lending market.</param>
        /// <param name="destinationLiquidity">The destination liquidity.</param>
        /// <param name="reserveCollateralMint">The reserve collateral mint.</param>
        /// <param name="reserveLiquiditySupply">The reserve liquidity supply.</param>
        /// <param name="obligationOwner">The obligation owner.</param>
        /// <param name="userTransferAuthority">The user transfer authority.</param>
        /// <returns>The <see cref="TransactionInstruction"/>.</returns>
        public static TransactionInstruction WithdrawObligationCollateralAndRedeemReserveCollateral(PublicKey programIdKey, ulong collateralAmount,
            PublicKey sourceCollateral, PublicKey destinationCollateral, PublicKey withdrawReserve, PublicKey obligation,
            PublicKey lendingMarket, PublicKey destinationLiquidity, PublicKey reserveCollateralMint, PublicKey reserveLiquiditySupply,
            PublicKey obligationOwner, PublicKey userTransferAuthority)
        {
            PublicKey lendingMarketAuthority = SolendProgramData.DeriveLendingMarketAuthority(lendingMarket, programIdKey);
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
                AccountMeta.ReadOnly(SysVars.ClockKey, false),
                AccountMeta.ReadOnly(TokenProgram.ProgramIdKey, false)
            };
            return new TransactionInstruction
            {
                ProgramId = programIdKey,
                Keys = keys,
                Data = SolendProgramData.EncodeWithdrawObligationCollateralAndRedeemReserveCollateralData(collateralAmount)
            };
        }

        /// <summary>
        /// Initialize a new <see cref="TransactionInstruction"/> for the <see cref="SolendProgramInstructions.Values.WithdrawObligationCollateralAndRedeemReserveCollateral"/> method.
        /// </summary>
        /// <param name="collateralAmount">The collateral amount.</param>
        /// <param name="sourceCollateral">The source collateral token account.</param>
        /// <param name="destinationCollateral">The destination collateral token account.</param>
        /// <param name="withdrawReserve">The reserve.</param>
        /// <param name="obligation">The obligation.</param>
        /// <param name="lendingMarket">The lending market.</param>
        /// <param name="destinationLiquidity">The destination liquidity.</param>
        /// <param name="reserveCollateralMint">The reserve collateral mint.</param>
        /// <param name="reserveLiquiditySupply">The reserve liquidity supply.</param>
        /// <param name="obligationOwner">The obligation owner.</param>
        /// <param name="userTransferAuthority">The user transfer authority.</param>
        /// <returns>The <see cref="TransactionInstruction"/>.</returns>
        public TransactionInstruction WithdrawObligationCollateralAndRedeemReserveCollateral(ulong collateralAmount,
            PublicKey sourceCollateral, PublicKey destinationCollateral, PublicKey withdrawReserve, PublicKey obligation,
            PublicKey lendingMarket, PublicKey destinationLiquidity, PublicKey reserveCollateralMint, PublicKey reserveLiquiditySupply,
            PublicKey obligationOwner, PublicKey userTransferAuthority)
            => WithdrawObligationCollateralAndRedeemReserveCollateral(ProgramIdKey, collateralAmount, sourceCollateral,
                destinationCollateral, withdrawReserve,obligation, lendingMarket, destinationLiquidity,
                reserveCollateralMint, reserveLiquiditySupply, obligationOwner, userTransferAuthority);

        /// <summary>
        /// Derives an obligation's address for a given market and owner.
        /// </summary>
        /// <param name="owner">The owner's public key.</param>
        /// <param name="market">The market's public key.</param>
        /// <returns>The obligation address.</returns>
        public PublicKey DeriveObligationAddress(PublicKey owner, PublicKey market)
            => DeriveObligationAddress(owner, market, ProgramIdKey);

        /// <summary>
        /// Derives an obligation's address for a given lending market, owner and program id.
        /// </summary>
        /// <param name="owner">The owner's public key.</param>
        /// <param name="market">The market's public key.</param>
        /// <param name="programId">The program id.</param>
        /// <returns>The obligation address.</returns>
        public static PublicKey DeriveObligationAddress(PublicKey owner, PublicKey market, PublicKey programId)
        {
            var success = PublicKey.TryCreateWithSeed(owner, market.Key[..32], programId, out var pubkey);

            return success ? pubkey : null;
        }

        /// <summary>
        /// Decodes an instruction created by the Solend Program.
        /// </summary>
        /// <param name="data">The instruction data to decode.</param>
        /// <param name="keys">The account keys present in the transaction.</param>
        /// <param name="keyIndices">The indices of the account keys for the instruction as they appear in the transaction.</param>
        /// <returns>A decoded instruction.</returns>
        public static DecodedInstruction Decode(ReadOnlySpan<byte> data, IList<PublicKey> keys, byte[] keyIndices)
        {
            uint instruction = data.GetU8(SolendProgramData.MethodOffset);
            SolendProgramInstructions.Values instructionValue =
                (SolendProgramInstructions.Values)Enum.Parse(typeof(SolendProgramInstructions.Values), instruction.ToString());

            DecodedInstruction decodedInstruction = new()
            {
                PublicKey = DevNetProgramIdKey,
                ProgramName = DefaultProgramName,
                InstructionName = SolendProgramInstructions.Names[instructionValue],
                Values = new Dictionary<string, object>(),
                InnerInstructions = new List<DecodedInstruction>()
            };

            switch (instructionValue)
            {
                case SolendProgramInstructions.Values.InitializeObligation:
                    SolendProgramData.DecodeInitializeObligationData(decodedInstruction, keys, keyIndices);
                    break;
                case SolendProgramInstructions.Values.RefreshReserve:
                    SolendProgramData.DecodeRefreshReserveData(decodedInstruction, keys, keyIndices);
                    break;
                case SolendProgramInstructions.Values.RefreshObligation:
                    SolendProgramData.DecodeRefreshObligationData(decodedInstruction, keys, keyIndices);
                    break;
                case SolendProgramInstructions.Values.DepositReserveLiquidity:
                    SolendProgramData.DecodeDepositReserveLiquidityData(decodedInstruction, data, keys, keyIndices);
                    break;
                case SolendProgramInstructions.Values.RedeemReserveCollateral:
                    SolendProgramData.DecodeRedeemReserveCollateralData(decodedInstruction, data, keys, keyIndices);
                    break;
                case SolendProgramInstructions.Values.DepositObligationCollateral:
                    SolendProgramData.DecodeDepositObligationCollateralData(decodedInstruction, data, keys, keyIndices);
                    break;
                case SolendProgramInstructions.Values.WithdrawObligationCollateral:
                    SolendProgramData.DecodeWithdrawObligationCollateralData(decodedInstruction, data, keys, keyIndices);
                    break;
                case SolendProgramInstructions.Values.BorrowObligationLiquidity:
                    SolendProgramData.DecodeBorrowObligationLiquidityData(decodedInstruction, data, keys, keyIndices);
                    break;
                case SolendProgramInstructions.Values.RepayObligationLiquidity:
                    SolendProgramData.DecodeRepayObligationLiduidityData(decodedInstruction, data, keys, keyIndices);
                    break;
                case SolendProgramInstructions.Values.DepositReserveLiquidityAndObligationCollateral:
                    SolendProgramData.DecodeDepositReserveLiquidityAndObligationCollateralData(decodedInstruction, data, keys, keyIndices);
                    break;
                case SolendProgramInstructions.Values.WithdrawObligationCollateralAndRedeemReserveCollateral:
                    SolendProgramData.DecodeWithdrawObligationCollateralAndRedeemReserveCollateralData(decodedInstruction, data, keys, keyIndices);
                    break;
            }
            return decodedInstruction;
        }
    }
}
