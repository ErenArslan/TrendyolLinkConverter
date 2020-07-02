#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["TrendyolLinkConverter.Api/TrendyolLinkConverter.Api.csproj", "TrendyolLinkConverter.Api/"]
COPY ["TrendyolLinkConverter.Infrastructure/TrendyolLinkConverter.Infrastructure.csproj", "TrendyolLinkConverter.Infrastructure/"]
COPY ["TrendyolLinkConverter.Core/TrendyolLinkConverter.Core.csproj", "TrendyolLinkConverter.Core/"]
RUN dotnet restore "TrendyolLinkConverter.Api/TrendyolLinkConverter.Api.csproj"
COPY . .
WORKDIR "/src/TrendyolLinkConverter.Api"
RUN dotnet build "TrendyolLinkConverter.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TrendyolLinkConverter.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TrendyolLinkConverter.Api.dll"]