using Microsoft.VisualStudio.TestTools.UnitTesting;
using Solnet.Solend.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solnet.Solend.Test
{
    [TestClass]
    public class ObligationTest : BaseTest
    {
        [TestMethod]
        public void CalculatePosition()
        {
            string obligationsResponse = File.ReadAllText("Resources/CalculatePositionsGetObligations.json");
            var rpc = SetupGetProgramAccountsFiltered(obligationsResponse, "https://ssc-dao.genesysgo.net",
                SolendProgram.DevNetProgramIdKey, Obligation.Layout.Length,
                Obligation.Layout.OwnerOffset, "hoakwpFB8UoLnPpLC56gsjpY7XbVwaCuRQRMQzN5TVh");

            var sut = ClientFactory.GetClient(rpc.Object, programId: SolendProgram.DevNetProgramIdKey);

            var obligations = sut.GetObligations(new("hoakwpFB8UoLnPpLC56gsjpY7XbVwaCuRQRMQzN5TVh"));

            Assert.IsNotNull(obligations);
            Assert.IsTrue(obligations.WasSuccessful);

            string reservesResponse = File.ReadAllText("Resources/CalculatePositionsGetReserves.json");
            SetupGetProgramAccountsFiltered(rpc, reservesResponse, "https://ssc-dao.genesysgo.net",
                SolendProgram.DevNetProgramIdKey, Reserve.Layout.Length,
                Reserve.Layout.LendingMarketOffset, "GvjoVKNjBvQcFaSKUW1gTE7DxhSpjHbE69umVR5nPuQp");

            var obligation = obligations.ParsedResult.First();
            Assert.IsNotNull(obligation);

            var reserves = sut.GetReserves(obligation.LendingMarket);

            Assert.IsNotNull(reserves);
            Assert.IsTrue(reserves.WasSuccessful);
            Assert.AreEqual(13, reserves.ParsedResult.Count);

            var position = obligation.CalculatePosition(reserves);

            Assert.AreEqual(3, position.Borrows.Count);
            Assert.AreEqual(1, position.Deposits.Count);
            Assert.AreEqual(4346.7007913682182825727265748m, position.ObligationStats.BorrowLimit);
            Assert.AreEqual(0.1611945327671603876873251991m, position.ObligationStats.BorrowUtilization);
            Assert.AreEqual(5795.601055157624376763635433m, position.ObligationStats.UserTotalDeposit);
            Assert.AreEqual(934.219204190995m, position.ObligationStats.UserTotalBorrow);
            Assert.AreEqual(4861.381850966629376763635433m, position.ObligationStats.NetAccountValue);
            Assert.AreEqual(4636.4808441260995014109083464m, position.ObligationStats.LiquidationThreshold);
            Assert.AreEqual(4, position.ObligationStats.Positions);
        }
    }
}
