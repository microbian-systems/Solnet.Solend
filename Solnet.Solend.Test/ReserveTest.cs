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
    public class ReserveTest : BaseTest
    {
        [TestMethod]
        public void CalculateBorrowApr()
        {
            string reservesResponse = File.ReadAllText("Resources/CalculatePositionsGetReserves.json");
            var rpc = SetupGetProgramAccountsFiltered(reservesResponse, "https://ssc-dao.genesysgo.net",
                SolendProgram.DevNetProgramIdKey, Reserve.Layout.Length,
                Reserve.Layout.LendingMarketOffset, "GvjoVKNjBvQcFaSKUW1gTE7DxhSpjHbE69umVR5nPuQp");

            var sut = ClientFactory.GetClient(rpc.Object, programId: SolendProgram.DevNetProgramIdKey);

            var reserves = sut.GetReserves(new("GvjoVKNjBvQcFaSKUW1gTE7DxhSpjHbE69umVR5nPuQp"));

            Assert.IsNotNull(reserves);
            Assert.IsTrue(reserves.WasSuccessful);
            Assert.AreEqual(13, reserves.ParsedResult.Count);

            // this is the soETH reserve
            var last = reserves.ParsedResult.Last();
            Assert.IsNotNull(last);

            var borrowApr = last.CalculateBorrowApr();
            var borrowApy = last.CalculateBorrowApy();
            var supplyApr = last.CalculateSupplyApr();
            var supplyApy = last.CalculateSupplyApy();
            var utilization = last.CalculateUtilizationRatio();
            var available = last.GetAvailableAmount();
            var availableUsd = last.GetAvailableAmountUsd();
            var exchangeRate = last.GetCTokenExchangeRate();
            var borrowed = last.GetTotalBorrow();
            var supplied = last.GetTotalSupply();
            var borrowedUsd = last.GetTotalBorrowUsd();
            var suppliedUsd = last.GetTotalSupplyUsd();

            Assert.AreEqual(0.0074299015787158947707665836m, borrowApr);
            Assert.AreEqual(0.0005520343746940494545851368m, supplyApr);
            Assert.AreEqual(0.00745756525565m, borrowApy);
            Assert.AreEqual(0.00055219350356m, supplyApy);
            Assert.AreEqual(0.0742990157871589477076658356m, utilization);
            Assert.AreEqual(246.948349m, available);
            Assert.AreEqual(652087.36776860149m, availableUsd);
            Assert.AreEqual(1.030420078553382007m, exchangeRate);
            Assert.AreEqual(52338.12047163876m, borrowedUsd);
            Assert.AreEqual(683629.42882614666m, suppliedUsd);
            Assert.AreEqual(19.820676m, borrowed);
            Assert.AreEqual(258.893466m, supplied);
        }
    }
}
