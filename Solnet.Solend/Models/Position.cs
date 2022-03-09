using Solnet.Wallet;

namespace Solnet.Solend.Models
{
    /// <summary>
    /// Represents a position.
    /// </summary>
    public class Position
    {
        /// <summary>
        /// The token mint address.
        /// </summary>
        public PublicKey MintAddress { get; set; }

        /// <summary>
        /// The native amount for UI display.
        /// </summary>
        public decimal NativeAmountUi { get; set; }

        /// <summary>
        /// The native amount.
        /// </summary>
        public decimal NativeAmount { get; set; }

        /// <summary>
        /// The amount in USD.
        /// </summary>
        public decimal AmountUsd { get; set; }
    }
}
