FROM mcr.microsoft.com/dotnet/runtime:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["castledice-game-server/castledice-game-server.csproj", "castledice-game-server/"]
RUN dotnet restore "castledice-game-server/castledice-game-server.csproj"
COPY . .
WORKDIR "/src/castledice-game-server"
RUN dotnet build "castledice-game-server.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "castledice-game-server.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "castledice-game-server.dll"]
