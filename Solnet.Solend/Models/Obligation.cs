using Solnet.Programs.Utilities;
using Solnet.Wallet;
using System;
using System.Collections.Generic;
using System.Numerics;

namespace Solnet.Solend.Models
{
    /// <summary>
    /// Represents an obligation in Solend.
    /// </summary>
    public class Obligation
    {
        /// <summary>
        /// The layout of the <see cref="Obligation"/> structure.
        /// </summary>
        public static class Layout
        {
            /// <summary>
            /// The length of the <see cref="Obligation"/> structure.
            /// </summary>
            public const int Length = 1300;

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
            public const int OwnerOffset = 42;

            /// <summary>
            /// The offset for the deposited value.
            /// </summary>
            public const int DepositedValueOffset = 74;

            /// <summary>
            /// The offset for the borrowed value.
            /// </summary>
            public const int BorrowedValueOffset = 90;

            /// <summary>
            /// The offset for the allowed borrow value.
            /// </summary>
            public const int AllowedBorrowValueOffset = 106;

            /// <summary>
            /// The offset for the unhealthy borrow value.
            /// </summary>
            public const int UnhealthyBorrowValueOffset = 122;

            /// <summary>
            /// The offset for the deposits length value.
            /// </summary>
            public const int DepositsLengthOffset = 202;

            /// <summary>
            /// The offset for the borrows length value.
            /// </summary>
            public const int BorrowsLengthOffset = 203;

            /// <summary>
            /// The offset for the data flat.
            /// </summary>
            public const int DataFlatOffset = 204;
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
        /// Owner authority which can borrow liquidity
        /// </summary>
        public PublicKey Owner;

        /// <summary>
        /// Deposited collateral for the obligation, unique by deposit reserve address
        /// </summary>
        public List<ObligationCollateral> Deposits;

        /// <summary>
        /// Borrowed liquidity for the obligation, unique by borrow reserve address
        /// </summary>
        public List<ObligationLiquidity> Borrows;

        /// <summary>
        /// Market value of deposits
        /// </summary>
        public BigInteger DepositedValue;

        /// <summary>
        /// Market value of borrows
        /// </summary>
        public BigInteger BorrowedValue;

        /// <summary>
        /// The maximum borrow value at the weighted average loan to value ratio
        /// </summary>
        public BigInteger AllowedBorrowValue;

        /// <summary>
        /// The dangerous borrow value at the weighted average liquidation threshold
        /// </summary>
        public BigInteger UnhealthyBorrowValue;

        /// <summary>
        /// Initialize the <see cref="Obligation"/> with the given data.
        /// </summary>
        /// <param name="data">The byte array.</param>
        public Obligation(ReadOnlySpan<byte> data)
        {
            if (data.Length != Layout.Length)
                throw new ArgumentException($"{nameof(data)} has wrong size. Expected {Layout.Length} bytes, actual {data.Length} bytes.");

            List<ObligationCollateral> deposits = new();
            List<ObligationLiquidity> borrows = new();
            int numDeposits = data.GetU8(Layout.DepositsLengthOffset);
            int numBorrows = data.GetU8(Layout.BorrowsLengthOffset);
            ReadOnlySpan<byte> dataFlatBytes = data[Layout.DataFlatOffset..];
            int offset = 0;

            for (int i = 0; i < numDeposits; i++)
            {
                ObligationCollateral obligationCollateral = new(dataFlatBytes.GetSpan(offset, ObligationCollateral.Layout.Length));
                deposits.Add(obligationCollateral);
                offset += ObligationCollateral.Layout.Length;
            }

            for (int i = 0; i < numBorrows; i++)
            {
                ObligationLiquidity obligationLiquidity = new(dataFlatBytes.GetSpan(offset, ObligationLiquidity.Layout.Length));
                borrows.Add(obligationLiquidity);
                offset += ObligationLiquidity.Layout.Length;
            }

            Version = data.GetU8(Layout.VersionOffset);
            LastUpdate = new(data.Slice(Layout.LastUpdateOffset, LastUpdate.Layout.Length));
            LendingMarket = data.GetPubKey(Layout.LendingMarketOffset);
            Owner = data.GetPubKey(Layout.OwnerOffset);
            DepositedValue = data.GetBigInt(Layout.DepositedValueOffset, 16, true);
            BorrowedValue = data.GetBigInt(Layout.BorrowedValueOffset, 16, true);
            AllowedBorrowValue = data.GetBigInt(Layout.AllowedBorrowValueOffset, 16, true);
            UnhealthyBorrowValue = data.GetBigInt(Layout.UnhealthyBorrowValueOffset, 16, true);
            Deposits = deposits;
            Borrows = borrows;
        }

        /// <summary>
        /// Deserialize the given byte array into the <see cref="Obligation"/> structure.
        /// </summary>
        /// <param name="data">The byte array.</param>
        /// <returns>The <see cref="Obligation"/> instance.</returns>
        public static Obligation Deserialize(byte[] data) => new(data.AsSpan());
    }
}
