FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /src
COPY . .
RUN dotnet tool restore
RUN dotnet paket install
RUN dotnet restore ./src/Gir/
RUN dotnet build ./src/Gir/

FROM build AS publish
RUN dotnet publish ./src/Gir/ -o /app

FROM base AS final
ARG clientId
ARG clientSecret
ARG checkoutBackendApiUrl="https://avdonl-p-checkout.westeurope.cloudapp.azure.com"
ARG checkoutFrontendBundleUrl="https://avdonl0p0checkout0fe.blob.core.windows.net/frontend/static/js/main.js"
ENV clientId=${clientId}
ENV clientSecret=${clientSecret}
ENV checkoutBackendApiUrl=${checkoutBackendApiUrl}
ENV checkoutFrontendBundleUrl=${checkoutFrontendBundleUrl}
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Gir.dll"]