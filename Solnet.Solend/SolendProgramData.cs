using Solnet.Programs;
using Solnet.Programs.Utilities;
using Solnet.Rpc.Utilities;
using Solnet.Solend.Models;
using Solnet.Wallet;
using System;
using System.Collections.Generic;

namespace Solnet.Solend
{
    /// <summary>
    /// Implements the <see cref="SolendProgram"/> data encodings.
    /// </summary>
    internal static class SolendProgramData
    {
        /// <summary>
        /// The offset at which to encode the value that specifies the instruction.
        /// </summary>
        internal const int MethodOffset = 0;

        /// <summary>
        /// Decodes the instruction instruction data  for the <see cref="SolendProgramInstructions.Values.InitializeLendingMarket"/> method.
        /// </summary>
        /// <param name="decodedInstruction">The decoded instruction to add data to.</param>
        /// <param name="data">The instruction data to decode.</param>
        /// <param name="keys">The account keys present in the transaction.</param>
        /// <param name="keyIndices">The indices of the account keys for the instruction as they appear in the transaction.</param>
        internal static void DecodeInitializeLendingMarketData(DecodedInstruction decodedInstruction, ReadOnlySpan<byte> data,
            IList<PublicKey> keys, byte[] keyIndices)
        {

        }

        /// <summary>
        /// Decodes the instruction instruction data  for the <see cref="SolendProgramInstructions.Values.SetLendingMarketOwner"/> method.
        /// </summary>
        /// <param name="decodedInstruction">The decoded instruction to add data to.</param>
        /// <param name="data">The instruction data to decode.</param>
        /// <param name="keys">The account keys present in the transaction.</param>
        /// <param name="keyIndices">The indices of the account keys for the instruction as they appear in the transaction.</param>
        internal static void DecodeSetLendingMarketOwnerData(DecodedInstruction decodedInstruction, ReadOnlySpan<byte> data,
            IList<PublicKey> keys, byte[] keyIndices)
        {

        }

        /// <summary>
        /// Decodes the instruction instruction data  for the <see cref="SolendProgramInstructions.Values.InitializeReserve"/> method.
        /// </summary>
        /// <param name="decodedInstruction">The decoded instruction to add data to.</param>
        /// <param name="data">The instruction data to decode.</param>
        /// <param name="keys">The account keys present in the transaction.</param>
        /// <param name="keyIndices">The indices of the account keys for the instruction as they appear in the transaction.</param>
        internal static void DecodeInitializeReserveData(DecodedInstruction decodedInstruction, ReadOnlySpan<byte> data,
            IList<PublicKey> keys, byte[] keyIndices)
        {

        }

        /// <summary>
        /// Encode the transaction instruction data for the <see cref="SolendProgramInstructions.Values.RefreshReserve"/> method.
        /// </summary>
        /// <returns>The byte array with the encoded data.</returns>
        internal static byte[] EncodeRefreshReserveData()
        {
            byte[] buffer = new byte[1];
            buffer.WriteU8((byte)SolendProgramInstructions.Values.RefreshReserve, MethodOffset);
            return buffer;
        }

        /// <summary>
        /// Decodes the instruction instruction data  for the <see cref="SolendProgramInstructions.Values.RefreshReserve"/> method.
        /// </summary>
        /// <param name="decodedInstruction">The decoded instruction to add data to.</param>
        /// <param name="keys">The account keys present in the transaction.</param>
        /// <param name="keyIndices">The indices of the account keys for the instruction as they appear in the transaction.</param>
        public static void DecodeRefreshReserveData(DecodedInstruction decodedInstruction, IList<PublicKey> keys,
            byte[] keyIndices)
        {
            decodedInstruction.Values.Add("Reserve", keys[keyIndices[0]]);
            decodedInstruction.Values.Add("Reserve Liquidity Oracle", keys[keyIndices[1]]);
            decodedInstruction.Values.Add("Sysvar Clock", keys[keyIndices[1]]);

        }

        /// <summary>
        /// Encode the transaction instruction data for the <see cref="SolendProgramInstructions.Values.DepositReserveLiquidity"/> method.
        /// </summary>
        /// <param name="liquidityAmount">The amount of liquidity.</param>
        /// <returns>The byte array with the encoded data.</returns>
        internal static byte[] EncodeDepositReserveLiquidityData(ulong liquidityAmount)
        {
            byte[] buffer = new byte[9];
            buffer.WriteU8((byte)SolendProgramInstructions.Values.DepositReserveLiquidity, MethodOffset);
            buffer.WriteU64(liquidityAmount, 1);
            return buffer;
        }

        /// <summary>
        /// Decodes the instruction instruction data  for the <see cref="SolendProgramInstructions.Values.DepositReserveLiquidity"/> method.
        /// </summary>
        /// <param name="decodedInstruction">The decoded instruction to add data to.</param>
        /// <param name="data">The instruction data to decode.</param>
        /// <param name="keys">The account keys present in the transaction.</param>
        /// <param name="keyIndices">The indices of the account keys for the instruction as they appear in the transaction.</param>
        internal static void DecodeDepositReserveLiquidityData(DecodedInstruction decodedInstruction, ReadOnlySpan<byte> data,
            IList<PublicKey> keys, byte[] keyIndices)
        {

        }

        /// <summary>
        /// Encode the transaction instruction data for the <see cref="SolendProgramInstructions.Values.RedeemReserveCollateral"/> method.
        /// </summary>
        /// <param name="collateralAmount">The amount of collateral.</param>
        /// <returns>The byte array with the encoded data.</returns>
        internal static byte[] EncodeRedeemReserveCollateralData(ulong collateralAmount)
        {
            byte[] buffer = new byte[9];
            buffer.WriteU8((byte)SolendProgramInstructions.Values.RedeemReserveCollateral, MethodOffset);
            buffer.WriteU64(collateralAmount, 1);
            return buffer;
        }

        /// <summary>
        /// Decodes the instruction instruction data  for the <see cref="SolendProgramInstructions.Values.RedeemReserveCollateral"/> method.
        /// </summary>
        /// <param name="decodedInstruction">The decoded instruction to add data to.</param>
        /// <param name="data">The instruction data to decode.</param>
        /// <param name="keys">The account keys present in the transaction.</param>
        /// <param name="keyIndices">The indices of the account keys for the instruction as they appear in the transaction.</param>
        internal static void DecodeRedeemReserveCollateralData(DecodedInstruction decodedInstruction, ReadOnlySpan<byte> data,
            IList<PublicKey> keys, byte[] keyIndices)
        {

        }

        /// <summary>
        /// Encode the transaction instruction data for the <see cref="SolendProgramInstructions.Values.InitializeObligation"/> method.
        /// </summary>
        /// <returns>The byte array with the encoded data.</returns>
        internal static byte[] EncodeInitializeObligationData()
        {
            byte[] buffer = new byte[1];
            buffer.WriteU8((byte)SolendProgramInstructions.Values.InitializeObligation, MethodOffset);
            return buffer;
        }

        /// <summary>
        /// Decodes the instruction instruction data  for the <see cref="SolendProgramInstructions.Values.InitializeObligation"/> method.
        /// </summary>
        /// <param name="decodedInstruction">The decoded instruction to add data to.</param>
        /// <param name="keys">The account keys present in the transaction.</param>
        /// <param name="keyIndices">The indices of the account keys for the instruction as they appear in the transaction.</param>
        public static void DecodeInitializeObligationData(DecodedInstruction decodedInstruction, IList<PublicKey> keys,
            byte[] keyIndices)
        {
            decodedInstruction.Values.Add("Obligation", keys[keyIndices[0]]);
            decodedInstruction.Values.Add("Lending Market", keys[keyIndices[1]]);
            decodedInstruction.Values.Add("Obligation Owner", keys[keyIndices[2]]);
            decodedInstruction.Values.Add("Sysvar Clock", keys[keyIndices[3]]);
            decodedInstruction.Values.Add("Sysvar Rent", keys[keyIndices[4]]);
            decodedInstruction.Values.Add("Token Program", keys[keyIndices[5]]);
        }

        /// <summary>
        /// Encode the transaction instruction data for the <see cref="SolendProgramInstructions.Values.RefreshObligation"/> method.
        /// </summary>
        /// <returns>The byte array with the encoded data.</returns>
        internal static byte[] EncodeRefreshObligationData()
        {
            byte[] buffer = new byte[1];
            buffer.WriteU8((byte)SolendProgramInstructions.Values.RefreshObligation, MethodOffset);
            return buffer;
        }

        /// <summary>
        /// Decodes the instruction instruction data  for the <see cref="SolendProgramInstructions.Values.RefreshObligation"/> method.
        /// </summary>
        /// <param name="decodedInstruction">The decoded instruction to add data to.</param>
        /// <param name="keys">The account keys present in the transaction.</param>
        /// <param name="keyIndices">The indices of the account keys for the instruction as they appear in the transaction.</param>
        internal static void DecodeRefreshObligationData(DecodedInstruction decodedInstruction, IList<PublicKey> keys,
            byte[] keyIndices)
        {

        }

        /// <summary>
        /// Encode the transaction instruction data for the <see cref="SolendProgramInstructions.Values.DepositObligationCollateral"/> method.
        /// </summary>
        /// <param name="collateralAmount">The amount of collateral.</param>
        /// <returns>The byte array with the encoded data.</returns>
        internal static byte[] EncodeDepositObligationCollateralData(ulong collateralAmount)
        {
            byte[] buffer = new byte[9];
            buffer.WriteU8((byte)SolendProgramInstructions.Values.DepositObligationCollateral, MethodOffset);
            buffer.WriteU64(collateralAmount, 1);
            return buffer;
        }

        /// <summary>
        /// Decodes the instruction instruction data  for the <see cref="SolendProgramInstructions.Values.DepositObligationCollateral"/> method.
        /// </summary>
        /// <param name="decodedInstruction">The decoded instruction to add data to.</param>
        /// <param name="data">The instruction data to decode.</param>
        /// <param name="keys">The account keys present in the transaction.</param>
        /// <param name="keyIndices">The indices of the account keys for the instruction as they appear in the transaction.</param>
        internal static void DecodeDepositObligationCollateralData(DecodedInstruction decodedInstruction, ReadOnlySpan<byte> data,
            IList<PublicKey> keys, byte[] keyIndices)
        {

        }

        /// <summary>
        /// Encode the transaction instruction data for the <see cref="SolendProgramInstructions.Values.WithdrawObligationCollateral"/> method.
        /// </summary>
        /// <param name="collateralAmount">The amount of collateral.</param>
        /// <returns>The byte array with the encoded data.</returns>
        internal static byte[] EncodeWithdrawObligationCollateralData(ulong collateralAmount)
        {
            byte[] buffer = new byte[9];
            buffer.WriteU8((byte)SolendProgramInstructions.Values.WithdrawObligationCollateral, MethodOffset);
            buffer.WriteU64(collateralAmount, 1);
            return buffer;
        }

        /// <summary>
        /// Decodes the instruction instruction data  for the <see cref="SolendProgramInstructions.Values.WithdrawObligationCollateral"/> method.
        /// </summary>
        /// <param name="decodedInstruction">The decoded instruction to add data to.</param>
        /// <param name="data">The instruction data to decode.</param>
        /// <param name="keys">The account keys present in the transaction.</param>
        /// <param name="keyIndices">The indices of the account keys for the instruction as they appear in the transaction.</param>
        internal static void DecodeWithdrawObligationCollateralData(DecodedInstruction decodedInstruction, ReadOnlySpan<byte> data,
            IList<PublicKey> keys, byte[] keyIndices)
        {

        }

        /// <summary>
        /// Encode the transaction instruction data for the <see cref="SolendProgramInstructions.Values.BorrowObligationLiquidity"/> method.
        /// </summary>
        /// <param name="liquidityAmount">The amount of liquidity.</param>
        /// <returns>The byte array with the encoded data.</returns>
        internal static byte[] EncodeBorrowObligationLiduidityData(ulong liquidityAmount)
        {
            byte[] buffer = new byte[9];
            buffer.WriteU8((byte)SolendProgramInstructions.Values.BorrowObligationLiquidity, MethodOffset);
            buffer.WriteU64(liquidityAmount, 1);
            return buffer;
        }

        /// <summary>
        /// Decodes the instruction instruction data  for the <see cref="SolendProgramInstructions.Values.BorrowObligationLiquidity"/> method.
        /// </summary>
        /// <param name="decodedInstruction">The decoded instruction to add data to.</param>
        /// <param name="data">The instruction data to decode.</param>
        /// <param name="keys">The account keys present in the transaction.</param>
        /// <param name="keyIndices">The indices of the account keys for the instruction as they appear in the transaction.</param>
        internal static void DecodeBorrowObligationLiduidityData(DecodedInstruction decodedInstruction, ReadOnlySpan<byte> data,
            IList<PublicKey> keys, byte[] keyIndices)
        {

        }

        /// <summary>
        /// Encode the transaction instruction data for the <see cref="SolendProgramInstructions.Values.RepayObligationLiquidity"/> method.
        /// </summary>
        /// <param name="liquidityAmount">The amount of liquidity.</param>
        /// <returns>The byte array with the encoded data.</returns>
        internal static byte[] EncodeRepayObligationLiduidityData(ulong liquidityAmount)
        {
            byte[] buffer = new byte[9];
            buffer.WriteU8((byte)SolendProgramInstructions.Values.RepayObligationLiquidity, MethodOffset);
            buffer.WriteU64(liquidityAmount, 1);
            return buffer;
        }

        /// <summary>
        /// Decodes the instruction instruction data  for the <see cref="SolendProgramInstructions.Values.RepayObligationLiquidity"/> method.
        /// </summary>
        /// <param name="decodedInstruction">The decoded instruction to add data to.</param>
        /// <param name="data">The instruction data to decode.</param>
        /// <param name="keys">The account keys present in the transaction.</param>
        /// <param name="keyIndices">The indices of the account keys for the instruction as they appear in the transaction.</param>
        internal static void DecodeRepayObligationLiduidityData(DecodedInstruction decodedInstruction, ReadOnlySpan<byte> data,
            IList<PublicKey> keys, byte[] keyIndices)
        {

        }

        /// <summary>
        /// Encode the transaction instruction data for the <see cref="SolendProgramInstructions.Values.LiquidateObligation"/> method.
        /// </summary>
        /// <param name="liquidityAmount">The amount of liquidity.</param>
        /// <returns>The byte array with the encoded data.</returns>
        internal static byte[] EncodeLiquidateObligationData(ulong liquidityAmount)
        {
            byte[] buffer = new byte[9];
            buffer.WriteU8((byte)SolendProgramInstructions.Values.LiquidateObligation, MethodOffset);
            buffer.WriteU64(liquidityAmount, 1);
            return buffer;
        }

        /// <summary>
        /// Decodes the instruction instruction data  for the <see cref="SolendProgramInstructions.Values.LiquidateObligation"/> method.
        /// </summary>
        /// <param name="decodedInstruction">The decoded instruction to add data to.</param>
        /// <param name="data">The instruction data to decode.</param>
        /// <param name="keys">The account keys present in the transaction.</param>
        /// <param name="keyIndices">The indices of the account keys for the instruction as they appear in the transaction.</param>
        internal static void DecodeLiquidateObligationData(DecodedInstruction decodedInstruction, ReadOnlySpan<byte> data,
            IList<PublicKey> keys, byte[] keyIndices)
        {

        }

        /// <summary>
        /// Encode the transaction instruction data for the <see cref="SolendProgramInstructions.Values.FlashLoan"/> method.
        /// </summary>
        /// <param name="liquidityAmount">The amount of liquidity.</param>
        /// <returns>The byte array with the encoded data.</returns>
        internal static byte[] EncodeFlashLoanData(ulong liquidityAmount)
        {
            byte[] buffer = new byte[9];
            buffer.WriteU8((byte)SolendProgramInstructions.Values.FlashLoan, MethodOffset);
            buffer.WriteU64(liquidityAmount, 1);
            return buffer;
        }

        /// <summary>
        /// Decodes the instruction instruction data  for the <see cref="SolendProgramInstructions.Values.FlashLoan"/> method.
        /// </summary>
        /// <param name="decodedInstruction">The decoded instruction to add data to.</param>
        /// <param name="data">The instruction data to decode.</param>
        /// <param name="keys">The account keys present in the transaction.</param>
        /// <param name="keyIndices">The indices of the account keys for the instruction as they appear in the transaction.</param>
        internal static void DecodeFlashLoanData(DecodedInstruction decodedInstruction, ReadOnlySpan<byte> data,
            IList<PublicKey> keys, byte[] keyIndices)
        {

        }

        /// <summary>
        /// Encode the transaction instruction data for the <see cref="SolendProgramInstructions.Values.DepositReserveLiquidityAndObligationCollateral"/> method.
        /// </summary>
        /// <param name="liquidityAmount">The amount of liquidity.</param>
        /// <returns>The byte array with the encoded data.</returns>
        internal static byte[] EncodeDepositReserveLiquidityAndObligationCollateralData(ulong liquidityAmount)
        {
            byte[] buffer = new byte[9];
            buffer.WriteU8((byte)SolendProgramInstructions.Values.DepositReserveLiquidityAndObligationCollateral, MethodOffset);
            buffer.WriteU64(liquidityAmount, 1);
            return buffer;
        }

        /// <summary>
        /// Decodes the instruction instruction data  for the <see cref="SolendProgramInstructions.Values.DepositReserveLiquidityAndObligationCollateral"/> method.
        /// </summary>
        /// <param name="decodedInstruction">The decoded instruction to add data to.</param>
        /// <param name="data">The instruction data to decode.</param>
        /// <param name="keys">The account keys present in the transaction.</param>
        /// <param name="keyIndices">The indices of the account keys for the instruction as they appear in the transaction.</param>
        internal static void DecodeDepositReserveLiquidityAndObligationCollateralData(DecodedInstruction decodedInstruction, ReadOnlySpan<byte> data,
            IList<PublicKey> keys, byte[] keyIndices)
        {
            decodedInstruction.Values.Add("Source Liquidity", keys[keyIndices[0]]);
            decodedInstruction.Values.Add("Destination Collateral", keys[keyIndices[1]]);
            decodedInstruction.Values.Add("Reserve", keys[keyIndices[2]]);
            decodedInstruction.Values.Add("Reserve Liquidity Supply", keys[keyIndices[3]]);
            decodedInstruction.Values.Add("Reserve Collateral Mint", keys[keyIndices[4]]);
            decodedInstruction.Values.Add("Lending Market", keys[keyIndices[5]]);
            decodedInstruction.Values.Add("Lending Market Authority", keys[keyIndices[6]]);
            decodedInstruction.Values.Add("Destination Deposit Collateral", keys[keyIndices[7]]);
            decodedInstruction.Values.Add("Obligation", keys[keyIndices[8]]);
            decodedInstruction.Values.Add("Obligation Owner", keys[keyIndices[9]]);
            decodedInstruction.Values.Add("Reserve Liquidity Pyth", keys[keyIndices[10]]);
            decodedInstruction.Values.Add("Reserve Liquidity Switchboard", keys[keyIndices[11]]);
            decodedInstruction.Values.Add("User Transfer Authority", keys[keyIndices[12]]);
            decodedInstruction.Values.Add("Sysvar Clock", keys[keyIndices[13]]);
            decodedInstruction.Values.Add("Token Program", keys[keyIndices[14]]);
            decodedInstruction.Values.Add("Liquidity Amount", data.GetU64(1));
        }

        /// <summary>
        /// Encode the transaction instruction data for the <see cref="SolendProgramInstructions.Values.WithdrawObligationCollateralAndRedeemReserveCollateral"/> method.
        /// </summary>
        /// <param name="liquidityAmount">The amount of liquidity.</param>
        /// <returns>The byte array with the encoded data.</returns>
        internal static byte[] EncodeWithdrawObligationCollateralAndRedeemReserveCollateralData(ulong liquidityAmount)
        {
            byte[] buffer = new byte[9];
            buffer.WriteU8((byte)SolendProgramInstructions.Values.WithdrawObligationCollateralAndRedeemReserveCollateral, MethodOffset);
            buffer.WriteU64(liquidityAmount, 1);
            return buffer;
        }

        /// <summary>
        /// Decodes the instruction instruction data  for the <see cref="SolendProgramInstructions.Values.WithdrawObligationCollateralAndRedeemReserveCollateral"/> method.
        /// </summary>
        /// <param name="decodedInstruction">The decoded instruction to add data to.</param>
        /// <param name="data">The instruction data to decode.</param>
        /// <param name="keys">The account keys present in the transaction.</param>
        /// <param name="keyIndices">The indices of the account keys for the instruction as they appear in the transaction.</param>
        internal static void DecodeWithdrawObligationCollateralAndRedeemReserveCollateralData(DecodedInstruction decodedInstruction, ReadOnlySpan<byte> data,
            IList<PublicKey> keys, byte[] keyIndices)
        {
            decodedInstruction.Values.Add("Source Collateral", keys[keyIndices[0]]);
            decodedInstruction.Values.Add("Destination Collateral", keys[keyIndices[1]]);
            decodedInstruction.Values.Add("Withdraw Reserve", keys[keyIndices[2]]);
            decodedInstruction.Values.Add("Obligation", keys[keyIndices[3]]);
            decodedInstruction.Values.Add("Lending Market", keys[keyIndices[4]]);
            decodedInstruction.Values.Add("Lending Market Authority", keys[keyIndices[5]]);
            decodedInstruction.Values.Add("Destination Liquidity", keys[keyIndices[6]]);
            decodedInstruction.Values.Add("Reserve Collateral Mint", keys[keyIndices[7]]);
            decodedInstruction.Values.Add("Reserve Liquidity Supply", keys[keyIndices[8]]);
            decodedInstruction.Values.Add("Obligation Owner", keys[keyIndices[9]]);
            decodedInstruction.Values.Add("User Transfer Authority", keys[keyIndices[10]]);
            decodedInstruction.Values.Add("Sysvar Clock", keys[keyIndices[11]]);
            decodedInstruction.Values.Add("Token Program", keys[keyIndices[12]]);
            decodedInstruction.Values.Add("Liquidity Amount", data.GetU64(1));
        }

        /// <summary>
        /// Derive the vault signer address for the given market.
        /// </summary>
        /// <param name="lendingMarket">The lending market public key.</param>
        /// <param name="programId">The program id.</param>
        /// <returns>The vault signer address.</returns>
        /// <exception cref="Exception">Throws exception when unable to derive the vault signer address.</exception>
        public static PublicKey DeriveLendingMarketAuthority(PublicKey lendingMarket, PublicKey programId)
        {
            List<byte[]> seeds = new() { lendingMarket.KeyBytes };

            bool success = AddressExtensions.TryFindProgramAddress(seeds, programId.KeyBytes,
                out byte[] lendingMarketAuthority, out _);

            return !success ? null : new(lendingMarketAuthority);
        }
    }
}
