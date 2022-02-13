using Solnet.Programs.Utilities;
using Solnet.Wallet;
using System;

namespace Solnet.Solend.Models
{
    /// <summary>
    /// Represents a lending market in Solend.
    /// </summary>
    public class LendingMarket : Programs.TokenLending.Models.LendingMarket
    {
        /// <summary>
        /// The layout of the structure.
        /// </summary>
        public static class ExtraLayout
        {
            /// <summary>
            /// The length of the structure.
            /// </summary>
            public const int Length = 290;
            /// <summary>
            /// The offset of the OracleProgramId property.
            /// </summary>
            public const int SwitchboardOracleProgramIdOffset = 130;
        }

        /// <summary>
        /// Oracle (switchboard) program id.
        /// </summary>
        public PublicKey SwitchboardOracleProgramId;

        /// <summary>
        /// Initialize a new <see cref="LendingMarket"/> from Solend with the given data..
        /// </summary>
        /// <param name="data">The data to deserialize into the structure.</param>
        public LendingMarket(ReadOnlySpan<byte> data) : base(data[..Layout.Length])
        {
            if (data.Length != ExtraLayout.Length)
                throw new ArgumentException($"{nameof(data)} has wrong size. Expected {ExtraLayout.Length} bytes, actual {data.Length} bytes.");

            SwitchboardOracleProgramId = data.GetPubKey(ExtraLayout.SwitchboardOracleProgramIdOffset);
        }

        /// <summary>
        /// Deserialize the given byte array into the <see cref="LendingMarket"/> structure.
        /// </summary>
        /// <param name="data">The byte array.</param>
        /// <returns>The <see cref="LendingMarket"/> instance.</returns>
        public static new LendingMarket Deserialize(byte[] data) => new (data.AsSpan());
    }
}
