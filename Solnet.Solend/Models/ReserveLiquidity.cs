using Solnet.Programs.Utilities;
using Solnet.Wallet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solnet.Solend.Models
{
    /// <summary>
    /// Represents a reserve's config in Solend.
    /// </summary>
    public class ReserveLiquidity : Programs.TokenLending.Models.ReserveLiquidity
    {
        /// <summary>
        /// The layout of the <see cref="ReserveLiquidity"/> structure.
        /// </summary>
        public static class ExtraLayout
        {
            /// <summary>
            /// The length of the <see cref="ReserveLiquidity"/> structure.
            /// </summary>
            public const int Length = 185;
        }

        /// <summary>
        /// The switchboard oracle account.
        /// </summary>
        public PublicKey SwitchboardOracle;

        /// <summary>
        /// Initialize the <see cref="ReserveLiquidity"/> with the given data.
        /// </summary>
        /// <param name="data">The byte array.</param>
        public ReserveLiquidity(ReadOnlySpan<byte> data) : base(data[..Layout.Length])
        {
            if (data.Length != ExtraLayout.Length)
                throw new ArgumentException($"{nameof(data)} has wrong size. Expected {ExtraLayout.Length} bytes, actual {data.Length} bytes.");

            // in solend, the ReserveLiquidity account has changes to what the values mean but the layout remains the same
            // because of these changes we have to deserialize some attributes again
            // in the future this may mean that subclassing `Programs.TokenLending.Models.ReserveLiquidity` may not be the best idea
            // if the program undergoes more changes for future additions

            // does not contain a `fee_receiver`, it is actually the `pyth_oracle_pubkey`
            FeeReceiver = null;
            Oracle = data.GetPubKey(Layout.FeeReceiverOffset); // the offset is the same, it just has a different purpose

            // the `switchboard_oracle_pubkey` is in the same offset as the `oracle_pubkey` in the base token-lending
            SwitchboardOracle = data.GetPubKey(Layout.OracleOffset);
        }

        /// <summary>
        /// Deserialize the given byte array into the <see cref="ReserveLiquidity"/> structure.
        /// </summary>
        /// <param name="data">The byte array.</param>
        /// <returns>The <see cref="ReserveLiquidity"/> instance.</returns>
        public static new ReserveLiquidity Deserialize(byte[] data) => new(data.AsSpan());
    }
}
