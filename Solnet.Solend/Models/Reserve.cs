using Solnet.Programs.Utilities;
using Solnet.Wallet;
using System;
using System.Numerics;

namespace Solnet.Solend.Models
{
    /// <summary>
    /// Represents a reserve in Solend.
    /// </summary>
    public class Reserve
    {
        /// <summary>
        /// The layout of the <see cref="Reserve"/> structure.
        /// </summary>
        public static class Layout
        {
            /// <summary>
            /// The length of the <see cref="Reserve"/> structure.
            /// </summary>
            public const int Length = 619;

            /// <summary>
            /// The offset at which the version value begins.
            /// </summary>
            public const int VersionOffset = 0;

            /// <summary>
            /// The offset at which the last update value begins.
            /// </summary>
            public const int LastUpdateOffset = 1;

            /// <summary>
            /// The offset at which the lending market value begins.
            /// </summary>
            public const int LendingMarketOffset = 10;

            /// <summary>
            /// The offset at which the liquidity value begins.
            /// </summary>
            public const int LiquidityOffset = 42;

            /// <summary>
            /// The offset at which the collateral value begins.
            /// </summary>
            public const int CollateralOffset = 227;

            /// <summary>
            /// The offset at which the config value begins.
            /// </summary>
            public const int ConfigOffset = 299;
        }

        /// <summary>
        /// Version of the struct
        /// </summary>
        public byte Version;

        /// <summary>
        /// Last slot when supply and rates updated
        /// </summary>
        public LastUpdate LastUpdate;

        /// <summary>
        /// Lending market public key
        /// </summary>
        public PublicKey LendingMarket;

        /// <summary>
        /// Reserve liquidity
        /// </summary>
        public ReserveLiquidity Liquidity;

        /// <summary>
        /// Reserve collateral
        /// </summary>
        public ReserveCollateral Collateral;

        /// <summary>
        /// Reserve configuration values
        /// </summary>
        public ReserveConfig Config;

        /// <summary>
        /// Initialize the <see cref="Reserve"/> with the given data.
        /// </summary>
        /// <param name="data">The byte array.</param>
        public Reserve(ReadOnlySpan<byte> data)
        {
            if (data.Length != Layout.Length)
                throw new ArgumentException($"{nameof(data)} has wrong size. Expected {Layout.Length} bytes, actual {data.Length} bytes.");
            Version = data.GetU8(Layout.VersionOffset);
            LastUpdate = new(data.Slice(Layout.LastUpdateOffset, LastUpdate.Layout.Length));
            LendingMarket = data.GetPubKey(Layout.LendingMarketOffset);
            Liquidity = new(data.Slice(Layout.LiquidityOffset, ReserveLiquidity.Layout.Length));
            Collateral = new(data.Slice(Layout.CollateralOffset, ReserveCollateral.Layout.Length));
            Config = new(data.Slice(Layout.ConfigOffset, ReserveConfig.Layout.Length));
        }

        /// <summary>
        /// Deserialize the given byte array into the <see cref="Reserve"/> structure.
        /// </summary>
        /// <param name="data">The byte array.</param>
        /// <returns>The <see cref="Reserve"/> instance.</returns>
        public static Reserve Deserialize(byte[] data) => new(data.AsSpan());

        /// <summary>
        /// Gets the total token supply.
        /// </summary>
        /// <returns>The total token supply.</returns>
        public decimal GetTotalSupply()
                => Collateral.TotalSupply / (decimal)Math.Pow(10, Liquidity.Decimals);

        /// <summary>
        /// Gets the available token supply.
        /// </summary>
        /// <returns>The available token supply.</returns>
        public decimal GetAvailableAmount()
            => Liquidity.AvailableAmount / (decimal)Math.Pow(10, Liquidity.Decimals);

        /// <summary>
        /// Gets the total token supply.
        /// </summary>
        /// <returns>The total token supply.</returns>
        public decimal GetTotalSupplyUsd()
            => GetTotalSupply() * GetMarketPrice();

        /// <summary>
        /// Gets the total borrow amount in the corresponding token.
        /// </summary>
        /// <returns>The total borrow amount in the corresponding token.</returns>
        public decimal GetTotalBorrow()
            => (decimal)(Liquidity.BorrowedAmountWads / Constants.Wad)
                / (decimal)Math.Pow(10, Liquidity.Decimals);

        /// <summary>
        /// Gets the market price of the underlying liquidity token.
        /// </summary>
        /// <returns>The market price in USD.</returns>
        public decimal GetMarketPrice()
            => (decimal)Liquidity.MarketPrice / (decimal)Constants.Wad;

        /// <summary>
        /// Gets total borrow amount in USD.
        /// </summary>
        /// <returns>The total borrow amount in USD.</returns>
        public decimal GetTotalBorrowUsd()
            => GetTotalBorrow() * GetMarketPrice();

        /// <summary>
        /// Calculates the supply APR for this reserve.
        /// </summary>
        /// <returns>The supply APR.</returns>
        public decimal CalculateSupplyApr()
        {
            decimal currentUtilization = CalculateUtilizationRatio();

            decimal borrowApr = CalculateBorrowApr();

            return currentUtilization * borrowApr;
        }

        /// <summary>
        /// Calculates the supply APY for this reserve.
        /// </summary>
        /// <returns>The supply APY.</returns>
        public decimal CalculateSupplyApy()
        {
            double apr = (double)CalculateSupplyApr();

            return CalculateApy(apr);
        }

        /// <summary>
        /// Calculates the borrow APR for this reserve.
        /// </summary>
        /// <returns>The borrow APR.</returns>
        public decimal CalculateBorrowApr()
        {
            decimal currentUtilization = CalculateUtilizationRatio();
            decimal optimalUtilization = Config.OptimalUtilizationRate / 100m;

            decimal borrowApr;

            if (optimalUtilization == 1.0m || currentUtilization < optimalUtilization)
            {
                decimal normalizedFactor = currentUtilization / optimalUtilization;
                decimal optimalBorrow = Config.OptimalBorrowRate / 100m;
                decimal minBorrow = Config.MinBorrowRate / 100m;

                borrowApr = normalizedFactor * (optimalBorrow - minBorrow) + minBorrow;
            }
            else
            {
                decimal normalizedFactor =
                    (currentUtilization - optimalUtilization) / (1 - optimalUtilization);
                decimal optimalBorrow = Config.OptimalBorrowRate / 100m;
                decimal maxBorrow = Config.MaxBorrowRate / 100m;

                borrowApr = normalizedFactor * (maxBorrow - optimalBorrow) + optimalBorrow;
            }

            return borrowApr;
        }

        /// <summary>
        /// Calculates the borrow APY for this reserve.
        /// </summary>
        /// <returns>The borrow APY.</returns>
        public decimal CalculateBorrowApy()
        {
            double apr = (double)CalculateBorrowApr();

            return CalculateApy(apr);
        }

        /// <summary>
        /// Calculates APY for the given APR based on the number of slots per year.
        /// </summary>
        /// <param name="apr">The APR value.</param>
        /// <returns>The APY value.</returns>
        private decimal CalculateApy(double apr)
        {
            return (decimal)Math.Pow(1d + (apr / Constants.SlotsPerYear), Constants.SlotsPerYear) - 1m;
        }

        /// <summary>
        /// Calculates the utilization ratio of this reserve.
        /// </summary>
        /// <returns>The utilization ratio.</returns>
        public decimal CalculateUtilizationRatio()
        {
            BigInteger totalBorrowsWads = Liquidity.BorrowedAmountWads / Constants.Wad;

            decimal totalBorrowsWadsAsDecimal = (decimal)totalBorrowsWads;

            return totalBorrowsWadsAsDecimal / (Liquidity.AvailableAmount + totalBorrowsWadsAsDecimal);
        }
    }
}
