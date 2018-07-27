# SwitcheoApi.NetCore  
.Net Core library for accessing the [Switcheo Exchange](https://switcheo.exchange) Api  
  

Switcheo is a Decentralized cryptocurrency exchange (DEX) for trading tokens built on the NEO blockchain (NEP-5)  
For more information: https://switcheo.network/  

Secure endpoints require a NEO wallet:  
https://neo.org/client#2  

Recommended wallets:  
[NEON](https://github.com/CityOfZion/neon-wallet) (Linux/Mac/Windows)  
[NEO Tracker](https://neotracker.io/wallet) (web)  
[O3](https://o3.network/) (Android/iOS)  

  
Initialization:  
  
Non-secured endpoints only:  
```
// main-net  
var switcheoRepo = new SwitcheoApiClient();  
// test-net  
var switcheoRepo = new SwitcheoApiClient(true);
```  
  
Secure & non-secure endpoints:  
```
// main-net  
var switcheoRepo = new SwitcheoApiClient(neoWalletAddress, walletPrivateKey);  
// test-net  
var switcheoRepo = new SwitcheoApiClient(neoWalletAddress, walletPrivateKey, true);
```  
  
Using an endpoint  
```
var pairs = await switcheoRepo.GetPairs();
```  

Non-secured endpoints:  

GetPairs() - get all trading pairs  
GetContracts() - get hashes of contracts deployed by Switcheo  
GetCandlesticks() - get candlestick chart data  
GetLast24Hours() - get last 24-hour data for all pairs  
GetLastPrice() - get last price of (a) symbol(s)  
GetOffers() - get best 70 offers on the order book  
GetTrades() - get trades executed for a pair  
GetBalances() - get contract balance of an address  
GetOrderes() - get orders for a given address  
  
Secure Endpoints:  
*<< coming soon >>*  
