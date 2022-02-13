using System;

namespace Solnet.Solend.Models
{

    /// <summary>
    /// Represents an obligation liquidity in Solend.
    /// </summary>
    public class ObligationLiquidity : Programs.TokenLending.Models.ObligationLiquidity
    {
        /// <summary>
        /// The layout of the <see cref="ObligationLiquidity"/> structure.
        /// </summary>
        public static class ExtraLayout
        {
            /// <summary>
            /// The length of the <see cref="ObligationLiquidity"/> structure.
            /// </summary>
            public const int Length = 112;
        }

        /// <summary>
        /// Initialize the <see cref="ObligationLiquidity"/> with the given data.
        /// </summary>
        /// <param name="data">The byte array.</param>
        public ObligationLiquidity(ReadOnlySpan<byte> data) : base(data[..Layout.Length]) { }

        /// <summary>
        /// Deserialize the given byte array into the <see cref="ObligationLiquidity"/> structure.
        /// </summary>
        /// <param name="data">The byte array.</param>
        /// <returns>The <see cref="ObligationLiquidity"/> instance.</returns>
        public static new ObligationLiquidity Deserialize(byte[] data) => new(data.AsSpan());
    }


}
