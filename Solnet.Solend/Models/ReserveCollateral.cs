using Solnet.Programs.Utilities;
using Solnet.Wallet;
using System;

namespace Solnet.Solend.Models
{

    /// <summary>
    /// Reserve collateral
    /// </summary>
    public class ReserveCollateral
    {
        /// <summary>
        /// The layout of the <see cref="ReserveCollateral"/> structure.
        /// </summary>
        public static class Layout
        {
            /// <summary>
            /// The length of the <see cref="ReserveCollateral"/> structure.
            /// </summary>
            public const int Length = 72;

            /// <summary>
            /// The offset at which the borrow fee value begins.
            /// </summary>
            public const int MintOffset = 0;

            /// <summary>
            /// The offset at which the flash loan fee value begins.
            /// </summary>
            public const int TotalSupplyOffset = 32;

            /// <summary>
            /// The offset at which the host fee percentage value begins.
            /// </summary>
            public const int SupplyOffset = 40;
        }

        /// <summary>
        /// Reserve collateral mint address
        /// </summary>
        public PublicKey Mint;

        /// <summary>
        /// Reserve collateral mint supply, used for exchange rate
        /// </summary>
        public ulong TotalSupply;

        /// <summary>
        /// Reserve collateral supply address
        /// </summary>
        public PublicKey Supply;

        /// <summary>
        /// Initialize the <see cref="ReserveCollateral"/> with the given data.
        /// </summary>
        /// <param name="data">The byte array.</param>
        public ReserveCollateral(ReadOnlySpan<byte> data)
        {
            if (data.Length != Layout.Length)
                throw new ArgumentException($"{nameof(data)} has wrong size. Expected {Layout.Length} bytes, actual {data.Length} bytes.");
            Mint = data.GetPubKey(Layout.MintOffset);
            TotalSupply = data.GetU64(Layout.TotalSupplyOffset);
            Supply = data.GetPubKey(Layout.SupplyOffset);
        }

        /// <summary>
        /// Deserialize the given byte array into the <see cref="ReserveCollateral"/> structure.
        /// </summary>
        /// <param name="data">The byte array.</param>
        /// <returns>The <see cref="ReserveCollateral"/> instance.</returns>
        public static ReserveCollateral Deserialize(byte[] data) => new(data.AsSpan());
    }
}
