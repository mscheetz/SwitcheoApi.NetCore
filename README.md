# SwitcheoApi.NetCore  
.Net Core library for accessing the [Switcheo Exchange](https://switcheo.exchange) Api  

This library is available on NuGet for download: https://www.nuget.org/packages/SwitcheoApi.NetCore
```
PM> Install-Package SwitcheoApi.NetCore -Version 1.2.0
```

Switcheo is a Decentralized cryptocurrency exchange (DEX) for trading tokens built on the NEO blockchain (NEP-5)  
For more information: https://switcheo.network/  

###### Secure endpoints require a NEO wallet:  
https://neo.org/client#2  

###### Recommended wallets:  
[O3](https://o3.network/) (Linux/Mac/Windows/Android/iOS)  
[NEON](https://github.com/CityOfZion/neon-wallet) (Linux/Mac/Windows)  
[NEO Tracker](https://neotracker.io/wallet) (web)  
  
## Initialization:  
  
###### Non-secured endpoints only:  
```
// main-net  
var switcheoRepo = new SwitcheoApiClient();  
// test-net  
var switcheoRepo = new SwitcheoApiClient(true);
```  
  
###### Secure & non-secure endpoints:  
```
// main-net  
var switcheo = new SwitcheoApiClient(neoWIF);  
// test-net  
var switcheo = new SwitcheoApiClient(neoWIF, true);
```  
  
## Using an endpoint  
```
var pairs = await switcheo.GetPairs();
```
or  
```
var pairs = switcheo.GetPairs().Result;
```  

###### Non-secured endpoints:  
GetBalances() | GetBalancesAsync() - get contract balance of an address  
GetCandlesticks() | GetCandlesticksAsync() - get candlestick chart data  
GetContracts() | GetContractsAsync() - get hashes of contracts deployed by Switcheo  
GetLast24Hours() | GetLast24HoursAsync() - get last 24-hour data for all pairs  
GetLastPrice() | GetLastPriceAsync() - get last price of (a) symbol(s)  
GetOffers() | GetOffersAsync() - get best 70 offers on the order book  
GetOrderes() | GetOrderesAsync() - get orders for a given address  
GetPairs() | GetPairsAsync() - get all trading pairs  
GetServerTime() | GetServerTimeAsync() - get Switcheo server time  
GetTokens() | GetTokensAsync() - get all tokens supported on Switcheo  
GetTrades() | GetTradesAsync() - get trades executed for a pair  

  
###### Secure Endpoints:  
GetBalances() | GetBalancesAsync() - get contract balance of logged in wallet  
Deposit() | DepositAsync() - deposit tokens from wallet to Switcheo exchange  
CreateDeposit() | CreateDepositAsync() - create a deposit from wallet to Switcheo exchange, REQUIRES ExecuteDeposit!  
ExecuteDeposit() | ExecuteDepositAsync() - execute a deposit from wallet to Switcheo exchange, REQUIRES CreateDeposit!  
Withdrawal() | WithdrawalAsync() - withdrawal tokens from Switcheo exchange to wallet  
CreateWithdrawal() | CreateWithdrawalAsync() - create a withdrawal from Switcheo exchange to wallet, REQUIRES ExecuteWithdrawal()!  
ExecuteWithdrawal() | ExecuteWithdrawalAsync() - execute a withdrawal from Switcheo exchange to wallet, REQUIRES CreateWithdrawal()!  
PlaceOrder() | PlaceOrderAsync() - place an order  
PlaceNeoOrder() | PlaceNeoOrderAsync() - place an order using neo value to trade  
CreateNeoOrder() | CreateNeoOrderAsync() - create an order using neo value to trade, REQUIRES BroadcastOrder()!  
CreateTokenOrder() | CreateTokenOrderAsync() - create an order, REQUIRES BroadcastOrder()!  
BroadcastOrder() | BroadcastOrderAsync() - broadcast an order, REQUIRES Create..Order()!  
CancelOrder() | CancelOrderAsync() - cancel an order  
CreateCancellation() | CreateCancellationAsync() - create a cancellation, REQUIRES ExcecuteCancellation()!  
ExcecuteCancellation() | ExcecuteCancellationAsync() - execute a cancellation, REQUIRES ExcecuteCancellation()!  
GetWallet() - get loaded wallet    

NEO/SWTH:   
AHtB1D5UtMiTJbDTn5pfJdPit77de19oao  
ETH:  
0x3c8e741c0a2Cb4b8d5cBB1ead482CFDF87FDd66F  
BTC:  
1MGLPvTzxK9argeNRTHJ9EZ3WtGZV6nxit  
XLM:  
GA6JNJRSTBV54W3EGWDAWKPEGGD3QCXIGEHMQE2TUYXUKKTNKLYWEXVV  
