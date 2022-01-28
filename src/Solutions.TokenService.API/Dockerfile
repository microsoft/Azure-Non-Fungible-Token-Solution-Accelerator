#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["Solutions.TokenService.API/Solutions.TokenService.API.csproj", "Solutions.TokenService.API/"]
COPY ["Solutions.Security.Cosmos/Solutions.Security.Cosmos.csproj", "Solutions.Security.Cosmos/"]
COPY ["Solutions.HTTP/Solutions.HTTP.csproj", "Solutions.HTTP/"]
COPY ["Solutions.TokenService.BlockchainNetworkManager/Solutions.TokenService.BlockchainNetworkManager.csproj", "Solutions.TokenService.BlockchainNetworkManager/"]
COPY ["Solutions.TokenService.LedgerClient/Solutions.TokenService.LedgerClient.csproj", "Solutions.TokenService.LedgerClient/"]
COPY ["Solutions.TokenService.KeyManagement/Solutions.TokenService.KeyManagement.csproj", "Solutions.TokenService.KeyManagement/"]
COPY ["Solutions.TokenService.UserManager/Solutions.TokenService.UserManager.csproj", "Solutions.TokenService.UserManager/"]
COPY ["Solutions.TokenService.PartyManager/Solutions.TokenService.PartyManager.csproj", "Solutions.TokenService.PartyManager/"]
RUN dotnet restore "Solutions.TokenService.API/Solutions.TokenService.API.csproj"
COPY . .
WORKDIR "/src/Solutions.TokenService.API"
RUN dotnet build "Solutions.TokenService.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Solutions.TokenService.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Solutions.TokenService.API.dll"]