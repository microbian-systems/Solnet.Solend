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
        public static readonly PublicKey MainNetProgramIdKey =
            new PublicKey("So1endDq2YkqhipRh3WViPa8hdiSpxWy6z3Z6tMCpAo");

        /// <summary>
        /// Solend Program DevNet Program ID.
        /// </summary>
        public static readonly PublicKey DevNetProgramIdKey =
            new PublicKey("ALend7Ketfx5bxh6ghsCDXAoDrhvEmsXT3cynB6aPLgx");

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
        public static SolendProgram CreateDevNet() => new SolendProgram(DevNetProgramIdKey);

        /// <summary>
        /// Initialize the <see cref="SolendProgram"/> for <see cref="Cluster.MainNet"/>.
        /// </summary>
        /// <returns>The <see cref="SolendProgram"/> instance.</returns>
        public static SolendProgram CreateMainNet() => new SolendProgram(MainNetProgramIdKey);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="programIdKey">The public key of the program.</param>
        /// <param name="reserve"></param>
        /// <param name="reserveLiquidityOracle"></param>
        /// <returns>The transaction instruction.</returns>
        public static TransactionInstruction RefreshReserve(PublicKey programIdKey, PublicKey reserve, PublicKey reserveLiquidityOracle)
        {

            List<AccountMeta> keys = new()
            {
                AccountMeta.Writable(reserve, false),
                AccountMeta.ReadOnly(reserveLiquidityOracle, false),
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
        /// 
        /// </summary>
        /// <param name="programIdKey">The public key of the program.</param>
        /// <param name="liquidityAmount"></param>
        /// <param name="sourceLiquidity"></param>
        /// <param name="destinationCollateral"></param>
        /// <param name="reserve"></param>
        /// <param name="reserveLiquiditySupply"></param>
        /// <param name="reserveCollateralMint"></param>
        /// <param name="lendingMarket"></param>
        /// <param name="userTransferAuthority"></param>
        /// <returns>The transaction instruction.</returns>
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
        /// 
        /// </summary>
        /// <param name="programIdKey">The public key of the program.</param>
        /// <param name="collateralAmount"></param>
        /// <param name="sourceCollateral"></param>
        /// <param name="destinationLiquidity"></param>
        /// <param name="reserve"></param>
        /// <param name="reserveCollateralMint"></param>
        /// <param name="reserveCollateralSupply"></param>
        /// <param name="lendingMarket"></param>
        /// <param name="userTransferAuthority"></param>
        /// <returns>The transaction instruction.</returns>
        public static TransactionInstruction RedeemReserveCollateral(PublicKey programIdKey, ulong collateralAmount,
            PublicKey sourceCollateral, PublicKey destinationLiquidity, PublicKey reserve, PublicKey reserveCollateralMint,
            PublicKey reserveCollateralSupply, PublicKey lendingMarket, PublicKey userTransferAuthority)
        {
            PublicKey lendingMarketAuthority = SolendProgramData.DeriveLendingMarketAuthority(lendingMarket, programIdKey);

            List<AccountMeta> keys = new()
            {
                AccountMeta.Writable(sourceCollateral, false),
                AccountMeta.Writable(destinationLiquidity, false),
                AccountMeta.Writable(reserve, false),
                AccountMeta.Writable(reserveCollateralMint, false),
                AccountMeta.Writable(reserveCollateralSupply, false),
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
        /// 
        /// </summary>
        /// <param name="programIdKey">The public key of the program.</param>
        /// <param name="obligation"></param>
        /// <param name="lendingMarket"></param>
        /// <param name="obligationOwner"></param>
        /// <returns>The transaction instruction.</returns>
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
        /// 
        /// </summary>
        /// <param name="programIdKey">The public key of the program.</param>
        /// <param name="obligation"></param>
        /// <param name="reserves"></param>
        /// <returns>The transaction instruction.</returns>
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
        /// 
        /// </summary>
        /// <param name="programIdKey">The public key of the program.</param>
        /// <param name="collateralAmount"></param>
        /// <param name="sourceCollateral"></param>
        /// <param name="destinationCollateral"></param>
        /// <param name="depositReserve"></param>
        /// <param name="obligation"></param>
        /// <param name="lendingMarket"></param>
        /// <param name="obligationOwner"></param>
        /// <param name="userTransferAuthority"></param>
        /// <returns>The transaction instruction.</returns>
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
        /// 
        /// </summary>
        /// <param name="programIdKey">The public key of the program.</param>
        /// <param name="collateralAmount"></param>
        /// <param name="sourceCollateral"></param>
        /// <param name="destinationCollateral"></param>
        /// <param name="withdrawReserve"></param>
        /// <param name="obligation"></param>
        /// <param name="lendingMarket"></param>
        /// <param name="obligationOwner"></param>
        /// <returns>The transaction instruction.</returns>
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
        /// 
        /// </summary>
        /// <param name="programIdKey">The public key of the program.</param>
        /// <param name="liquidityAmount"></param>
        /// <param name="sourceLiquidity"></param>
        /// <param name="destinationLiquidity"></param>
        /// <param name="borrowReserve"></param>
        /// <param name="borrowReserveLiquidityFeeReceiver"></param>
        /// <param name="obligation"></param>
        /// <param name="lendingMarket"></param>
        /// <param name="obligationOwner"></param>
        /// <param name="hostFeeReceiver"></param>
        /// <returns>The transaction instruction.</returns>
        public static TransactionInstruction BorrowObligationLiduidity(PublicKey programIdKey, ulong liquidityAmount,
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
                Data = SolendProgramData.EncodeBorrowObligationLiduidityData(liquidityAmount),
                Keys = keys
            };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="programIdKey">The public key of the program.</param>
        /// <param name="liquidityAmount"></param>
        /// <param name="sourceLiquidity"></param>
        /// <param name="destinationLiquidity"></param>
        /// <param name="repayReserve"></param>
        /// <param name="obligation"></param>
        /// <param name="lendingMarket"></param>
        /// <param name="userTransferAuthority"></param>
        /// <returns>The transaction instruction.</returns>
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
        /// 
        /// </summary>
        /// <param name="programIdKey">The public key of the program.</param>
        /// <param name="liquidityAmount"></param>
        /// <param name="sourceLiquidity"></param>
        /// <param name="destinationCollateral"></param>
        /// <param name="repayReserve"></param>
        /// <param name="repayReserveLiquiditySupply"></param>
        /// <param name="withdrawReserve"></param>
        /// <param name="withdrawReserveCollateralSupply"></param>
        /// <param name="obligation"></param>
        /// <param name="lendingMarket"></param>
        /// <param name="userTransferAuthority"></param>
        /// <returns>The transaction instruction.</returns>
        public static TransactionInstruction LiquidateObligation(PublicKey programIdKey, ulong liquidityAmount,
            PublicKey sourceLiquidity, PublicKey destinationCollateral, PublicKey repayReserve,
            PublicKey repayReserveLiquiditySupply, PublicKey withdrawReserve, PublicKey withdrawReserveCollateralSupply,
            PublicKey obligation, PublicKey lendingMarket, PublicKey userTransferAuthority)
        {
            PublicKey lendingMarketAuthority = SolendProgramData.DeriveLendingMarketAuthority(lendingMarket, programIdKey);

            List<AccountMeta> keys = new()
            {
                AccountMeta.Writable(sourceLiquidity, false),
                AccountMeta.Writable(destinationCollateral, false),
                AccountMeta.Writable(repayReserve, false),
                AccountMeta.Writable(repayReserveLiquiditySupply, false),
                AccountMeta.ReadOnly(withdrawReserve, false),
                AccountMeta.Writable(withdrawReserveCollateralSupply, false),
                AccountMeta.Writable(obligation, false),
                AccountMeta.ReadOnly(lendingMarket, false),
                AccountMeta.ReadOnly(lendingMarketAuthority, false),
                AccountMeta.ReadOnly(userTransferAuthority, true),
                AccountMeta.ReadOnly(SysVars.ClockKey, false),
                AccountMeta.ReadOnly(TokenProgram.ProgramIdKey, false)
            };


            return new TransactionInstruction
            {
                ProgramId = programIdKey,
                Data = SolendProgramData.EncodeLiquidateObligationData(liquidityAmount),
                Keys = keys
            };
        }

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
            PublicKey lendingMarketAuthority = SolendProgramData.DeriveLendingMarketAuthority(lendingMarket, ProgramIdKey);
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
            PublicKey lendingMarketAuthority = SolendProgramData.DeriveLendingMarketAuthority(lendingMarket, ProgramIdKey);
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
                ProgramId = ProgramIdKey.KeyBytes,
                Keys = keys,
                Data = SolendProgramData.EncodeDepositReserveLiquidityAndObligationCollateralData(collateralAmount)
            };
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
                case SolendProgramInstructions.Values.RefreshReserve:
                    SolendProgramData.DecodeRefreshReserveData(decodedInstruction, keys, keyIndices);
                    break;
                case SolendProgramInstructions.Values.InitializeObligation:
                    SolendProgramData.DecodeInitializeObligationData(decodedInstruction, keys, keyIndices);
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
