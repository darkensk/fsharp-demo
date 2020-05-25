# GirShop

Checkout 3.0 integration in F# + Giraffe

Install .NET Core 3.1 [https://dotnet.microsoft.com/download/dotnet-core/3.1](https://dotnet.microsoft.com/download/dotnet-core/3.1)

Add following variables to your environment variables:

- `clientId` - ClientID for shop authentication

- `clientSecret` - ClientSecret for shop authentication

- `checkoutBackendApiUrl` - Checkout 3.0 Backend API url

- `checkoutFrontendBundleUrl` - Checkout 3.0 Frontend JS bundle url

Please refer to articles [How to get started](https://docs.avarda.com/checkout-3/how-to-get-started/) and
[Embed Checkout](https://docs.avarda.com/checkout-3/embed-checkout/) for more info.

Run following commands:

```bash
dotnet tool restore

dotnet paket install

dotnet fake build
```

Open [http://localhost:5000](localhost:5000) or [https://localhost:5001](localhost:5001)
