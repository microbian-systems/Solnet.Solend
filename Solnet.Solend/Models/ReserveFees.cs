using Solnet.Programs.Utilities;
using System;

namespace Solnet.Solend.Models
{

    /// <summary>
    /// Additional fee information on a reserve.
    ///
    /// These exist separately from interest accrual fees, and are specifically for the program owner
    /// and frontend host. The fees are paid out as a percentage of liquidity token amounts during
    /// repayments and liquidations.
    /// </summary>
    public class ReserveFees
    {
        /// <summary>
        /// The layout of the <see cref="ReserveFees"/> structure.
        /// </summary>
        public static class Layout
        {
            /// <summary>
            /// The length of the <see cref="ReserveFees"/>  structure.
            /// </summary>
            public const int Length = 17;

            /// <summary>
            /// The offset at which the borrow fee value begins.
            /// </summary>
            public const int BorrowFeeWadOffset = 0;

            /// <summary>
            /// The offset at which the flash loan fee value begins.
            /// </summary>
            public const int FlashLoanFeeWadOffset = 8;

            /// <summary>
            /// The offset at which the host fee percentage value begins.
            /// </summary>
            public const int HostFeePercentageOffset = 16;
        }

        /// <summary>
        /// Fee assessed on `BorrowObligationLiquidity`, expressed as a Wad.
        /// Must be between 0 and 10^18, such that 10^18 = 1.  A few examples for
        /// clarity:
        /// 1% = 10_000_000_000_000_000
        /// 0.01% (1 basis point) = 100_000_000_000_000
        /// 0.00001% (Aave borrow fee) = 100_000_000_000
        /// </summary>
        public ulong BorrowFeeWad;

        /// <summary>
        /// Fee for flash loan, expressed as a Wad.
        /// 0.3% (Aave flash loan fee) = 3_000_000_000_000_000
        /// </summary>
        public ulong FlashLoanFeeWad;

        /// <summary>
        /// Amount of fee going to host account, if provided in liquidate and repay
        /// </summary>
        public byte HostFeePercentage;

        /// <summary>
        /// Initialize the <see cref="ReserveFees"/> with the given data.
        /// </summary>
        /// <param name="data">The byte array.</param>
        public ReserveFees(ReadOnlySpan<byte> data)
        {
            if (data.Length != Layout.Length)
                throw new ArgumentException($"{nameof(data)} has wrong size. Expected {Layout.Length} bytes, actual {data.Length} bytes.");

            BorrowFeeWad = data.GetU64(Layout.BorrowFeeWadOffset);
            FlashLoanFeeWad = data.GetU64(Layout.FlashLoanFeeWadOffset);
            HostFeePercentage = data.GetU8(Layout.HostFeePercentageOffset);
        }

        /// <summary>
        /// Deserialize the given byte array into the <see cref="ReserveFees"/> structure.
        /// </summary>
        /// <param name="data">The byte array.</param>
        /// <returns>The <see cref="ReserveFees"/> instance.</returns>
        public static ReserveFees Deserialize(byte[] data)
            => new(data.AsSpan());
    }
}
