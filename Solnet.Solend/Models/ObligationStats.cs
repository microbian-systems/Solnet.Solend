using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solnet.Solend.Models
{
    /// <summary>
    /// Represents the <see cref="Obligation"/>'s stats.
    /// </summary>
    public class ObligationStats
    {
        /// <summary>
        /// The liquidation threshold.
        /// </summary>
        public decimal LiquidationThreshold { get; set; }

        /// <summary>
        /// The total deposited amount.
        /// </summary>
        public decimal UserTotalDeposit { get; set; }

        /// <summary>
        /// The total borrowed amount.
        /// </summary>
        public decimal UserTotalBorrow { get; set; }

        /// <summary>
        /// The borrow limit.
        /// </summary>
        public decimal BorrowLimit { get; set; }

        /// <summary>
        /// The borrow utilization.
        /// </summary>
        public decimal BorrowUtilization { get; set; }

        /// <summary>
        /// The net account value.
        /// </summary>
        public decimal NetAccountValue { get; set; }

        /// <summary>
        /// The number of positions.
        /// </summary>
        public int Positions { get; set; }
    }
}
