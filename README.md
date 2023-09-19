# GirShop

Checkout 3.0 integration in F# + Giraffe

- Install .NET 6 [https://dotnet.microsoft.com/en-us/download/dotnet/6.0](https://dotnet.microsoft.com/en-us/download/dotnet/6.0)

### Add following variables to your environment variables:

- `apiPublicUrl` - URL where the server is running

- `swedenClientId` - ClientID for shop authentication in Swedish Market

- `swedenClientSecret` - ClientSecret for shop authentication Swedish Market

- `checkoutBackendApiUrl` - Checkout 3.0 Backend API url

- `checkoutFrontendBundleUrl` - Checkout 3.0 Frontend JS bundle url

- `paymentWidgetBundleUrl` - **_NEW_** Payment Widget bundle url (optional)

- `partnerShippingBundleUrl` - **_NEW_** Partner Shipping bundle url (optional)

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
Germany:
    "germanyClientId"
    "germanyClientSecret"
Austria:
    "austriaClientId"
    "austriaClientSecret"
International*:
    "internationalClientId"
    "internationalClientSecret"
```

\* - International market requires extra setup by Avarda, please contact Avarda representative or support.

<hr>

Please refer to articles [How to get started](https://docs.avarda.com/checkout-3/how-to-get-started/) and
[Embed Checkout](https://docs.avarda.com/checkout-3/embed-checkout/) for more info.

One way of adding the env variables is adding a "launchSettings.json" `src/Gir/Properties/launchSettings.json`:

```json
{
  "profiles": {
    "Gir": {
      "commandName": "Project",
      "environmentVariables": {
        "apiPublicUrl": "https://localhost:5001",
        "swedenClientId": "<clientId>",
        "swedenClientSecret": "<clientSecret>",
        "checkoutBackendApiUrl": "<checkoutApiUrl>",
        "checkoutFrontendBundleUrl": "<checkoutBundleUrl>",
        "paymentWidgetBundleUrl": "<paymentWidgetUrl>"
      }
    }
  }
}
```

Run following commands:

```bash
dotnet tool restore

dotnet paket install

dotnet fake build
```

Open [http://localhost:5000](localhost:5000) or [https://localhost:5001](localhost:5001)

## Documentation:

### Checkout 3

[Avarda Checkout 3 Documentation](https://docs.avarda.com/checkout-3/checkout-3-0/)

#### Payment Widget

[Payment Widget documentation](https://docs.avarda.com/checkout-3/payment-widget/)

#### Partner Shipping Module

[Partner Shipping Module documentation](https://docs.avarda.com/checkout-3/overview/shipping-broker/provider-specific-integration-guide/partner-shipping/)
