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
ARG apiPublicUrl
ARG swedenClientId
ARG swedenClientSecret
ARG finlandClientId
ARG finlandClientSecret
ARG checkoutBackendApiUrl="https://avdonl-p-checkout.westeurope.cloudapp.azure.com"
ARG checkoutFrontendBundleUrl="https://avdonl0p0checkout0fe.blob.core.windows.net/frontend/static/js/main.js"
ENV apiPublicUrl=${apiPublicUrl}
ENV swedenClientId=${swedenClientId}
ENV swedenClientSecret=${swedenClientSecret}
ENV finlandClientId=${finlandClientId}
ENV finlandClientSecret=${finlandClientSecret}
ENV checkoutBackendApiUrl=${checkoutBackendApiUrl}
ENV checkoutFrontendBundleUrl=${checkoutFrontendBundleUrl}
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Gir.dll"]