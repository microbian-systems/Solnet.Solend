using Microsoft.VisualStudio.TestTools.UnitTesting;
using Solnet.Programs;
using Solnet.Solend.Models;
using Solnet.Wallet;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Solnet.Solend.Test
{
    [TestClass]
    public class SolendProgramTest
    {

        private Reserve LoadReserve(string path)
        {
            var data = File.ReadAllText(path);

            return Reserve.Deserialize(Convert.FromBase64String(data));
        }

        [TestMethod]
        public void CreateMainNet()
        {
            var solend = SolendProgram.CreateMainNet();
            Assert.IsNotNull(solend);
            Assert.AreEqual(SolendProgram.MainNetProgramIdKey, solend.ProgramIdKey);
        }

        [TestMethod]
        public void CreateDevNet()
        {
            var solend = SolendProgram.CreateDevNet();
            Assert.IsNotNull(solend);
            Assert.AreEqual(SolendProgram.DevNetProgramIdKey, solend.ProgramIdKey);
        }

        [TestMethod]
        public void InitializeObligation()
        {
            var solend = SolendProgram.CreateDevNet();
            var lendingMarketPubkey = Constants.DevNetLendingMarkets.Values.First();

            var account = new PublicKey("FcaY8uFsFpuDhRrDyP9JkGJNcQ8fLokkB99KegzVc5ak");

            var obligationPubkey = solend.DeriveObligationAddress(account, lendingMarketPubkey);

            var ix = solend.InitializeObligation(obligationPubkey, lendingMarketPubkey, account);

            Assert.IsNotNull(ix);
            Assert.AreEqual(6, ix.Keys.Count);
            Assert.AreEqual(solend.ProgramIdKey, ix.ProgramId);
            CollectionAssert.AreEqual(new byte[] { 6 }, ix.Data);
        }

        [TestMethod]
        public void RefreshReserve()
        {
            var solend = SolendProgram.CreateDevNet();

            var ix = solend.RefreshReserve(
                new("5VVLD7BQp8y3bTgyF5ezm1ResyMTR3PhYsT4iHFU8Sxz"),
                new("J83w4HKfqxwcq3BEMMkPFSppX3gqekLyLJBexebFVkix"),
                new("AdtRGGhmqvom3Jemp5YNrxd9q9unX36BZk1pujkkXijL")
                );

            Assert.IsNotNull(ix);
            Assert.AreEqual(4, ix.Keys.Count);
            Assert.AreEqual(solend.ProgramIdKey, ix.ProgramId);
            CollectionAssert.AreEqual(new byte[] { 3 }, ix.Data);
        }

        [TestMethod]
        public void RefreshObligation()
        {
            var solend = SolendProgram.CreateDevNet();
            var lendingMarketPubkey = Constants.DevNetLendingMarkets.Values.First();

            var account = new PublicKey("FcaY8uFsFpuDhRrDyP9JkGJNcQ8fLokkB99KegzVc5ak");

            var obligationPubkey = solend.DeriveObligationAddress(account, lendingMarketPubkey);

            var ix = solend.RefreshObligation(
                obligationPubkey,
                new List<PublicKey>() {
                    new("5VVLD7BQp8y3bTgyF5ezm1ResyMTR3PhYsT4iHFU8Sxz")
                });

            Assert.IsNotNull(ix);
            Assert.AreEqual(3, ix.Keys.Count);
            Assert.AreEqual(solend.ProgramIdKey, ix.ProgramId);
            CollectionAssert.AreEqual(new byte[] { 7 }, ix.Data);
        }

        [TestMethod]
        public void DepositReserveLiquidity()
        {
            var reserve = LoadReserve("Resources/Reserves/DevNetSolReserve.txt");
            var solend = SolendProgram.CreateDevNet();
            var lendingMarketPubkey = Constants.DevNetLendingMarkets.Values.First();

            var account = new PublicKey("FcaY8uFsFpuDhRrDyP9JkGJNcQ8fLokkB99KegzVc5ak");

            var liqAta = AssociatedTokenAccountProgram.DeriveAssociatedTokenAccount(account, reserve.Liquidity.Mint);
            var collateralAta = AssociatedTokenAccountProgram.DeriveAssociatedTokenAccount(account, reserve.Collateral.Mint);

            var ix = solend.DepositReserveLiquidity(
                1_000_000,
                liqAta,
                collateralAta,
                new("5VVLD7BQp8y3bTgyF5ezm1ResyMTR3PhYsT4iHFU8Sxz"),
                reserve.Liquidity.Supply,
                reserve.Collateral.Mint,
                lendingMarketPubkey,
                account
                );

            Assert.IsNotNull(ix);
            Assert.AreEqual(10, ix.Keys.Count);
            Assert.AreEqual(solend.ProgramIdKey, ix.ProgramId);
            CollectionAssert.AreEqual(new byte[] { 4, 64, 66 , 15, 0, 0, 0, 0, 0 }, ix.Data);
        }

        [TestMethod]
        public void BorrowObligationLiquidity()
        {
            var reserve = LoadReserve("Resources/Reserves/DevNetSolReserve.txt");
            var solend = SolendProgram.CreateDevNet();
            var lendingMarketPubkey = Constants.DevNetLendingMarkets.Values.First();

            var account = new PublicKey("FcaY8uFsFpuDhRrDyP9JkGJNcQ8fLokkB99KegzVc5ak");

            var obligationPubkey = solend.DeriveObligationAddress(account, lendingMarketPubkey);
            var liqAta = AssociatedTokenAccountProgram.DeriveAssociatedTokenAccount(account, reserve.Liquidity.Mint);
            var collateralAta = AssociatedTokenAccountProgram.DeriveAssociatedTokenAccount(account, reserve.Collateral.Mint);

            var ix = solend.BorrowObligationLiquidity(
                1_000_000,
                reserve.Liquidity.Supply,
                liqAta,
                new("5VVLD7BQp8y3bTgyF5ezm1ResyMTR3PhYsT4iHFU8Sxz"),
                reserve.Config.FeeReceiver,
                obligationPubkey,
                lendingMarketPubkey,
                account
                );

            Assert.IsNotNull(ix);
            Assert.AreEqual(10, ix.Keys.Count);
            Assert.AreEqual(solend.ProgramIdKey, ix.ProgramId);
            CollectionAssert.AreEqual(new byte[] { 10, 64, 66 , 15, 0, 0, 0, 0, 0 }, ix.Data);
        }

        [TestMethod]
        public void BorrowObligationLiquidityWithFeeReceiver()
        {
            var reserve = LoadReserve("Resources/Reserves/DevNetSolReserve.txt");
            var solend = SolendProgram.CreateDevNet();
            var lendingMarketPubkey = Constants.DevNetLendingMarkets.Values.First();

            var feeReceiver = new Account();
            var account = new PublicKey("FcaY8uFsFpuDhRrDyP9JkGJNcQ8fLokkB99KegzVc5ak");

            var obligationPubkey = solend.DeriveObligationAddress(account, lendingMarketPubkey);
            var liqAta = AssociatedTokenAccountProgram.DeriveAssociatedTokenAccount(account, reserve.Liquidity.Mint);
            var collateralAta = AssociatedTokenAccountProgram.DeriveAssociatedTokenAccount(account, reserve.Collateral.Mint);

            var ix = solend.BorrowObligationLiquidity(
                1_000_000,
                reserve.Liquidity.Supply,
                liqAta,
                new("5VVLD7BQp8y3bTgyF5ezm1ResyMTR3PhYsT4iHFU8Sxz"),
                reserve.Config.FeeReceiver,
                obligationPubkey,
                lendingMarketPubkey,
                account,
                feeReceiver
                );

            Assert.IsNotNull(ix);
            Assert.AreEqual(11, ix.Keys.Count);
            Assert.AreEqual(solend.ProgramIdKey, ix.ProgramId);
            CollectionAssert.AreEqual(new byte[] { 10, 64, 66 , 15, 0, 0, 0, 0, 0 }, ix.Data);
        }

        [TestMethod]
        public void RedeemReserveCollateral()
        {
            var reserve = LoadReserve("Resources/Reserves/DevNetSolReserve.txt");
            var solend = SolendProgram.CreateDevNet();
            var lendingMarketPubkey = Constants.DevNetLendingMarkets.Values.First();

            var account = new PublicKey("FcaY8uFsFpuDhRrDyP9JkGJNcQ8fLokkB99KegzVc5ak");

            var liqAta = AssociatedTokenAccountProgram.DeriveAssociatedTokenAccount(account, reserve.Liquidity.Mint);
            var collateralAta = AssociatedTokenAccountProgram.DeriveAssociatedTokenAccount(account, reserve.Collateral.Mint);

            var ix = solend.RedeemReserveCollateral(
                1_000_000,
                collateralAta,
                liqAta,
                new("5VVLD7BQp8y3bTgyF5ezm1ResyMTR3PhYsT4iHFU8Sxz"),
                reserve.Collateral.Mint,
                reserve.Liquidity.Supply,
                lendingMarketPubkey,
                account);

            Assert.IsNotNull(ix);
            Assert.AreEqual(10, ix.Keys.Count);
            Assert.AreEqual(solend.ProgramIdKey, ix.ProgramId);
            CollectionAssert.AreEqual(new byte[] { 5, 64, 66, 15, 0, 0, 0, 0, 0 }, ix.Data);
        }

        [TestMethod]
        public void DepositReserveLiquidityAndObligationCollateral()
        {
            var reserve = LoadReserve("Resources/Reserves/DevNetSolReserve.txt");

            var solend = SolendProgram.CreateDevNet();
            var lendingMarketPubkey = Constants.DevNetLendingMarkets.Values.First();

            var account = new PublicKey("FcaY8uFsFpuDhRrDyP9JkGJNcQ8fLokkB99KegzVc5ak");

            var obligationPubkey = solend.DeriveObligationAddress(account, lendingMarketPubkey);
            var liqAta = AssociatedTokenAccountProgram.DeriveAssociatedTokenAccount(account, reserve.Liquidity.Mint);
            var collateralAta = AssociatedTokenAccountProgram.DeriveAssociatedTokenAccount(account, reserve.Collateral.Mint);

            var ix = solend.DepositReserveLiquidityAndObligationCollateral(
                1_000_000,
                liqAta,
                collateralAta,
                new("5VVLD7BQp8y3bTgyF5ezm1ResyMTR3PhYsT4iHFU8Sxz"),
                reserve.Liquidity.Supply,
                reserve.Collateral.Mint,
                lendingMarketPubkey,
                reserve.Collateral.Supply,
                obligationPubkey,
                account,
                reserve.Liquidity.PythOracle,
                reserve.Liquidity.SwitchboardOracle,
                account);

            Assert.IsNotNull(ix);
            Assert.AreEqual(15, ix.Keys.Count);
            Assert.AreEqual(solend.ProgramIdKey, ix.ProgramId);
            CollectionAssert.AreEqual(new byte[] { 14, 64, 66, 15, 0, 0, 0, 0, 0 }, ix.Data);
        }

        [TestMethod]
        public void WithdrawObligationCollateralAndRedeemReserveCollateral()
        {
            var reserve = LoadReserve("Resources/Reserves/DevNetSolReserve.txt");

            var solend = SolendProgram.CreateDevNet();
            var lendingMarketPubkey = Constants.DevNetLendingMarkets.Values.First();

            var account = new PublicKey("FcaY8uFsFpuDhRrDyP9JkGJNcQ8fLokkB99KegzVc5ak");

            var obligationPubkey = solend.DeriveObligationAddress(account, lendingMarketPubkey);
            var liqAta = AssociatedTokenAccountProgram.DeriveAssociatedTokenAccount(account, reserve.Liquidity.Mint);
            var collateralAta = AssociatedTokenAccountProgram.DeriveAssociatedTokenAccount(account, reserve.Collateral.Mint);

            var ix = solend.WithdrawObligationCollateralAndRedeemReserveCollateral(
                1_000_000,
                collateralAta,
                reserve.Collateral.Supply,
                new("5VVLD7BQp8y3bTgyF5ezm1ResyMTR3PhYsT4iHFU8Sxz"),
                obligationPubkey,
                lendingMarketPubkey,
                liqAta,
                reserve.Collateral.Mint,
                reserve.Liquidity.Supply,
                account,
                account
                );

            Assert.IsNotNull(ix);
            Assert.AreEqual(13, ix.Keys.Count);
            Assert.AreEqual(solend.ProgramIdKey, ix.ProgramId);
            CollectionAssert.AreEqual(new byte[] { 15, 64, 66, 15, 0, 0, 0, 0, 0 }, ix.Data);
        }

        [TestMethod]
        public void DepositObligationCollateral()
        {
            var reserve = LoadReserve("Resources/Reserves/DevNetSolReserve.txt");

            var solend = SolendProgram.CreateDevNet();
            var lendingMarketPubkey = Constants.DevNetLendingMarkets.Values.First();

            var account = new PublicKey("FcaY8uFsFpuDhRrDyP9JkGJNcQ8fLokkB99KegzVc5ak");

            var obligationPubkey = solend.DeriveObligationAddress(account, lendingMarketPubkey);
            var collateralAta = AssociatedTokenAccountProgram.DeriveAssociatedTokenAccount(account, reserve.Collateral.Mint);

            var ix = solend.DepositObligationCollateral(
                1_000_000,
                collateralAta,
                reserve.Collateral.Supply,
                new("5VVLD7BQp8y3bTgyF5ezm1ResyMTR3PhYsT4iHFU8Sxz"),
                obligationPubkey,
                lendingMarketPubkey,
                account,
                account);

            Assert.IsNotNull(ix);
            Assert.AreEqual(9, ix.Keys.Count);
            Assert.AreEqual(solend.ProgramIdKey, ix.ProgramId);
            CollectionAssert.AreEqual(new byte[] { 8, 64, 66, 15, 0, 0, 0, 0, 0 }, ix.Data);
        }

        [TestMethod]
        public void WithdrawObligationCollateral()
        {
            var reserve = LoadReserve("Resources/Reserves/DevNetSolReserve.txt");

            var solend = SolendProgram.CreateDevNet();
            var lendingMarketPubkey = Constants.DevNetLendingMarkets.Values.First();

            var account = new PublicKey("FcaY8uFsFpuDhRrDyP9JkGJNcQ8fLokkB99KegzVc5ak");

            var obligationPubkey = solend.DeriveObligationAddress(account, lendingMarketPubkey);
            var collateralAta = AssociatedTokenAccountProgram.DeriveAssociatedTokenAccount(account, reserve.Collateral.Mint);

            var ix = solend.WithdrawObligationCollateral(
                1_000_000,
                reserve.Collateral.Supply,
                collateralAta,
                new("5VVLD7BQp8y3bTgyF5ezm1ResyMTR3PhYsT4iHFU8Sxz"),
                obligationPubkey,
                lendingMarketPubkey,
                account
                );

            Assert.IsNotNull(ix);
            Assert.AreEqual(9, ix.Keys.Count);
            Assert.AreEqual(solend.ProgramIdKey, ix.ProgramId);
            CollectionAssert.AreEqual(new byte[] { 9, 64, 66, 15, 0, 0, 0, 0, 0 }, ix.Data);
        }

        [TestMethod]
        public void RepayObligationLiquidity()
        {
            var reserve = LoadReserve("Resources/Reserves/DevNetSolReserve.txt");

            var solend = SolendProgram.CreateDevNet();
            var lendingMarketPubkey = Constants.DevNetLendingMarkets.Values.First();

            var account = new PublicKey("FcaY8uFsFpuDhRrDyP9JkGJNcQ8fLokkB99KegzVc5ak");

            var obligationPubkey = solend.DeriveObligationAddress(account, lendingMarketPubkey);
            var liqAta = AssociatedTokenAccountProgram.DeriveAssociatedTokenAccount(account, reserve.Liquidity.Mint);
            var collateralAta = AssociatedTokenAccountProgram.DeriveAssociatedTokenAccount(account, reserve.Collateral.Mint);

            var ix = solend.RepayObligationLiduidity(
                1_000_000,
                liqAta,
                reserve.Liquidity.Supply,
                new("5VVLD7BQp8y3bTgyF5ezm1ResyMTR3PhYsT4iHFU8Sxz"),
                obligationPubkey,
                lendingMarketPubkey,
                account
                );

            Assert.IsNotNull(ix);
            Assert.AreEqual(8, ix.Keys.Count);
            Assert.AreEqual(solend.ProgramIdKey, ix.ProgramId);
            CollectionAssert.AreEqual(new byte[] { 11, 64, 66, 15, 0, 0, 0, 0, 0 }, ix.Data);
        }
    }
}
