# GirShop

Checkout3.0 integration in F# + Giraffe

Install .NET Core 3.1 [https://dotnet.microsoft.com/download/dotnet-core/3.1](https://dotnet.microsoft.com/download/dotnet-core/3.1)

Add `clientId` and `clientSecret` to `local.settings.json` (Use template from `template.settings.json`)

```bash
dotnet tool restore

dotnet paket install

dotnet fake build
```

Open [localhost:5001](localhost:5001)
