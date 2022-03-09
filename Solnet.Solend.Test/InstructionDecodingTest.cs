using Microsoft.VisualStudio.TestTools.UnitTesting;
using Solnet.Programs;
using Solnet.Rpc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solnet.Solend.Test
{
    [TestClass]
    public class InstructionDecodingTest
    {
        private static readonly string InitializeObligationAndDepositMessage
            = "AQALE9kg3ZGWzPkpBm8cZkULK9uWdA3sisjXDdshQTdvQyhNy7rOvVx9CSAJ6m+7cZ5+O2ku1E72lwuC0c7lY6+lNirWCy8agPTv5RUTpOtYBcz47B7SEoJStqlookXaJKhSaHgXhLNzZW2aVIzjy0XOusd7txwXaWsl8cOixffWFItV3ttqMLib+7zUxPMkRnDt5VeV52SuSfZtTiOoGSInUnxCuwtmfMwiTWabXLU3bqJOCA/GcFtPOohc3HFjH3P1Own31a6wojxfXhuYziZ/XOS+1jAJMN210lDMvyZsG952/bGpdWxLgIFcDmDq5iAPV7Hw1Q7WIh4+GHmZW0GOM/YAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAOyje4xa6wSdaaY2+r99GpE76Z57BUIZA3yc05/vfBp1BqfVFxjHdMkoVmOYaR1etoteuKObS21cc1VbIQAAAAAGp9UXGSxcUSGMyUw9SvF/WNruCJuh/UTj29mKAAAAAAbd9uHXZaGT2cvhRs7reawctIXtX1s3kTqM9YV+/wCpisHxckX1kDkXg6O4VnW0a52vXaNf8ljS3xX3bbTnMlkGm4hX/quBhPtof2NGGMA12sQ53BrrO1WYoPAAAAAAAYyXJY9OJInxuz0QKRSODYMLWhOZ2v8QhASOe9jb6fhZy3p0uIF4hi+mIBzzl/scSBL2DFy06YXNR5B8j1TKG9z+ZQ8DZ9Sn75gVpZPqFdNlk/BkOqrwFJuwS+Z6uFHezY8sOvi7biRDq2PsMrsolh4EmTQIVFcYBIJ34Msy48wbQOGDz+i+MZ3onwvqRF1f3AUCQCzxDLGs9ivolg0mNhQHCAIAAXwDAAAA2SDdkZbM+SkGbxxmRQsr25Z0DeyKyNcN2yFBN29DKE0gAAAAAAAAAEd2am9WS05qQnZRY0ZhU0tVVzFnVEU3RHhoU3BqSGJFwKeXAAAAAAAUBQAAAAAAAIrB8XJF9ZA5F4OjuFZ1tGudr12jX/JY0t8V92205zJZDQYBCQAKCwwBBggCAAIMAgAAAPDnuTsAAAAADwcAAgAOCAwLAA8HAAMABAgMCwANDwIDBQYECRAHAQAREgAKDAkOAMqaOwAAAAAMAwIAAAEJ";

        private static readonly string RefreshReserveAndDepositLiquidityMessage
            = "AQALEdkg3ZGWzPkpBm8cZkULK9uWdA3sisjXDdshQTdvQyhN1gsvGoD07+UVE6TrWAXM+Owe0hKCUrapaKJF2iSoUmhCuwtmfMwiTWabXLU3bqJOCA/GcFtPOohc3HFjH3P1O3gXhLNzZW2aVIzjy0XOusd7txwXaWsl8cOixffWFItVCffVrrCiPF9eG5jOJn9c5L7WMAkw3bXSUMy/Jmwb3nbe22owuJv7vNTE8yRGcO3lV5XnZK5J9m1OI6gZIidSfAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAABpuIV/6rgYT7aH9jRhjANdrEOdwa6ztVmKDwAAAAAAEG3fbh12Whk9nL4UbO63msHLSF7V9bN5E6jPWFfv8AqQan1RcZLFxRIYzJTD1K8X9Y2u4Im6H9ROPb2YoAAAAAjJclj04kifG7PRApFI4NgwtaE5na/xCEBI572Nvp+Fn+ZQ8DZ9Sn75gVpZPqFdNlk/BkOqrwFJuwS+Z6uFHezY8sOvi7biRDq2PsMrsolh4EmTQIVFcYBIJ34Msy48wbBqfVFxjHdMkoVmOYaR1etoteuKObS21cc1VbIQAAAACKwfFyRfWQOReDo7hWdbRrna9do1/yWNLfFfdttOcyWeyje4xa6wSdaaY2+r99GpE76Z57BUIZA3yc05/vfBp1y3p0uIF4hi+mIBzzl/scSBL2DFy06YXNR5B8j1TKG9xv2QE0zww8ioRGoKK5zBW5bo46gQurQqhAPIIY1TuqkAUGAgABDAIAAADw57k7AAAAAAoHAAEABwYICQAOBAILDA0BAw4KAQMCBAUPEAANCAkEAMqaOwAAAAAIAwEAAAEJ";

        private static readonly string RefreshReserveAndRedeemReserveCollateralMessage
            = "AQALEdkg3ZGWzPkpBm8cZkULK9uWdA3sisjXDdshQTdvQyhN1gsvGoD07+UVE6TrWAXM+Owe0hKCUrapaKJF2iSoUmhCuwtmfMwiTWabXLU3bqJOCA/GcFtPOohc3HFjH3P1O3gXhLNzZW2aVIzjy0XOusd7txwXaWsl8cOixffWFItV3ttqMLib+7zUxPMkRnDt5VeV52SuSfZtTiOoGSInUnwJ99WusKI8X14bmM4mf1zkvtYwCTDdtdJQzL8mbBvedgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAABpuIV/6rgYT7aH9jRhjANdrEOdwa6ztVmKDwAAAAAAEG3fbh12Whk9nL4UbO63msHLSF7V9bN5E6jPWFfv8AqQan1RcZLFxRIYzJTD1K8X9Y2u4Im6H9ROPb2YoAAAAAjJclj04kifG7PRApFI4NgwtaE5na/xCEBI572Nvp+Fn+ZQ8DZ9Sn75gVpZPqFdNlk/BkOqrwFJuwS+Z6uFHezY8sOvi7biRDq2PsMrsolh4EmTQIVFcYBIJ34Msy48wbBqfVFxjHdMkoVmOYaR1etoteuKObS21cc1VbIQAAAACKwfFyRfWQOReDo7hWdbRrna9do1/yWNLfFfdttOcyWeyje4xa6wSdaaY2+r99GpE76Z57BUIZA3yc05/vfBp1y3p0uIF4hi+mIBzzl/scSBL2DFy06YXNR5B8j1TKG9yHMBoiYjkHIxskzZsTwSSeOY+hOcZn5M0azvJbl/yUNgUGAgABDAIAAADwHR8AAAAAAAoHAAEABwYICQAOBAILDA0BAw4KAwECBAUPEAANCAkFAMqaOwAAAAAIAwEAAAEJ";

        private static readonly string DepositObligationCollateralMessage
            = "AQAGC9kg3ZGWzPkpBm8cZkULK9uWdA3sisjXDdshQTdvQyhNQrsLZnzMIk1mm1y1N26iTggPxnBbTzqIXNxxYx9z9Tt4F4Szc2VtmlSM48tFzrrHe7ccF2lrJfHDosX31hSLVf2xqXVsS4CBXA5g6uYgD1ex8NUO1iIePhh5mVtBjjP2y7rOvVx9CSAJ6m+7cZ5+O2ku1E72lwuC0c7lY6+lNir+ZQ8DZ9Sn75gVpZPqFdNlk/BkOqrwFJuwS+Z6uFHezY8sOvi7biRDq2PsMrsolh4EmTQIVFcYBIJ34Msy48wbBqfVFxjHdMkoVmOYaR1etoteuKObS21cc1VbIQAAAACKwfFyRfWQOReDo7hWdbRrna9do1/yWNLfFfdttOcyWeyje4xa6wSdaaY2+r99GpE76Z57BUIZA3yc05/vfBp1Bt324ddloZPZy+FGzut5rBy0he1fWzeROoz1hX7/AKnh1IevzZ3XA0tUJM1f827c/shZaiqVIyBAMFPVy2aILQIIBAEFBgcBAwgJAgMBBAkAAAcKCQgAypo7AAAAAA==";

        private static readonly string RefreshReservesObligationAndWithdrawCollateralMessage
            = "AQAJD9kg3ZGWzPkpBm8cZkULK9uWdA3sisjXDdshQTdvQyhNQrsLZnzMIk1mm1y1N26iTggPxnBbTzqIXNxxYx9z9TvVfWqLsVTK+kkQQppELNgRHYriqCd0X/MTy66Ugwuu7Mu6zr1cfQkgCepvu3GefjtpLtRO9pcLgtHO5WOvpTYq/bGpdWxLgIFcDmDq5iAPV7Hw1Q7WIh4+GHmZW0GOM/Z4F4Szc2VtmlSM48tFzrrHe7ccF2lrJfHDosX31hSLVf5lDwNn1KfvmBWlk+oV02WT8GQ6qvAUm7BL5nq4Ud7Njyw6+LtuJEOrY+wyuyiWHgSZNAhUVxgEgnfgyzLjzBsGp9UXGMd0yShWY5hpHV62i164o5tLbVxzVVshAAAAAIrB8XJF9ZA5F4OjuFZ1tGudr12jX/JY0t8V92205zJZQfNiWXHKLtImPnhXP+XOI+E9JVjtPy5Hqw+E+5565yKr4VpwXw7TuJxKe/TIU5mszHOE1/5KEWoQmZGJTxQ95uyje4xa6wSdaaY2+r99GpE76Z57BUIZA3yc05/vfBp1y3p0uIF4hi+mIBzzl/scSBL2DFy06YXNR5B8j1TKG9wG3fbh12Whk9nL4UbO63msHLSF7V9bN5E6jPWFfv8AqUAdm1QgaHmR8t7ZcYiULUZL8dB5XcWeLaDOoEAv7QdtBAkEAQYHCAEDCQQCCgsIAQMJBAMIAQIBBwkJBAUBAwwNAAgOCQkAypo7AAAAAA==";

        private static readonly string RefreshReservesObligationAndBorrowObligationLiquidityMessage
            = "AQANFNkg3ZGWzPkpBm8cZkULK9uWdA3sisjXDdshQTdvQyhN1gsvGoD07+UVE6TrWAXM+Owe0hKCUrapaKJF2iSoUmhCuwtmfMwiTWabXLU3bqJOCA/GcFtPOohc3HFjH3P1O9V9aouxVMr6SRBCmkQs2BEdiuKoJ3Rf8xPLrpSDC67sy7rOvVx9CSAJ6m+7cZ5+O2ku1E72lwuC0c7lY6+lNioJ99WusKI8X14bmM4mf1zkvtYwCTDdtdJQzL8mbBvedkaDgeYHl+gNlDj8NO0DuNCA5eFwulJrzv0yGFj/89B+BpuIV/6rgYT7aH9jRhjANdrEOdwa6ztVmKDwAAAAAAEAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAbd9uHXZaGT2cvhRs7reawctIXtX1s3kTqM9YV+/wCpBqfVFxksXFEhjMlMPUrxf1ja7gibof1E49vZigAAAACMlyWPTiSJ8bs9ECkUjg2DC1oTmdr/EIQEjnvY2+n4Wf5lDwNn1KfvmBWlk+oV02WT8GQ6qvAUm7BL5nq4Ud7Njyw6+LtuJEOrY+wyuyiWHgSZNAhUVxgEgnfgyzLjzBsGp9UXGMd0yShWY5hpHV62i164o5tLbVxzVVshAAAAAIrB8XJF9ZA5F4OjuFZ1tGudr12jX/JY0t8V92205zJZQfNiWXHKLtImPnhXP+XOI+E9JVjtPy5Hqw+E+5565yKr4VpwXw7TuJxKe/TIU5mszHOE1/5KEWoQmZGJTxQ95uyje4xa6wSdaaY2+r99GpE76Z57BUIZA3yc05/vfBp1y3p0uIF4hi+mIBzzl/scSBL2DFy06YXNR5B8j1TKG9wKSi763q158J/6/ZlO5Mbtu1ux7HlnOAdW81sjeLrUmwYLBwABAAcICQoADwQCDA0OAQMPBAMQEQ4BAw8EBA4CAwEHDwoFAQIGBBITAA4JCQoAypo7AAAAAAkDAQAAAQk=";

        private static readonly string RefreshReservesObligationAndRepayObligationLiquidityMessage
            = "AQAMEtkg3ZGWzPkpBm8cZkULK9uWdA3sisjXDdshQTdvQyhN1gsvGoD07+UVE6TrWAXM+Owe0hKCUrapaKJF2iSoUmhCuwtmfMwiTWabXLU3bqJOCA/GcFtPOohc3HFjH3P1O9V9aouxVMr6SRBCmkQs2BEdiuKoJ3Rf8xPLrpSDC67sy7rOvVx9CSAJ6m+7cZ5+O2ku1E72lwuC0c7lY6+lNioJ99WusKI8X14bmM4mf1zkvtYwCTDdtdJQzL8mbBvedgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAABpuIV/6rgYT7aH9jRhjANdrEOdwa6ztVmKDwAAAAAAEG3fbh12Whk9nL4UbO63msHLSF7V9bN5E6jPWFfv8AqQan1RcZLFxRIYzJTD1K8X9Y2u4Im6H9ROPb2YoAAAAAjJclj04kifG7PRApFI4NgwtaE5na/xCEBI572Nvp+Fn+ZQ8DZ9Sn75gVpZPqFdNlk/BkOqrwFJuwS+Z6uFHezY8sOvi7biRDq2PsMrsolh4EmTQIVFcYBIJ34Msy48wbBqfVFxjHdMkoVmOYaR1etoteuKObS21cc1VbIQAAAACKwfFyRfWQOReDo7hWdbRrna9do1/yWNLfFfdttOcyWUHzYllxyi7SJj54Vz/lziPhPSVY7T8uR6sPhPueeuciq+FacF8O07icSnv0yFOZrMxzhNf+ShFqEJmRiU8UPebso3uMWusEnWmmNvq/fRqRO+meewVCGQN8nNOf73waddnBVSoJ9oY3Jn48HeoD95HjSrNQnJXLZkM10wxF/P5aCAYCAAEMAgAAAPDnuTsAAAAACgcAAQAHBggJAA4EAgsMDQEDDgQDDxANAQMOBAILDA0BAw4FBA0CAwIBBw4IAQUCBBEADQgJCwDKmjsAAAAACAMBAAABCQ==";

        private static readonly string RefreshReservesObligationAndWithdrawAndRedeemMessage
            = "AQANFtkg3ZGWzPkpBm8cZkULK9uWdA3sisjXDdshQTdvQyhN1gsvGoD07+UVE6TrWAXM+Owe0hKCUrapaKJF2iSoUmhCuwtmfMwiTWabXLU3bqJOCA/GcFtPOohc3HFjH3P1O9V9aouxVMr6SRBCmkQs2BEdiuKoJ3Rf8xPLrpSDC67sy7rOvVx9CSAJ6m+7cZ5+O2ku1E72lwuC0c7lY6+lNir9sal1bEuAgVwOYOrmIA9XsfDVDtYiHj4YeZlbQY4z9ngXhLNzZW2aVIzjy0XOusd7txwXaWsl8cOixffWFItV3ttqMLib+7zUxPMkRnDt5VeV52SuSfZtTiOoGSInUnwJ99WusKI8X14bmM4mf1zkvtYwCTDdtdJQzL8mbBvedgabiFf+q4GE+2h/Y0YYwDXaxDncGus7VZig8AAAAAABAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAG3fbh12Whk9nL4UbO63msHLSF7V9bN5E6jPWFfv8AqQan1RcZLFxRIYzJTD1K8X9Y2u4Im6H9ROPb2YoAAAAAjJclj04kifG7PRApFI4NgwtaE5na/xCEBI572Nvp+Fn+ZQ8DZ9Sn75gVpZPqFdNlk/BkOqrwFJuwS+Z6uFHezY8sOvi7biRDq2PsMrsolh4EmTQIVFcYBIJ34Msy48wbBqfVFxjHdMkoVmOYaR1etoteuKObS21cc1VbIQAAAACKwfFyRfWQOReDo7hWdbRrna9do1/yWNLfFfdttOcyWUHzYllxyi7SJj54Vz/lziPhPSVY7T8uR6sPhPueeuciq+FacF8O07icSnv0yFOZrMxzhNf+ShFqEJmRiU8UPebso3uMWusEnWmmNvq/fRqRO+meewVCGQN8nNOf73wadct6dLiBeIYvpiAc85f7HEgS9gxctOmFzUeQfI9UyhvcRykERPhKdQ7YwczsyvS3bA4MPBate8/CuAX98+CRcdcHDQcAAQAJCgsMABEEAg4PEAEDEQQDEhMQAQMRBAIODxABAxEFBBACAwIBBxENBQYCBBQVAQcIAAAQCwkPAMqaOwAAAAALAwEAAAEJ";

        [ClassInitialize]
        public static void Setup(TestContext tc)
        {
            InstructionDecoder.Register(SolendProgram.DevNetProgramIdKey, SolendProgram.Decode);
            // do not assert the program key of the decoded instruction because it defaults to mainnet
        }

        [TestMethod]
        public void InitializeObligationAndDeposit()
        {
            Message msg = Message.Deserialize(Convert.FromBase64String(InitializeObligationAndDepositMessage));
            List<DecodedInstruction> ix =
                InstructionDecoder.DecodeInstructions(msg);

            Assert.AreEqual(7, ix.Count);
            Assert.AreEqual("Solend Program", ix[1].ProgramName);
            Assert.AreEqual("Initialize Obligation", ix[1].InstructionName);
            Assert.AreEqual("EiGycyytNhNtfsLZLFfCwspjBhhVem284qqP4AQ38897", ix[1].Values.GetValueOrDefault("Obligation").ToString());
            Assert.AreEqual("GvjoVKNjBvQcFaSKUW1gTE7DxhSpjHbE69umVR5nPuQp", ix[1].Values.GetValueOrDefault("Lending Market").ToString());
            Assert.AreEqual("FcaY8uFsFpuDhRrDyP9JkGJNcQ8fLokkB99KegzVc5ak", ix[1].Values.GetValueOrDefault("Obligation Owner").ToString());
            Assert.AreEqual(SysVars.ClockKey, ix[1].Values.GetValueOrDefault("Sysvar Clock").ToString());
            Assert.AreEqual(SysVars.RentKey, ix[1].Values.GetValueOrDefault("Sysvar Rent").ToString());
            Assert.AreEqual(TokenProgram.ProgramIdKey, ix[1].Values.GetValueOrDefault("Token Program").ToString());

            Assert.AreEqual("Solend Program", ix[5].ProgramName);
            Assert.AreEqual("Deposit Reserve Liquidity And Obligation Collateral", ix[5].InstructionName);
            Assert.AreEqual("FQY92w6qva4TakeTPf7ruAoKciobnmfk7NJG47se4PKu", ix[5].Values.GetValueOrDefault("Source Liquidity").ToString());
            Assert.AreEqual("95nixpmsxz3MQRfxotnWNGK7eh6QsKcf7FJZoVfCnACU", ix[5].Values.GetValueOrDefault("Destination Collateral").ToString());
            Assert.AreEqual("5VVLD7BQp8y3bTgyF5ezm1ResyMTR3PhYsT4iHFU8Sxz", ix[5].Values.GetValueOrDefault("Reserve").ToString());
            Assert.AreEqual("furd3XUtjXZ2gRvSsoUts9A5m8cMJNqdsyR2Rt8vY9s", ix[5].Values.GetValueOrDefault("Reserve Liquidity Supply").ToString());
            Assert.AreEqual("FzwZWRMc3GCqjSrcpVX3ueJc6UpcV6iWWb7ZMsTXE3Gf", ix[5].Values.GetValueOrDefault("Reserve Collateral Mint").ToString());
            Assert.AreEqual("EhJ4fwaXUp7aiwvZThSUaGWCaBQAJe3AEaJJJVCn3UCK", ix[5].Values.GetValueOrDefault("Lending Market Authority").ToString());
            Assert.AreEqual("J5KGpESS8Zq2MvK4rtL6wKbeMRYZzb6TEzn8qPsZFgGd", ix[5].Values.GetValueOrDefault("Destination Deposit Collateral").ToString());
            Assert.AreEqual("J83w4HKfqxwcq3BEMMkPFSppX3gqekLyLJBexebFVkix", ix[5].Values.GetValueOrDefault("Reserve Liquidity Pyth Oracle").ToString());
            Assert.AreEqual("AdtRGGhmqvom3Jemp5YNrxd9q9unX36BZk1pujkkXijL", ix[5].Values.GetValueOrDefault("Reserve Liquidity Switchboard Oracle").ToString());
            Assert.AreEqual("EiGycyytNhNtfsLZLFfCwspjBhhVem284qqP4AQ38897", ix[5].Values.GetValueOrDefault("Obligation").ToString());
            Assert.AreEqual("GvjoVKNjBvQcFaSKUW1gTE7DxhSpjHbE69umVR5nPuQp", ix[5].Values.GetValueOrDefault("Lending Market").ToString());
            Assert.AreEqual("FcaY8uFsFpuDhRrDyP9JkGJNcQ8fLokkB99KegzVc5ak", ix[5].Values.GetValueOrDefault("Obligation Owner").ToString());
            Assert.AreEqual("FcaY8uFsFpuDhRrDyP9JkGJNcQ8fLokkB99KegzVc5ak", ix[5].Values.GetValueOrDefault("User Transfer Authority").ToString());
            Assert.AreEqual("1000000000", ix[5].Values.GetValueOrDefault("Liquidity Amount").ToString());
            Assert.AreEqual(SysVars.ClockKey, ix[5].Values.GetValueOrDefault("Sysvar Clock").ToString());
            Assert.AreEqual(TokenProgram.ProgramIdKey, ix[5].Values.GetValueOrDefault("Token Program").ToString());
        }

        [TestMethod]
        public void RefreshReserveAndDepositLiquidity()
        {
            Message msg = Message.Deserialize(Convert.FromBase64String(RefreshReserveAndDepositLiquidityMessage));
            List<DecodedInstruction> ix =
                InstructionDecoder.DecodeInstructions(msg);

            Assert.AreEqual(5, ix.Count);
            Assert.AreEqual("Solend Program", ix[2].ProgramName);
            Assert.AreEqual("Refresh Reserve", ix[2].InstructionName);
            Assert.AreEqual("5VVLD7BQp8y3bTgyF5ezm1ResyMTR3PhYsT4iHFU8Sxz", ix[2].Values.GetValueOrDefault("Reserve").ToString());
            Assert.AreEqual("J83w4HKfqxwcq3BEMMkPFSppX3gqekLyLJBexebFVkix", ix[2].Values.GetValueOrDefault("Reserve Liquidity Pyth Oracle").ToString());
            Assert.AreEqual("AdtRGGhmqvom3Jemp5YNrxd9q9unX36BZk1pujkkXijL", ix[2].Values.GetValueOrDefault("Reserve Liquidity Switchboard Oracle").ToString());
            Assert.AreEqual(SysVars.ClockKey, ix[2].Values.GetValueOrDefault("Sysvar Clock").ToString());

            Assert.AreEqual("Solend Program", ix[3].ProgramName);
            Assert.AreEqual("Deposit Reserve Liquidity", ix[3].InstructionName);
            Assert.AreEqual("FQY92w6qva4TakeTPf7ruAoKciobnmfk7NJG47se4PKu", ix[3].Values.GetValueOrDefault("Source Liquidity").ToString());
            Assert.AreEqual("95nixpmsxz3MQRfxotnWNGK7eh6QsKcf7FJZoVfCnACU", ix[3].Values.GetValueOrDefault("Destination Collateral").ToString());
            Assert.AreEqual("5VVLD7BQp8y3bTgyF5ezm1ResyMTR3PhYsT4iHFU8Sxz", ix[3].Values.GetValueOrDefault("Reserve").ToString());
            Assert.AreEqual("FzwZWRMc3GCqjSrcpVX3ueJc6UpcV6iWWb7ZMsTXE3Gf", ix[3].Values.GetValueOrDefault("Reserve Collateral Mint").ToString());
            Assert.AreEqual("furd3XUtjXZ2gRvSsoUts9A5m8cMJNqdsyR2Rt8vY9s", ix[3].Values.GetValueOrDefault("Reserve Liquidity Supply").ToString());
            Assert.AreEqual("GvjoVKNjBvQcFaSKUW1gTE7DxhSpjHbE69umVR5nPuQp", ix[3].Values.GetValueOrDefault("Lending Market").ToString());
            Assert.AreEqual("EhJ4fwaXUp7aiwvZThSUaGWCaBQAJe3AEaJJJVCn3UCK", ix[3].Values.GetValueOrDefault("Lending Market Authority").ToString());
            Assert.AreEqual("FcaY8uFsFpuDhRrDyP9JkGJNcQ8fLokkB99KegzVc5ak", ix[3].Values.GetValueOrDefault("User Transfer Authority").ToString());
            Assert.AreEqual("1000000000", ix[3].Values.GetValueOrDefault("Liquidity Amount").ToString());
            Assert.AreEqual(SysVars.ClockKey, ix[3].Values.GetValueOrDefault("Sysvar Clock").ToString());
            Assert.AreEqual(TokenProgram.ProgramIdKey, ix[3].Values.GetValueOrDefault("Token Program").ToString());
        }

        [TestMethod]
        public void RefreshReserveAndRedeemReserveCollateral()
        {
            Message msg = Message.Deserialize(Convert.FromBase64String(RefreshReserveAndRedeemReserveCollateralMessage));
            List<DecodedInstruction> ix =
                InstructionDecoder.DecodeInstructions(msg);

            Assert.AreEqual(5, ix.Count);
            Assert.AreEqual("Solend Program", ix[2].ProgramName);
            Assert.AreEqual("Refresh Reserve", ix[2].InstructionName);
            Assert.AreEqual("5VVLD7BQp8y3bTgyF5ezm1ResyMTR3PhYsT4iHFU8Sxz", ix[2].Values.GetValueOrDefault("Reserve").ToString());
            Assert.AreEqual("J83w4HKfqxwcq3BEMMkPFSppX3gqekLyLJBexebFVkix", ix[2].Values.GetValueOrDefault("Reserve Liquidity Pyth Oracle").ToString());
            Assert.AreEqual("AdtRGGhmqvom3Jemp5YNrxd9q9unX36BZk1pujkkXijL", ix[2].Values.GetValueOrDefault("Reserve Liquidity Switchboard Oracle").ToString());
            Assert.AreEqual(SysVars.ClockKey, ix[2].Values.GetValueOrDefault("Sysvar Clock").ToString());

            Assert.AreEqual("Solend Program", ix[3].ProgramName);
            Assert.AreEqual("Redeem Reserve Collateral", ix[3].InstructionName);
            Assert.AreEqual("95nixpmsxz3MQRfxotnWNGK7eh6QsKcf7FJZoVfCnACU", ix[3].Values.GetValueOrDefault("Source Collateral").ToString());
            Assert.AreEqual("FQY92w6qva4TakeTPf7ruAoKciobnmfk7NJG47se4PKu", ix[3].Values.GetValueOrDefault("Destination Liquidity").ToString());
            Assert.AreEqual("5VVLD7BQp8y3bTgyF5ezm1ResyMTR3PhYsT4iHFU8Sxz", ix[3].Values.GetValueOrDefault("Reserve").ToString());
            Assert.AreEqual("FzwZWRMc3GCqjSrcpVX3ueJc6UpcV6iWWb7ZMsTXE3Gf", ix[3].Values.GetValueOrDefault("Reserve Collateral Mint").ToString());
            Assert.AreEqual("furd3XUtjXZ2gRvSsoUts9A5m8cMJNqdsyR2Rt8vY9s", ix[3].Values.GetValueOrDefault("Reserve Liquidity Supply").ToString());
            Assert.AreEqual("GvjoVKNjBvQcFaSKUW1gTE7DxhSpjHbE69umVR5nPuQp", ix[3].Values.GetValueOrDefault("Lending Market").ToString());
            Assert.AreEqual("EhJ4fwaXUp7aiwvZThSUaGWCaBQAJe3AEaJJJVCn3UCK", ix[3].Values.GetValueOrDefault("Lending Market Authority").ToString());
            Assert.AreEqual("FcaY8uFsFpuDhRrDyP9JkGJNcQ8fLokkB99KegzVc5ak", ix[3].Values.GetValueOrDefault("User Transfer Authority").ToString());
            Assert.AreEqual("1000000000", ix[3].Values.GetValueOrDefault("Liquidity Amount").ToString());
            Assert.AreEqual(SysVars.ClockKey, ix[3].Values.GetValueOrDefault("Sysvar Clock").ToString());
            Assert.AreEqual(TokenProgram.ProgramIdKey, ix[3].Values.GetValueOrDefault("Token Program").ToString());
        }

        [TestMethod]
        public void DepositObligationCollateral()
        {
            Message msg = Message.Deserialize(Convert.FromBase64String(DepositObligationCollateralMessage));
            List<DecodedInstruction> ix =
                InstructionDecoder.DecodeInstructions(msg);

            Assert.AreEqual(2, ix.Count);
            Assert.AreEqual("Solend Program", ix[0].ProgramName);
            Assert.AreEqual("Refresh Reserve", ix[0].InstructionName);
            Assert.AreEqual("5VVLD7BQp8y3bTgyF5ezm1ResyMTR3PhYsT4iHFU8Sxz", ix[0].Values.GetValueOrDefault("Reserve").ToString());
            Assert.AreEqual("J83w4HKfqxwcq3BEMMkPFSppX3gqekLyLJBexebFVkix", ix[0].Values.GetValueOrDefault("Reserve Liquidity Pyth Oracle").ToString());
            Assert.AreEqual("AdtRGGhmqvom3Jemp5YNrxd9q9unX36BZk1pujkkXijL", ix[0].Values.GetValueOrDefault("Reserve Liquidity Switchboard Oracle").ToString());
            Assert.AreEqual(SysVars.ClockKey, ix[0].Values.GetValueOrDefault("Sysvar Clock").ToString());

            Assert.AreEqual("Solend Program", ix[1].ProgramName);
            Assert.AreEqual("Deposit Obligation Collateral", ix[1].InstructionName);
            Assert.AreEqual("95nixpmsxz3MQRfxotnWNGK7eh6QsKcf7FJZoVfCnACU", ix[1].Values.GetValueOrDefault("Source Collateral").ToString());
            Assert.AreEqual("J5KGpESS8Zq2MvK4rtL6wKbeMRYZzb6TEzn8qPsZFgGd", ix[1].Values.GetValueOrDefault("Destination Collateral").ToString());
            Assert.AreEqual("5VVLD7BQp8y3bTgyF5ezm1ResyMTR3PhYsT4iHFU8Sxz", ix[1].Values.GetValueOrDefault("Deposit Reserve").ToString());
            Assert.AreEqual("EiGycyytNhNtfsLZLFfCwspjBhhVem284qqP4AQ38897", ix[1].Values.GetValueOrDefault("Obligation").ToString());
            Assert.AreEqual("GvjoVKNjBvQcFaSKUW1gTE7DxhSpjHbE69umVR5nPuQp", ix[1].Values.GetValueOrDefault("Lending Market").ToString());
            Assert.AreEqual("FcaY8uFsFpuDhRrDyP9JkGJNcQ8fLokkB99KegzVc5ak", ix[1].Values.GetValueOrDefault("Obligation Owner").ToString());
            Assert.AreEqual("FcaY8uFsFpuDhRrDyP9JkGJNcQ8fLokkB99KegzVc5ak", ix[1].Values.GetValueOrDefault("User Transfer Authority").ToString());
            Assert.AreEqual("1000000000", ix[1].Values.GetValueOrDefault("Collateral Amount").ToString());
            Assert.AreEqual(SysVars.ClockKey, ix[1].Values.GetValueOrDefault("Sysvar Clock").ToString());
            Assert.AreEqual(TokenProgram.ProgramIdKey, ix[1].Values.GetValueOrDefault("Token Program").ToString());

        }

        [TestMethod]
        public void RefreshReservesObligationAndWithdrawCollateral()
        {
            Message msg = Message.Deserialize(Convert.FromBase64String(RefreshReservesObligationAndWithdrawCollateralMessage));
            List<DecodedInstruction> ix =
                InstructionDecoder.DecodeInstructions(msg);

            Assert.AreEqual(4, ix.Count);
            Assert.AreEqual("Solend Program", ix[0].ProgramName);
            Assert.AreEqual("Refresh Reserve", ix[0].InstructionName);
            Assert.AreEqual("5VVLD7BQp8y3bTgyF5ezm1ResyMTR3PhYsT4iHFU8Sxz", ix[0].Values.GetValueOrDefault("Reserve").ToString());
            Assert.AreEqual("J83w4HKfqxwcq3BEMMkPFSppX3gqekLyLJBexebFVkix", ix[0].Values.GetValueOrDefault("Reserve Liquidity Pyth Oracle").ToString());
            Assert.AreEqual("AdtRGGhmqvom3Jemp5YNrxd9q9unX36BZk1pujkkXijL", ix[0].Values.GetValueOrDefault("Reserve Liquidity Switchboard Oracle").ToString());
            Assert.AreEqual(SysVars.ClockKey, ix[0].Values.GetValueOrDefault("Sysvar Clock").ToString());

            Assert.AreEqual("Solend Program", ix[1].ProgramName);
            Assert.AreEqual("Refresh Reserve", ix[1].InstructionName);
            Assert.AreEqual("FNNkz4RCQezSSS71rW2tvqZH1LCkTzaiG7Nd1LeA5x5y", ix[1].Values.GetValueOrDefault("Reserve").ToString());
            Assert.AreEqual("5SSkXsEKQepHHAewytPVwdej4epN1nxgLVM84L4KXgy7", ix[1].Values.GetValueOrDefault("Reserve Liquidity Pyth Oracle").ToString());
            Assert.AreEqual("CZx29wKMUxaJDq6aLVQTdViPL754tTR64NAgQBUGxxHb", ix[1].Values.GetValueOrDefault("Reserve Liquidity Switchboard Oracle").ToString());
            Assert.AreEqual(SysVars.ClockKey, ix[1].Values.GetValueOrDefault("Sysvar Clock").ToString());

            Assert.AreEqual("Solend Program", ix[2].ProgramName);
            Assert.AreEqual("Refresh Obligation", ix[2].InstructionName);
            Assert.AreEqual("EiGycyytNhNtfsLZLFfCwspjBhhVem284qqP4AQ38897", ix[2].Values.GetValueOrDefault("Obligation").ToString());
            Assert.AreEqual("5VVLD7BQp8y3bTgyF5ezm1ResyMTR3PhYsT4iHFU8Sxz", ix[2].Values.GetValueOrDefault("Reserve 1").ToString());
            Assert.AreEqual("FNNkz4RCQezSSS71rW2tvqZH1LCkTzaiG7Nd1LeA5x5y", ix[2].Values.GetValueOrDefault("Reserve 2").ToString());
            Assert.AreEqual(SysVars.ClockKey, ix[2].Values.GetValueOrDefault("Sysvar Clock").ToString());

            Assert.AreEqual("Solend Program", ix[3].ProgramName);
            Assert.AreEqual("Withdraw Obligation Collateral", ix[3].InstructionName);
            Assert.AreEqual("J5KGpESS8Zq2MvK4rtL6wKbeMRYZzb6TEzn8qPsZFgGd", ix[3].Values.GetValueOrDefault("Source Collateral").ToString());
            Assert.AreEqual("95nixpmsxz3MQRfxotnWNGK7eh6QsKcf7FJZoVfCnACU", ix[3].Values.GetValueOrDefault("Destination Collateral").ToString());
            Assert.AreEqual("5VVLD7BQp8y3bTgyF5ezm1ResyMTR3PhYsT4iHFU8Sxz", ix[3].Values.GetValueOrDefault("Withdraw Reserve").ToString());
            Assert.AreEqual("EiGycyytNhNtfsLZLFfCwspjBhhVem284qqP4AQ38897", ix[3].Values.GetValueOrDefault("Obligation").ToString());
            Assert.AreEqual("GvjoVKNjBvQcFaSKUW1gTE7DxhSpjHbE69umVR5nPuQp", ix[3].Values.GetValueOrDefault("Lending Market").ToString());
            Assert.AreEqual("EhJ4fwaXUp7aiwvZThSUaGWCaBQAJe3AEaJJJVCn3UCK", ix[3].Values.GetValueOrDefault("Lending Market Authority").ToString());
            Assert.AreEqual("FcaY8uFsFpuDhRrDyP9JkGJNcQ8fLokkB99KegzVc5ak", ix[3].Values.GetValueOrDefault("Obligation Owner").ToString());
            Assert.AreEqual("1000000000", ix[3].Values.GetValueOrDefault("Collateral Amount").ToString());
            Assert.AreEqual(SysVars.ClockKey, ix[3].Values.GetValueOrDefault("Sysvar Clock").ToString());
            Assert.AreEqual(TokenProgram.ProgramIdKey, ix[3].Values.GetValueOrDefault("Token Program").ToString());
        }

        [TestMethod]
        public void RefreshReservesObligationAndBorrowObligationLiquidity()
        {
            Message msg = Message.Deserialize(Convert.FromBase64String(RefreshReservesObligationAndBorrowObligationLiquidityMessage));
            List<DecodedInstruction> ix =
                InstructionDecoder.DecodeInstructions(msg);

            Assert.AreEqual(6, ix.Count);
            Assert.AreEqual("Solend Program", ix[1].ProgramName);
            Assert.AreEqual("Refresh Reserve", ix[1].InstructionName);
            Assert.AreEqual("5VVLD7BQp8y3bTgyF5ezm1ResyMTR3PhYsT4iHFU8Sxz", ix[1].Values.GetValueOrDefault("Reserve").ToString());
            Assert.AreEqual("J83w4HKfqxwcq3BEMMkPFSppX3gqekLyLJBexebFVkix", ix[1].Values.GetValueOrDefault("Reserve Liquidity Pyth Oracle").ToString());
            Assert.AreEqual("AdtRGGhmqvom3Jemp5YNrxd9q9unX36BZk1pujkkXijL", ix[1].Values.GetValueOrDefault("Reserve Liquidity Switchboard Oracle").ToString());
            Assert.AreEqual(SysVars.ClockKey, ix[1].Values.GetValueOrDefault("Sysvar Clock").ToString());

            Assert.AreEqual("Solend Program", ix[2].ProgramName);
            Assert.AreEqual("Refresh Reserve", ix[2].InstructionName);
            Assert.AreEqual("FNNkz4RCQezSSS71rW2tvqZH1LCkTzaiG7Nd1LeA5x5y", ix[2].Values.GetValueOrDefault("Reserve").ToString());
            Assert.AreEqual("5SSkXsEKQepHHAewytPVwdej4epN1nxgLVM84L4KXgy7", ix[2].Values.GetValueOrDefault("Reserve Liquidity Pyth Oracle").ToString());
            Assert.AreEqual("CZx29wKMUxaJDq6aLVQTdViPL754tTR64NAgQBUGxxHb", ix[2].Values.GetValueOrDefault("Reserve Liquidity Switchboard Oracle").ToString());
            Assert.AreEqual(SysVars.ClockKey, ix[2].Values.GetValueOrDefault("Sysvar Clock").ToString());

            Assert.AreEqual("Solend Program", ix[3].ProgramName);
            Assert.AreEqual("Refresh Obligation", ix[3].InstructionName);
            Assert.AreEqual("EiGycyytNhNtfsLZLFfCwspjBhhVem284qqP4AQ38897", ix[3].Values.GetValueOrDefault("Obligation").ToString());
            Assert.AreEqual("5VVLD7BQp8y3bTgyF5ezm1ResyMTR3PhYsT4iHFU8Sxz", ix[3].Values.GetValueOrDefault("Reserve 1").ToString());
            Assert.AreEqual("FNNkz4RCQezSSS71rW2tvqZH1LCkTzaiG7Nd1LeA5x5y", ix[3].Values.GetValueOrDefault("Reserve 2").ToString());
            Assert.AreEqual(SysVars.ClockKey, ix[3].Values.GetValueOrDefault("Sysvar Clock").ToString());

            Assert.AreEqual("Solend Program", ix[4].ProgramName);
            Assert.AreEqual("Borrow Obligation Liquidity", ix[4].InstructionName);
            Assert.AreEqual("furd3XUtjXZ2gRvSsoUts9A5m8cMJNqdsyR2Rt8vY9s", ix[4].Values.GetValueOrDefault("Source Liquidity").ToString());
            Assert.AreEqual("FQY92w6qva4TakeTPf7ruAoKciobnmfk7NJG47se4PKu", ix[4].Values.GetValueOrDefault("Destination Liquidity").ToString());
            Assert.AreEqual("5VVLD7BQp8y3bTgyF5ezm1ResyMTR3PhYsT4iHFU8Sxz", ix[4].Values.GetValueOrDefault("Borrow Reserve").ToString());
            Assert.AreEqual("5kFqzU2k1tEXtoeNayk1TVxLycoAH5k8WsaGnBnanYJH", ix[4].Values.GetValueOrDefault("Borrow Reserve Liquidity Fee Receiver").ToString());
            Assert.AreEqual("EiGycyytNhNtfsLZLFfCwspjBhhVem284qqP4AQ38897", ix[4].Values.GetValueOrDefault("Obligation").ToString());
            Assert.AreEqual("GvjoVKNjBvQcFaSKUW1gTE7DxhSpjHbE69umVR5nPuQp", ix[4].Values.GetValueOrDefault("Lending Market").ToString());
            Assert.AreEqual("EhJ4fwaXUp7aiwvZThSUaGWCaBQAJe3AEaJJJVCn3UCK", ix[4].Values.GetValueOrDefault("Lending Market Authority").ToString());
            Assert.AreEqual("FcaY8uFsFpuDhRrDyP9JkGJNcQ8fLokkB99KegzVc5ak", ix[4].Values.GetValueOrDefault("Obligation Owner").ToString());
            Assert.AreEqual("1000000000", ix[4].Values.GetValueOrDefault("Liquidity Amount").ToString());
            Assert.AreEqual(SysVars.ClockKey, ix[4].Values.GetValueOrDefault("Sysvar Clock").ToString());
            Assert.AreEqual(TokenProgram.ProgramIdKey, ix[4].Values.GetValueOrDefault("Token Program").ToString());
        }

        [TestMethod]
        public void RefreshReservesObligationAndRepayObligationLiquidity()
        {
            Message msg = Message.Deserialize(Convert.FromBase64String(RefreshReservesObligationAndRepayObligationLiquidityMessage));
            List<DecodedInstruction> ix =
                InstructionDecoder.DecodeInstructions(msg);

            Assert.AreEqual(8, ix.Count);
            Assert.AreEqual("Solend Program", ix[2].ProgramName);
            Assert.AreEqual("Refresh Reserve", ix[2].InstructionName);
            Assert.AreEqual("5VVLD7BQp8y3bTgyF5ezm1ResyMTR3PhYsT4iHFU8Sxz", ix[2].Values.GetValueOrDefault("Reserve").ToString());
            Assert.AreEqual("J83w4HKfqxwcq3BEMMkPFSppX3gqekLyLJBexebFVkix", ix[2].Values.GetValueOrDefault("Reserve Liquidity Pyth Oracle").ToString());
            Assert.AreEqual("AdtRGGhmqvom3Jemp5YNrxd9q9unX36BZk1pujkkXijL", ix[2].Values.GetValueOrDefault("Reserve Liquidity Switchboard Oracle").ToString());
            Assert.AreEqual(SysVars.ClockKey, ix[2].Values.GetValueOrDefault("Sysvar Clock").ToString());

            Assert.AreEqual("Solend Program", ix[3].ProgramName);
            Assert.AreEqual("Refresh Reserve", ix[3].InstructionName);
            Assert.AreEqual("FNNkz4RCQezSSS71rW2tvqZH1LCkTzaiG7Nd1LeA5x5y", ix[3].Values.GetValueOrDefault("Reserve").ToString());
            Assert.AreEqual("5SSkXsEKQepHHAewytPVwdej4epN1nxgLVM84L4KXgy7", ix[3].Values.GetValueOrDefault("Reserve Liquidity Pyth Oracle").ToString());
            Assert.AreEqual("CZx29wKMUxaJDq6aLVQTdViPL754tTR64NAgQBUGxxHb", ix[3].Values.GetValueOrDefault("Reserve Liquidity Switchboard Oracle").ToString());
            Assert.AreEqual(SysVars.ClockKey, ix[3].Values.GetValueOrDefault("Sysvar Clock").ToString());

            Assert.AreEqual("Solend Program", ix[5].ProgramName);
            Assert.AreEqual("Refresh Obligation", ix[5].InstructionName);
            Assert.AreEqual("EiGycyytNhNtfsLZLFfCwspjBhhVem284qqP4AQ38897", ix[5].Values.GetValueOrDefault("Obligation").ToString());
            Assert.AreEqual("5VVLD7BQp8y3bTgyF5ezm1ResyMTR3PhYsT4iHFU8Sxz", ix[5].Values.GetValueOrDefault("Reserve 1").ToString());
            Assert.AreEqual("FNNkz4RCQezSSS71rW2tvqZH1LCkTzaiG7Nd1LeA5x5y", ix[5].Values.GetValueOrDefault("Reserve 2").ToString());
            Assert.AreEqual(SysVars.ClockKey, ix[5].Values.GetValueOrDefault("Sysvar Clock").ToString());

            Assert.AreEqual("Solend Program", ix[6].ProgramName);
            Assert.AreEqual("Repay Obligation Liquidity", ix[6].InstructionName);
            Assert.AreEqual("FQY92w6qva4TakeTPf7ruAoKciobnmfk7NJG47se4PKu", ix[6].Values.GetValueOrDefault("Source Liquidity").ToString());
            Assert.AreEqual("furd3XUtjXZ2gRvSsoUts9A5m8cMJNqdsyR2Rt8vY9s", ix[6].Values.GetValueOrDefault("Destination Liquidity").ToString());
            Assert.AreEqual("5VVLD7BQp8y3bTgyF5ezm1ResyMTR3PhYsT4iHFU8Sxz", ix[6].Values.GetValueOrDefault("Repay Reserve").ToString());
            Assert.AreEqual("EiGycyytNhNtfsLZLFfCwspjBhhVem284qqP4AQ38897", ix[6].Values.GetValueOrDefault("Obligation").ToString());
            Assert.AreEqual("GvjoVKNjBvQcFaSKUW1gTE7DxhSpjHbE69umVR5nPuQp", ix[6].Values.GetValueOrDefault("Lending Market").ToString());
            Assert.AreEqual("FcaY8uFsFpuDhRrDyP9JkGJNcQ8fLokkB99KegzVc5ak", ix[6].Values.GetValueOrDefault("User Transfer Authority").ToString());
            Assert.AreEqual("1000000000", ix[6].Values.GetValueOrDefault("Liquidity Amount").ToString());
            Assert.AreEqual(SysVars.ClockKey, ix[6].Values.GetValueOrDefault("Sysvar Clock").ToString());
            Assert.AreEqual(TokenProgram.ProgramIdKey, ix[6].Values.GetValueOrDefault("Token Program").ToString());
        }

        [TestMethod]
        public void RefreshReservesObligationAndWithdrawAndRedeem()
        {
            Message msg = Message.Deserialize(Convert.FromBase64String(RefreshReservesObligationAndWithdrawAndRedeemMessage));
            List<DecodedInstruction> ix =
                InstructionDecoder.DecodeInstructions(msg);

            Assert.AreEqual(7, ix.Count);
            Assert.AreEqual("Solend Program", ix[1].ProgramName);
            Assert.AreEqual("Refresh Reserve", ix[1].InstructionName);
            Assert.AreEqual("5VVLD7BQp8y3bTgyF5ezm1ResyMTR3PhYsT4iHFU8Sxz", ix[1].Values.GetValueOrDefault("Reserve").ToString());
            Assert.AreEqual("J83w4HKfqxwcq3BEMMkPFSppX3gqekLyLJBexebFVkix", ix[1].Values.GetValueOrDefault("Reserve Liquidity Pyth Oracle").ToString());
            Assert.AreEqual("AdtRGGhmqvom3Jemp5YNrxd9q9unX36BZk1pujkkXijL", ix[1].Values.GetValueOrDefault("Reserve Liquidity Switchboard Oracle").ToString());
            Assert.AreEqual(SysVars.ClockKey, ix[1].Values.GetValueOrDefault("Sysvar Clock").ToString());

            Assert.AreEqual("Solend Program", ix[2].ProgramName);
            Assert.AreEqual("Refresh Reserve", ix[2].InstructionName);
            Assert.AreEqual("FNNkz4RCQezSSS71rW2tvqZH1LCkTzaiG7Nd1LeA5x5y", ix[2].Values.GetValueOrDefault("Reserve").ToString());
            Assert.AreEqual("5SSkXsEKQepHHAewytPVwdej4epN1nxgLVM84L4KXgy7", ix[2].Values.GetValueOrDefault("Reserve Liquidity Pyth Oracle").ToString());
            Assert.AreEqual("CZx29wKMUxaJDq6aLVQTdViPL754tTR64NAgQBUGxxHb", ix[2].Values.GetValueOrDefault("Reserve Liquidity Switchboard Oracle").ToString());
            Assert.AreEqual(SysVars.ClockKey, ix[2].Values.GetValueOrDefault("Sysvar Clock").ToString());

            Assert.AreEqual("Solend Program", ix[4].ProgramName);
            Assert.AreEqual("Refresh Obligation", ix[4].InstructionName);
            Assert.AreEqual("EiGycyytNhNtfsLZLFfCwspjBhhVem284qqP4AQ38897", ix[4].Values.GetValueOrDefault("Obligation").ToString());
            Assert.AreEqual("5VVLD7BQp8y3bTgyF5ezm1ResyMTR3PhYsT4iHFU8Sxz", ix[4].Values.GetValueOrDefault("Reserve 1").ToString());
            Assert.AreEqual("FNNkz4RCQezSSS71rW2tvqZH1LCkTzaiG7Nd1LeA5x5y", ix[4].Values.GetValueOrDefault("Reserve 2").ToString());
            Assert.AreEqual(SysVars.ClockKey, ix[4].Values.GetValueOrDefault("Sysvar Clock").ToString());

            Assert.AreEqual("Solend Program", ix[5].ProgramName);
            Assert.AreEqual("Withdraw Obligation Collateral And Redeem Reserve Collateral", ix[5].InstructionName);
            Assert.AreEqual("J5KGpESS8Zq2MvK4rtL6wKbeMRYZzb6TEzn8qPsZFgGd", ix[5].Values.GetValueOrDefault("Source Collateral").ToString());
            Assert.AreEqual("95nixpmsxz3MQRfxotnWNGK7eh6QsKcf7FJZoVfCnACU", ix[5].Values.GetValueOrDefault("Destination Collateral").ToString());
            Assert.AreEqual("5VVLD7BQp8y3bTgyF5ezm1ResyMTR3PhYsT4iHFU8Sxz", ix[5].Values.GetValueOrDefault("Withdraw Reserve").ToString());
            Assert.AreEqual("EiGycyytNhNtfsLZLFfCwspjBhhVem284qqP4AQ38897", ix[5].Values.GetValueOrDefault("Obligation").ToString());
            Assert.AreEqual("GvjoVKNjBvQcFaSKUW1gTE7DxhSpjHbE69umVR5nPuQp", ix[5].Values.GetValueOrDefault("Lending Market").ToString());
            Assert.AreEqual("EhJ4fwaXUp7aiwvZThSUaGWCaBQAJe3AEaJJJVCn3UCK", ix[5].Values.GetValueOrDefault("Lending Market Authority").ToString());
            Assert.AreEqual("FQY92w6qva4TakeTPf7ruAoKciobnmfk7NJG47se4PKu", ix[5].Values.GetValueOrDefault("Destination Liquidity").ToString());
            Assert.AreEqual("FzwZWRMc3GCqjSrcpVX3ueJc6UpcV6iWWb7ZMsTXE3Gf", ix[5].Values.GetValueOrDefault("Reserve Collateral Mint").ToString());
            Assert.AreEqual("furd3XUtjXZ2gRvSsoUts9A5m8cMJNqdsyR2Rt8vY9s", ix[5].Values.GetValueOrDefault("Reserve Liquidity Supply").ToString());
            Assert.AreEqual("FcaY8uFsFpuDhRrDyP9JkGJNcQ8fLokkB99KegzVc5ak", ix[5].Values.GetValueOrDefault("Obligation Owner").ToString());
            Assert.AreEqual("FcaY8uFsFpuDhRrDyP9JkGJNcQ8fLokkB99KegzVc5ak", ix[5].Values.GetValueOrDefault("User Transfer Authority").ToString());
            Assert.AreEqual("1000000000", ix[5].Values.GetValueOrDefault("Liquidity Amount").ToString());
            Assert.AreEqual(SysVars.ClockKey, ix[5].Values.GetValueOrDefault("Sysvar Clock").ToString());
            Assert.AreEqual(TokenProgram.ProgramIdKey, ix[5].Values.GetValueOrDefault("Token Program").ToString());
        }
    }
}
