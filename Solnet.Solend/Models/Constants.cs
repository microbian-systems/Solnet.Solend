using Solnet.Rpc;
using Solnet.Wallet;
using System.Collections.Generic;
using System.Numerics;

namespace Solnet.Solend.Models
{
    /// <summary>
    /// Constants.
    /// </summary>
    public static class Constants
    {
        /// <summary>
        /// Scale of precision.
        /// </summary>
        public const uint Scale = 18;

        /// <summary>
        /// Number of slots per year.
        /// </summary>
        public const int SlotsPerYear = 63072000;

        /// <summary>
        /// The maximum amount of positions opened.
        /// </summary>
        public const int MaxPositions = 6;

        /// <summary>
        /// Identity.
        /// </summary>
        public static readonly BigInteger Wad = new(1_000_000_000_000_000_000);

        /// <summary>
        /// 
        /// </summary>
        public static readonly BigInteger Wang = BigInteger.Parse("1000000000000000000000000000000000000");

        /// <summary>
        /// Half of identity.
        /// </summary>
        public static readonly BigInteger HalfWad = new(500_000_000_000_000_000);

        /// <summary>
        /// Scale for percentages.
        /// </summary>
        public static readonly BigInteger PercentScaler = new(10_000_000_000_000_000);

        /// <summary>
        /// The <see cref="LendingMarket"/> for <see cref="Cluster.MainNet"/>.
        /// </summary>
        public static readonly Dictionary<string, PublicKey> LendingMarkets = new()
        {
            { "Main Pool", new("4UpD2fh7xH3VP9QQaXtsS1YY3bxzWhtfpks7FatyKvdY") },
            { "TURBO SOL Pool", new("7RCz8wb6WXxUhAigok9ttgrVgDFFFbibcirECzWSBauM") },
            { "Invictus Pool", new("5i8SzwX2LjpGUxLZRJ8EiYohpuKgW2FYDFhVjhGj66P1") },
            { "Bonfida Pool", new("91taAt3bocVZwcChVgZTTaQYt2WpBVE3M9PkWekFQx4J") },
            { "Step Pool", new("DxdnNmdWHcW6RGTYiD5ms5f7LNZBaA7Kd1nMfASnzwdY") },
            { "Star Atlas Pool", new("99S4iReDsyxKDViKdXQKWDcB6C3waDmfPWWyb5HAbcZF") },
        }; 

        /// <summary>
        /// The <see cref="LendingMarket"/> for <see cref="Cluster.DevNet"/>.
        /// </summary>
        public static readonly Dictionary<string, PublicKey> DevNetLendingMarkets = new()
        {
            { "Main Pool", new("GvjoVKNjBvQcFaSKUW1gTE7DxhSpjHbE69umVR5nPuQp") },
        }; 
    }
}
