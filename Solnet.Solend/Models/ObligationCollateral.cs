using Solnet.Programs.Utilities;
using Solnet.Wallet;
using System;
using System.Numerics;

namespace Solnet.Solend.Models
{
    /// <summary>
    /// Represents an obligation collateral in Solend.
    /// </summary>
    public class ObligationCollateral
    {
        /// <summary>
        /// The layout of the <see cref="ObligationCollateral"/> structure.
        /// </summary>
        public static class Layout
        {
            /// <summary>
            /// The length of the <see cref="ObligationCollateral"/> structure.
            /// </summary>
            public const int Length = 88;

            /// <summary>
            /// The offset at which the deposit reserve value begins.
            /// </summary>
            public const int DepositReserveOffset = 0;

            /// <summary>
            /// The offset at which the cumulative borrow rate value begins.
            /// </summary>
            public const int DepositedAmountOffset = 32;

            /// <summary>
            /// The offset at which the borrow amount value begins.
            /// </summary>
            public const int MarketValueOffset = 40;
        }

        /// <summary>
        /// Reserve collateral is deposited to
        /// </summary>
        public PublicKey DepositReserve;

        /// <summary>
        /// Amount of collateral deposited
        /// </summary>
        public ulong DepositedAmount;

        /// <summary>
        /// Collateral market value in quote currency
        /// </summary>
        public BigInteger MarketValue;

        /// <summary>
        /// Initialize the <see cref="ObligationCollateral"/> with the given data.
        /// </summary>
        /// <param name="data">The byte array.</param>
        public ObligationCollateral(ReadOnlySpan<byte> data)
        {
            if (data.Length != Layout.Length)
                throw new ArgumentException($"{nameof(data)} has wrong size. Expected {Layout.Length} bytes, actual {data.Length} bytes.");

            DepositReserve = data.GetPubKey(Layout.DepositReserveOffset);
            DepositedAmount = data.GetU64(Layout.DepositedAmountOffset);
            MarketValue = data.GetBigInt(Layout.MarketValueOffset, 16, true);
        }

        /// <summary>
        /// Deserialize the given byte array into the <see cref="ObligationCollateral"/> structure.
        /// </summary>
        /// <param name="data">The byte array.</param>
        /// <returns>The <see cref="ObligationCollateral"/> instance.</returns>
        public static ObligationCollateral Deserialize(byte[] data) => new(data.AsSpan());
    }
}
