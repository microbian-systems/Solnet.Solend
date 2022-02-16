using Solnet.Programs.Utilities;
using Solnet.Wallet;
using System;

namespace Solnet.Solend.Models
{
    /// <summary>
    /// Represents a reserve's config in Solend.
    /// </summary>
    public class ReserveConfig
    {
        /// <summary>
        /// The layout of the <see cref="ReserveConfig"/> structure.
        /// </summary>
        public static class Layout
        {
            /// <summary>
            /// The length of the <see cref="ReserveConfig"/> structure.
            /// </summary>
            public const int Length = 72;

            /// <summary>
            /// The offset at which the optimal utilization rate value begins.
            /// </summary>
            public const int OptimalUtilizationRateOffset = 0;

            /// <summary>
            /// The offset at which the loan to value ratio value begins.
            /// </summary>
            public const int LoanToValueRatioOffset = 1;

            /// <summary>
            /// The offset at which the liquidation bonus value begins.
            /// </summary>
            public const int LiquidationBonusOffset = 2;

            /// <summary>
            /// The offset at which the liquidation threshold value begins-
            /// </summary>
            public const int LiquidationThresholdOffset = 3;

            /// <summary>
            /// The offset at which the minimum borrow rate value begins.
            /// </summary>
            public const int MinBorrowRateOffset = 4;

            /// <summary>
            /// The offset at which the optimal borrow rate value begins.
            /// </summary>
            public const int OptimalBorrowRateOffset = 5;

            /// <summary>
            /// The offset at which the maximum borrow rate value begins.
            /// </summary>
            public const int MaxBorrowRateOffset = 6;

            /// <summary>
            /// The offset at which the fees value begins.
            /// </summary>
            public const int FeesOffset = 7;

            /// <summary>
            /// The offset at which the deposit limit value begins.
            /// </summary>
            public const int DepositLimitOffset = 24;

            /// <summary>
            /// The offset at which the borrow limit value begins.
            /// </summary>
            public const int BorrowLimitOffset = 32;

            /// <summary>
            /// The offset at which the fee receiver public key begins.
            /// </summary>
            public const int FeeReceiverOffset = 40;
        }

        /// <summary>
        /// Optimal utilization rate, as a percentage
        /// </summary>
        public byte OptimalUtilizationRate;

        /// <summary>
        /// Target ratio of the value of borrows to deposits, as a percentage
        /// 0 if use as collateral is disabled
        /// </summary>
        public byte LoanToValueRatio;

        /// <summary>
        /// Bonus a liquidator gets when repaying part of an unhealthy obligation, as a percentage
        /// </summary>
        public byte LiquidationBonus;

        /// <summary>
        /// Loan to value ratio at which an obligation can be liquidated, as a percentage
        /// </summary>
        public byte LiquidationThreshold;

        /// <summary>
        /// Min borrow APY
        /// </summary>
        public byte MinBorrowRate;

        /// <summary>
        /// Optimal (utilization) borrow APY
        /// </summary>
        public byte OptimalBorrowRate;

        /// <summary>
        /// Max borrow APY
        /// </summary>
        public byte MaxBorrowRate;

        /// <summary>
        /// Program owner fees assessed, separate from gains due to interest accrual
        /// </summary>
        public ReserveFees Fees;

        /// <summary>
        /// Maximum deposit limit of liquidity in native units, u64::MAX for inf
        /// </summary>
        public ulong DepositLimit;

        /// <summary>
        /// Borrows disabled
        /// </summary>
        public ulong BorrowLimit;

        /// <summary>
        /// Reserve liquidity fee receiver address
        /// </summary>
        public PublicKey FeeReceiver;

        /// <summary>
        /// Initialize the <see cref="ReserveConfig"/> with the given data.
        /// </summary>
        /// <param name="data">The byte array.</param>
        public ReserveConfig(ReadOnlySpan<byte> data)
        {
            if (data.Length != Layout.Length)
                throw new ArgumentException($"{nameof(data)} has wrong size. Expected {Layout.Length} bytes, actual {data.Length} bytes.");

            OptimalUtilizationRate = data.GetU8(Layout.OptimalUtilizationRateOffset);
            LoanToValueRatio = data.GetU8(Layout.LoanToValueRatioOffset);
            LiquidationBonus = data.GetU8(Layout.LiquidationBonusOffset);
            LiquidationThreshold = data.GetU8(Layout.LiquidationThresholdOffset);
            MinBorrowRate = data.GetU8(Layout.MinBorrowRateOffset);
            OptimalBorrowRate = data.GetU8(Layout.OptimalBorrowRateOffset);
            MaxBorrowRate = data.GetU8(Layout.MaxBorrowRateOffset);
            Fees = new(data.Slice(Layout.FeesOffset, ReserveFees.Layout.Length));
            DepositLimit = data.GetU64(Layout.DepositLimitOffset);
            BorrowLimit = data.GetU64(Layout.BorrowLimitOffset);
            FeeReceiver = data.GetPubKey(Layout.FeeReceiverOffset);
        }

        /// <summary>
        /// Deserialize the given byte array into the <see cref="ReserveConfig"/> structure.
        /// </summary>
        /// <param name="data">The byte array.</param>
        /// <returns>The <see cref="ReserveConfig"/> instance.</returns>
        public static ReserveConfig Deserialize(byte[] data)
            => new(data.AsSpan());

        /// <summary>
        /// Serializes the <see cref="ReserveConfig"/> object to the given buffer at the desired offset.
        /// </summary>
        /// <param name="buffer">The buffer to serialize into.</param>
        /// <param name="offset">The offset at which to begin serialization.</param>
        public void Serialize(byte[] buffer, int offset = 0)
        {
            buffer.WriteU8(OptimalUtilizationRate, offset + Layout.OptimalUtilizationRateOffset);
            buffer.WriteU8(LoanToValueRatio, offset + Layout.LoanToValueRatioOffset);
            buffer.WriteU8(LiquidationBonus, offset + Layout.LiquidationBonusOffset);
            buffer.WriteU8(LiquidationThreshold, offset + Layout.LiquidationThresholdOffset);
            buffer.WriteU8(MinBorrowRate, offset + Layout.MinBorrowRateOffset);
            buffer.WriteU8(OptimalBorrowRate, offset + Layout.OptimalBorrowRateOffset);
            buffer.WriteU8(MaxBorrowRate, offset + Layout.MaxBorrowRateOffset);
            Fees.Serialize(buffer, offset + Layout.FeesOffset);
        }
    }
}