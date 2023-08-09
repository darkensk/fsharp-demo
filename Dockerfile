FROM mcr.microsoft.com/dotnet/sdk:6.0 AS base

WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build

WORKDIR /src
COPY . .

RUN dotnet tool restore
RUN dotnet paket install
RUN dotnet restore ./src/Gir/
RUN dotnet build ./src/Gir/

FROM build AS publish
RUN dotnet publish ./src/Gir/ -o /app

FROM base AS final

WORKDIR /app
COPY --from=publish /app .

EXPOSE 80
ENV ASPNETCORE_URLS=http://+:80

ENTRYPOINT ["dotnet", "Gir.dll"]