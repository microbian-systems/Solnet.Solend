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
    public class ReserveConfig : Programs.TokenLending.Models.ReserveConfig
    {
        /// <summary>
        /// The layout of the <see cref="ReserveConfig"/> structure.
        /// </summary>
        public static class ExtraLayout
        {
            /// <summary>
            /// The length of the <see cref="ReserveConfig"/> structure.
            /// </summary>
            public const int Length = 72;

            /// <summary>
            /// The offset at which the deposit limit value begins.
            /// </summary>
            public const int DepositLimitOffset = 24;

            /// <summary>
            /// The offset at which the borrow limit value begins.
            /// </summary>
            public const int BorrowLimitOffset = 32;

            /// <summary>
            /// The offset at which the fee receiver public key begins.
            /// </summary>
            public const int FeeReceiverOffset = 40;
        }

        /// <summary>
        /// Maximum deposit limit of liquidity in native units, u64::MAX for inf
        /// </summary>
        public ulong DepositLimit;

        /// <summary>
        /// Borrows disabled
        /// </summary>
        public ulong BorrowLimit;

        /// <summary>
        /// Reserve liquidity fee receiver address
        /// </summary>
        public PublicKey FeeReceiver;

        /// <summary>
        /// Initialize the <see cref="ReserveConfig"/> with the given data.
        /// </summary>
        /// <param name="data">The byte array.</param>
        public ReserveConfig(ReadOnlySpan<byte> data) : base(data[..Layout.Length])
        {
            if (data.Length != ExtraLayout.Length)
                throw new ArgumentException($"{nameof(data)} has wrong size. Expected {ExtraLayout.Length} bytes, actual {data.Length} bytes.");

            DepositLimit = data.GetU64(ExtraLayout.DepositLimitOffset);
            BorrowLimit = data.GetU64(ExtraLayout.BorrowLimitOffset);
            FeeReceiver = data.GetPubKey(ExtraLayout.FeeReceiverOffset);
        }

        /// <summary>
        /// Deserialize the given byte array into the <see cref="ReserveConfig"/> structure.
        /// </summary>
        /// <param name="data">The byte array.</param>
        /// <returns>The <see cref="ReserveConfig"/> instance.</returns>
        public static new ReserveConfig Deserialize(byte[] data) => new(data.AsSpan());
    }
}