#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443
EXPOSE 3000

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["StocksInfo/StocksInfo.csproj", "StocksInfo/"]
RUN dotnet restore "StocksInfo/StocksInfo.csproj"
COPY . .
WORKDIR "/src/StocksInfo"
RUN dotnet build "StocksInfo.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "StocksInfo.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "StocksInfo.dll", "--urls", "http://+:3000"]
