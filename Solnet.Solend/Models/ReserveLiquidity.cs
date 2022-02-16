using Solnet.Programs.Utilities;
using Solnet.Wallet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Solnet.Solend.Models
{
    /// <summary>
    /// Represents a reserve's config in Solend.
    /// </summary>
    public class ReserveLiquidity
    {
        /// <summary>
        /// The layout of the <see cref="ReserveLiquidity"/> structure.
        /// </summary>
        public static class Layout
        {
            /// <summary>
            /// The length of the <see cref="ReserveLiquidity"/> structure.
            /// </summary>
            public const int Length = 185;

            /// <summary>
            /// The offset at which the mint value begins.
            /// </summary>
            public const int MintOffset = 0;

            /// <summary>
            /// The offset at which the decimals value begins.
            /// </summary>
            public const int DecimalsOffset = 32;

            /// <summary>
            /// The offset at which the supply value begins.
            /// </summary>
            public const int SupplyOffset = 33;

            /// <summary>
            /// The offset at which the pyth oracle value begins.
            /// </summary>
            public const int PythOracleOffset = 65;

            /// <summary>
            /// The offset at which the switchboard oracle value begins.
            /// </summary>
            public const int SwitchboardOracleOffset = 97;

            /// <summary>
            /// The offset at which the available amount offset begins.
            /// </summary>
            public const int AvailableAmountOffset = 129;

            /// <summary>
            /// The offset at which the borrow amount value begins.
            /// </summary>
            public const int BorrowedAmountOffset = 137;

            /// <summary>
            /// The offset at which the cumulative borrow amount value begins.
            /// </summary>
            public const int CumulativeBorrowAmountOffset = 153;

            /// <summary>
            /// The offset at which the market price value begins.
            /// </summary>
            public const int MarketPriceOffset = 169;
        }

        /// <summary>
        /// Reserve liquidity mint address
        /// </summary>
        public PublicKey Mint;

        /// <summary>
        /// Reserve liquidity mint decimals
        /// </summary>
        public byte Decimals;

        /// <summary>
        /// Reserve liquidity supply address
        /// </summary>
        public PublicKey Supply;

        /// <summary>
        /// Reserve liquidity oracle account
        /// </summary>
        public PublicKey PythOracle;

        /// <summary>
        /// The switchboard oracle account.
        /// </summary>
        public PublicKey SwitchboardOracle;

        /// <summary>
        /// Reserve liquidity available
        /// </summary>
        public ulong AvailableAmount;

        /// <summary>
        /// Reserve liquidity borrowed
        /// </summary>
        public BigInteger BorrowedAmountWads;

        /// <summary>
        /// Reserve liquidity cumulative borrow rate
        /// </summary>
        public BigInteger CumulativeBorrowAmountWads;

        /// <summary>
        /// Reserve liquidity market price in quote currency
        /// </summary>
        public BigInteger MarketPrice;

        /// <summary>
        /// Initialize the <see cref="ReserveLiquidity"/> with the given data.
        /// </summary>
        /// <param name="data">The byte array.</param>
        public ReserveLiquidity(ReadOnlySpan<byte> data)
        {
            if (data.Length != Layout.Length)
                throw new ArgumentException($"{nameof(data)} has wrong size. Expected {Layout.Length} bytes, actual {data.Length} bytes.");

            Mint = data.GetPubKey(Layout.MintOffset);
            Decimals = data.GetU8(Layout.DecimalsOffset);
            Supply = data.GetPubKey(Layout.SupplyOffset);
            PythOracle = data.GetPubKey(Layout.PythOracleOffset);
            SwitchboardOracle = data.GetPubKey(Layout.SwitchboardOracleOffset);
            AvailableAmount = data.GetU64(Layout.AvailableAmountOffset);
            BorrowedAmountWads = data.GetBigInt(Layout.BorrowedAmountOffset, 16, true);
            CumulativeBorrowAmountWads = data.GetBigInt(Layout.CumulativeBorrowAmountOffset, 16, true);
            MarketPrice = data.GetBigInt(Layout.MarketPriceOffset, 16, true);
        }

        /// <summary>
        /// Deserialize the given byte array into the <see cref="ReserveLiquidity"/> structure.
        /// </summary>
        /// <param name="data">The byte array.</param>
        /// <returns>The <see cref="ReserveLiquidity"/> instance.</returns>
        public static ReserveLiquidity Deserialize(byte[] data) => new(data.AsSpan());
    }
}
