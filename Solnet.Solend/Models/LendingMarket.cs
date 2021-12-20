using Solnet.Programs.Utilities;
using Solnet.Wallet;
using System;
using TokenLendingProgramLendingMarket = Solnet.Programs.TokenLending.Models.LendingMarket;

namespace Solnet.Solend.Models
{
    /// <summary>
    /// The state of a lending market.
    /// </summary>
    public class LendingMarket : TokenLendingProgramLendingMarket
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
        /// Initialize a new Lending Market from Solend.
        /// </summary>
        /// <param name="data"></param>
        public LendingMarket(ReadOnlySpan<byte> data)
            : base(data.Slice(0, TokenLendingProgramLendingMarket.Layout.Length))
        {
            if (data.Length != ExtraLayout.Length)
                throw new ArgumentException("data length is invalid");

            SwitchboardOracleProgramId = data.GetPubKey(ExtraLayout.SwitchboardOracleProgramIdOffset);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static LendingMarket Deserialize(byte[] data)
            => new LendingMarket(data.AsSpan());
    }
}
