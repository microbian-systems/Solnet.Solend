using Microsoft.VisualStudio.TestTools.UnitTesting;
using Solnet.Solend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solnet.Solend.Test
{
    [TestClass]
    public class DeserializationTest
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ErrorDeserializeLendingMarket()
        {
            _ = LendingMarket.Deserialize(Array.Empty<byte>());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ErrorDeserializeReserve()
        {
            _ = Reserve.Deserialize(Array.Empty<byte>());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ErrorDeserializeObligation()
        {
            _ = Obligation.Deserialize(Array.Empty<byte>());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ErrorDeserializeObligationCollateral()
        {
            _ = ObligationCollateral.Deserialize(Array.Empty<byte>());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ErrorDeserializeObligationLiquidity()
        {
            _ = ObligationLiquidity.Deserialize(Array.Empty<byte>());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ErrorDeserializeReserveConfig()
        {
            _ = ReserveConfig.Deserialize(Array.Empty<byte>());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ErrorDeserializeReserveFees()
        {
            _ = ReserveFees.Deserialize(Array.Empty<byte>());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ErrorDeserializeReserveLiquidity()
        {
            _ = ReserveLiquidity.Deserialize(Array.Empty<byte>());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ErrorDeserializeReserveCollateral()
        {
            _ = ReserveCollateral.Deserialize(Array.Empty<byte>());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ErrorDeserializeLastUpdate()
        {
            _ = LastUpdate.Deserialize(Array.Empty<byte>());
        }
    }
}
