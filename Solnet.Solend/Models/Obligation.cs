using System;

namespace Solnet.Solend.Models
{
    /// <summary>
    /// Represents an obligation in Solend.
    /// </summary>
    public class Obligation : Programs.TokenLending.Models.Obligation
    {
        /// <summary>
        /// The layout of the <see cref="Obligation"/> structure.
        /// </summary>
        public static class ExtraLayout
        {
            /// <summary>
            /// The length of the <see cref="Obligation"/> structure.
            /// </summary>
            public const int Length = 1300;
        }

        /// <summary>
        /// Initialize the <see cref="Obligation"/> with the given data.
        /// </summary>
        /// <param name="data">The byte array.</param>
        public Obligation(ReadOnlySpan<byte> data) : base(data[..Layout.Length]) { }

        /// <summary>
        /// Deserialize the given byte array into the <see cref="Obligation"/> structure.
        /// </summary>
        /// <param name="data">The byte array.</param>
        /// <returns>The <see cref="Obligation"/> instance.</returns>
        public static new Obligation Deserialize(byte[] data) => new(data.AsSpan());
    }
}
