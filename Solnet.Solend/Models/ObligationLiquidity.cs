using Solnet.Programs.Utilities;
using Solnet.Wallet;
using System;
using System.Numerics;

namespace Solnet.Solend.Models
{
    /// <summary>
    /// Represents an obligation liquidity in Solend.
    /// </summary>
    public class ObligationLiquidity
    {
        /// <summary>
        /// The layout of the <see cref="ObligationLiquidity"/> structure.
        /// </summary>
        public static class Layout
        {
            /// <summary>
            /// The length of the <see cref="ObligationLiquidity"/> structure.
            /// </summary>
            public const int Length = 112;

            /// <summary>
            /// The offset at which the borrow reserve value begins.
            /// </summary>
            public const int BorrowReserveOffset = 0;

            /// <summary>
            /// The offset at which the cumulative borrow rate value begins.
            /// </summary>
            public const int CumulativeBorrowRateOffset = 32;

            /// <summary>
            /// The offset at which the borrow amount value begins.
            /// </summary>
            public const int BorrowAmountOffset = 48;

            /// <summary>
            /// The offset at which the market value begins.
            /// </summary>
            public const int MarketValueOffset = 64;
        }

        /// <summary>
        /// Reserve liquidity is borrowed from
        /// </summary>
        public PublicKey BorrowReserve;

        /// <summary>
        /// Borrow rate used for calculating interest
        /// </summary>
        public BigInteger CumulativeBorrowRateWads;

        /// <summary>
        /// Amount of liquidity borrowed plus interest
        /// </summary>
        public BigInteger BorrowedAmountWads;

        /// <summary>
        /// Liquidity market value in quote currency
        /// </summary>
        public BigInteger MarketValue;

        /// <summary>
        /// Initialize the <see cref="ObligationLiquidity"/> with the given data.
        /// </summary>
        /// <param name="data">The byte array.</param>
        public ObligationLiquidity(ReadOnlySpan<byte> data)
        {
            if (data.Length != Layout.Length)
                throw new ArgumentException($"{nameof(data)} has wrong size. Expected {Layout.Length} bytes, actual {data.Length} bytes.");

            BorrowReserve = data.GetPubKey(Layout.BorrowReserveOffset);
            CumulativeBorrowRateWads = data.GetBigInt(Layout.CumulativeBorrowRateOffset, 16, true);
            BorrowedAmountWads = data.GetBigInt(Layout.BorrowAmountOffset, 16, true);
            MarketValue = data.GetBigInt(Layout.MarketValueOffset, 16, true);
        }

        /// <summary>
        /// Deserialize the given byte array into the <see cref="ObligationLiquidity"/> structure.
        /// </summary>
        /// <param name="data">The byte array.</param>
        /// <returns>The <see cref="ObligationLiquidity"/> instance.</returns>
        public static ObligationLiquidity Deserialize(byte[] data) => new(data.AsSpan());
    }
}
