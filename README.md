# GirShop

Checkout 3.0 integration in F# + Giraffe

Install .NET Core 3.1 [https://dotnet.microsoft.com/download/dotnet-core/3.1](https://dotnet.microsoft.com/download/dotnet-core/3.1)

### Add following variables to your environment variables:

- `apiPublicUrl` - URL where the server is running

- `swedenClientId` - ClientID for shop authentication in Swedish Market

- `swedenClientSecret` - ClientSecret for shop authentication Swedish Market

- `checkoutBackendApiUrl` - Checkout 3.0 Backend API url

- `checkoutFrontendBundleUrl` - Checkout 3.0 Frontend JS bundle url

### Available markets:

In order to test different markets set up following `clientId`/`clientSecret` values based on market. Market will be available in the `/settings/` page when credentials are set up.

```json
Sweden:
    "swedenClientId"
    "swedenClientSecret"
Finland:
    "finlandClientId"
    "finlandClientSecret"
Norway:
    "norwayClientId"
    "norwayClientSecret"
Denmark:
    "denmarkClientId"
    "denmarkClientSecret"
Slovakia:
    "slovakiaClientId"
    "slovakiaClientSecret"
Czechia:
    "czechiaClientId"
    "czechiaClientSecret"
Poland:
    "polandClientId"
    "polandClientSecret"
Latvia:
    "latviaClientId"
    "latviaClientSecret"
Estonia:
    "estoniaClientId"
    "estoniaClientSecret"
```

Please refer to articles [How to get started](https://docs.avarda.com/checkout-3/how-to-get-started/) and
[Embed Checkout](https://docs.avarda.com/checkout-3/embed-checkout/) for more info.

Run following commands:

```bash
dotnet tool restore

dotnet paket install

dotnet fake build
```

Open [http://localhost:5000](localhost:5000) or [https://localhost:5001](localhost:5001)
