<p align="center">
    <img src="assets/icon.png" margin="auto" height="175"/>
</p>
<p align="center">
    <a href="https://github.com/bmresearch/Solnet.Solend/actions/workflows/dotnet.yml">
        <img src="https://github.com/bmresearch/Solnet.Solend/actions/workflows/dotnet.yml/badge.svg"
            alt="Build Status (master)" ></a>
    <a href="https://coveralls.io/github/bmresearch/Solnet.Solend?branch=master">
        <img src="https://coveralls.io/repos/github/bmresearch/Solnet.Solend/badge.svg?branch=master" 
            alt="Coverage Status" ></a>
<br/>
    <a href="">
        <img src="https://img.shields.io/github/license/bmresearch/Solnet.Solend?style=flat-square"
            alt="Code License"></a>
    <a href="https://twitter.com/intent/follow?screen_name=blockmountainio">
        <img src="https://img.shields.io/twitter/follow/blockmountainio?style=flat-square&logo=twitter"
            alt="Follow on Twitter"></a>
    <a href="https://discord.gg/YHMbpuS3Tx">
       <img alt="Discord" src="https://img.shields.io/discord/849407317761064961?style=flat-square"
            alt="Join the discussion!"></a>
</p>

# What is Solnet.Solend?

[Solnet](https://github.com/bmresearch/Solnet) is Solana's .NET integration library, a number of packages that implement features to interact with
Solana from .Net applications.

Solnet.Solend is a package within the same `Solnet.` namespace that implements a Client for [Solend](https://solend.fi/), this project is in a
separate repository so it is contained, as the goal for [Solnet](https://github.com/bmresearch/Solnet) was to be a core SDK.

## Features

- Decoding of Solend data structures
  - `LendingMarket`
  - `Obligation` including `ObligationCollateral` and `ObligationLiquidity`
  - `Reserve` including `ReserveCollateral` and `ReserveLiquidity`
- Calculation of APR, APYs, total supplied/borrowed value, account positions, etc
- `SolendProgram` instructions implemented
  - `InitializeObligation`
  - `RefreshReserve`
  - `RefreshObligation`
  - `DepositReserveLiquidity`
  - `RedeemReserveCollateral`
  - `BorrowObligationLiquidity`
  - `RepayObligationLiquidity`
  - `DepositObligationCollateral`
  - `WithdrawObligationCollateral`
  - `DepositReserveLiquidityAndObligationCollateral`
  - `WithdrawObligationCollateralAndRedeemReserveCollateral`

## Requirements
- net 6.0

## Dependencies
- Solnet.Programs v6.0.7
- Solnet.Wallet v6.0.7
- Solnet.Rpc v6.0.7

## Examples

The [Solnet.Solend.Examples](https://github.com/bmresearch/Solnet.Solend/tree/master/Solnet.Solend.Examples) project features some examples on how to use the [ISolendClient](https://github.com/bmresearch/Solnet.Solend/tree/master/Solnet.Solend/ISolendClient.cs), these examples include:
- Getting all lending markets
- Getting all reserves
- Getting all obligations for a given user and lending market
- Calculating Solend's TVL

As well as examples on how to use the `SolendProgram`
- Minting cTokens
- Redeeming cTokens
- Depositing and withdrawing liquidity
- Borrowing liquidity and repaying the borrowed liquidity

## Contribution

We encourage everyone to contribute, submit issues, PRs, discuss. Every kind of help is welcome.

## Contributors

* **Hugo** - *Maintainer* - [murlokito](https://github.com/murlokito)

See also the list of [contributors](https://github.com/bmresearch/Solnet.Solend/contributors) who participated in this project.

## License

This project is licensed under the MIT License - see the [LICENSE](https://github.com/bmresearch/Solnet.Solend/blob/master/LICENSE) file for details

