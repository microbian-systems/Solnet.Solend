using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Solnet.Rpc;
using Solnet.Rpc.Core.Http;
using Solnet.Rpc.Messages;
using Solnet.Rpc.Models;
using Solnet.Rpc.Types;
using Solnet.Solend.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Solnet.Solend.Test
{
    [TestClass]
    public class SolendClientTest : BaseTest
    {
        [TestMethod]
        public void GetLendingMarkets()
        {
            string response = File.ReadAllText("Resources/GetProgramAccountsLendingMarkets.json");
            var rpc = SetupGetProgramAccounts(response, "https://ssc-dao.genesysgo.net",
                SolendProgram.MainNetProgramIdKey, LendingMarket.Layout.Length);

            var sut = ClientFactory.GetClient(rpc.Object, programId: SolendProgram.MainNetProgramIdKey);

            var lendingMarkets = sut.GetLendingMarkets();

            Assert.IsNotNull(lendingMarkets);
            Assert.IsTrue(lendingMarkets.WasSuccessful);
            Assert.AreEqual(9, lendingMarkets.ParsedResult.Count);
        }

        [TestMethod]
        public void GetObligations()
        {
            string response = File.ReadAllText("Resources/GetProgramAccountsObligations.json");
            var rpc = SetupGetProgramAccountsFiltered(response, "https://ssc-dao.genesysgo.net",
                SolendProgram.DevNetProgramIdKey, Obligation.Layout.Length,
                Obligation.Layout.OwnerOffset, "hoakwpFB8UoLnPpLC56gsjpY7XbVwaCuRQRMQzN5TVh");

            var sut = ClientFactory.GetClient(rpc.Object, programId: SolendProgram.DevNetProgramIdKey);

            var obligations = sut.GetObligations(new("hoakwpFB8UoLnPpLC56gsjpY7XbVwaCuRQRMQzN5TVh"));

            Assert.IsNotNull(obligations);
            Assert.IsTrue(obligations.WasSuccessful);
            Assert.AreEqual(1, obligations.ParsedResult.Count);
        }

        [TestMethod]
        public void GetReserves()
        {
            string response = File.ReadAllText("Resources/GetProgramAccountsReserves.json");
            var rpc = SetupGetProgramAccountsFiltered(response, "https://ssc-dao.genesysgo.net",
                SolendProgram.MainNetProgramIdKey, Reserve.Layout.Length,
                Reserve.Layout.LendingMarketOffset, "99S4iReDsyxKDViKdXQKWDcB6C3waDmfPWWyb5HAbcZF");

            var sut = ClientFactory.GetClient(rpc.Object, programId: SolendProgram.MainNetProgramIdKey);

            var reserves = sut.GetReserves(new("99S4iReDsyxKDViKdXQKWDcB6C3waDmfPWWyb5HAbcZF"));

            Assert.IsNotNull(reserves);
            Assert.IsTrue(reserves.WasSuccessful);
            Assert.AreEqual(3, reserves.ParsedResult.Count);
        }

        [TestMethod]
        public void GetReserve()
        {

        }

        [TestMethod]
        public void GetObligation()
        {

        }
    }
}
