using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solnet.Solend.Models
{
    /// <summary>
    /// Represents the position stats, this holds the deposit and borrow <see cref="Position"/>s as well as the <see cref="ObligationStats"/>.
    /// </summary>
    public class PositionStats
    {
        /// <summary>
        /// The borrow positions.
        /// </summary>
        public List<Position> Borrows { get; set; }

        /// <summary>
        /// The deposit positions.
        /// </summary>
        public List<Position> Deposits { get; set; }

        /// <summary>
        /// The obligation stats.
        /// </summary>
        public ObligationStats ObligationStats { get; set; }
    }
}
