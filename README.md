# GirShop

Checkout 3.0 and PayFrame integration in F# + Giraffe

- Install .NET 8 [https://dotnet.microsoft.com/en-us/download/dotnet/8.0](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)

## Testing and Develomepnt

**_NEW_** -> With introduction of `appsettings` in the source project, the default testing setup is handled via environment-specific configuration files that already have most of the needed setup.

### Checkout3

Previously needed values as environment variables for testing:

- `apiPublicUrl` - URL where the server is running
- `checkoutBackendApiUrl` - Checkout 3.0 Backend API url
- `checkoutFrontendBundleUrl` - Checkout 3.0 Frontend JS bundle url
- `paymentWidgetBundleUrl` - Payment Widget bundle url
- `partnerShippingBundleUrl` - Partner Shipping bundle url

are now stored in `appsettings`:

```json
  "CheckoutOptions": {
    "PublicUrl": "",
    "BackendApiUrl": "",
    "FrontendBundleUrl": ""
  },
  "PaymentWidgetOptions" : {
    "FrontendBundleUrl": ""
  },
  "PartnerShippingOptions": {
    "FrontendBundleUrl": ""
  },
```

now only `CheckoutOptions:PublicUrl` [default [http://localhost:5000](localhost:5000) ] may be changed for development purposes in `appsettings.Development.json` file matching the port setup in `launchSettings.json` (if used, just for consistency, more info in section [Local run](#local-run)).

#### Markets

In order to test different markets specific for Checkout set up values for `clientId`and `clientSecret` based on market in the `appsettings.json` under `CheckoutOptions` (see example below). Market will be available in the `/settings/` page when credentials are set up properly.

Example:
```json
  "CheckoutOptions": {
    "Sweden": {
      "ClientId": "***",
      "ClientSecret": "***"
    }
  }
```

Available markets:
```json
    | Sweden
    | Finland
    | Norway
    | Denmark
    | Germany
    | Austria
    | Slovakia
    | Czechia
    | Poland
    | Latvia
    | Estonia
    | International *
```

\* - International market requires extra setup by Avarda, please contact Avarda representative or support.

### PayFrame

For testing PayFrame add the `SiteKey` [ required unique identifier provided by Avarda ] parameter to `appsettings.json`. There is also an posibility to change optional property `Language` [ ISO 639-1 language code - default `en` ]

Example:
```json
  "PayFrame": {
      "SiteKey": "***",
      "Language" "sv"
  }
```

Alternatively you can pass `siteKey`, and `language` as query parameters on the `pay-frame` page like this:
```json
  https://localhost:5000/pay-frame?siteKey=cc2898b0-362c-445a-b777-80408e74b9a8&language=sv
```

<hr>

Please refer to articles [Getting started](https://docs.avarda.com/checkout-3/getting-started/) and
[Embed Checkout](https://docs.avarda.com/checkout-3/embed-checkout/) for more info.

<hr>

### Keyvault

**_NEW_** -> For testing and local developlment Azure Keyvault support can be added by filling `VaultName` of your keyvault in `appsettings.json`.

### Local run

For easier development a `launchSettings.json` file can be added to `src/Gir/Properties/launchSettings.json`:

```json
{
  "profiles": {
    "Gir": {
      "commandName": "Project",
      "dotnetRunMessages": true, // - optional 
      "launchBrowser": true, // - optional | only for IDE tooling
      "applicationUrl": "https://localhost:7148;http://localhost:5168", // optional | - custom local ports https;http 
      "environmentVariables": {} // - optional | if any variables are needed
    }
  }
}
```

**Note:** Also for safer handling of sercet values add all of them to the standard `secrets.json` that is resolved last in the configuration builder rather than exposing them in `appsettings` and accidentally pushing them to Git. More info [here](https://learn.microsoft.com/en-us/aspnet/core/security/app-secrets?view=aspnetcore-10.0&tabs=windows).

Example: 
```json
  {
    "CheckoutOptions": {
      "Sweden": {
        "ClientId": "***",
        "ClientSecret": "***"
      }
    },
    "PayFrameOptions": {
      "SiteKey": "***"
    },
    "VaultName": "***"
  }
```

After completing the whole setup including `appsettings` run following commands from `src/Gir` path:

```bash
dotnet tool restore

dotnet paket install

dotnet run
```

or run the Gir application setup from your IDE.

Open [http://localhost:5000](localhost:5000).

<hr>

## Documentation:

### Checkout 3

[Avarda Checkout 3 documentation](https://docs.avarda.com/checkout-3/overview/)

### PayFrame

[PayFrame documentation](https://docs.avarda.com/pay-frame/overview/)

### Payment Widget

[Payment Widget documentation](https://docs.avarda.com/checkout-3/payment-widget/)

### Partner Shipping Module

[Partner Shipping Module documentation](https://docs.avarda.com/checkout-3/shipping-broker/provider-specific-integration-guide/partner-shipping/)