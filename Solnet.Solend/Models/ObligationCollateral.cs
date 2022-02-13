using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solnet.Solend.Models
{
    /// <summary>
    /// Represents an obligation collateral in Solend.
    /// </summary>
    public class ObligationCollateral : Programs.TokenLending.Models.ObligationCollateral
    {
        /// <summary>
        /// The layout of the <see cref="ObligationCollateral"/> structure.
        /// </summary>
        public static class ExtraLayout
        {
            /// <summary>
            /// The length of the <see cref="ObligationCollateral"/> structure.
            /// </summary>
            public const int Length = 88;
        }

        /// <summary>
        /// Initialize the <see cref="ObligationCollateral"/> with the given data.
        /// </summary>
        /// <param name="data">The byte array.</param>
        public ObligationCollateral(ReadOnlySpan<byte> data) : base(data[..Layout.Length]) { }

        /// <summary>
        /// Deserialize the given byte array into the <see cref="ObligationCollateral"/> structure.
        /// </summary>
        /// <param name="data">The byte array.</param>
        /// <returns>The <see cref="ObligationCollateral"/> instance.</returns>
        public static new ObligationCollateral Deserialize(byte[] data) => new(data.AsSpan());
    }
}
