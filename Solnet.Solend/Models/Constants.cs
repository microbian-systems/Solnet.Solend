using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

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
    }
}
