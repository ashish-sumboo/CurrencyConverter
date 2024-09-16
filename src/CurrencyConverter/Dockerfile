FROM mcr.microsoft.com/dotnet/sdk:8.0-alpine AS build

ARG BUILDCONFIG=Release
ARG VERSION=1.0.0

COPY ./src/CurrencyConverter/CurrencyConverter.csproj ./CurrencyConverter/
COPY ./src/BuildingBlocks/BuildingBlocks.csproj ./BuildingBlocks/
RUN dotnet restore CurrencyConverter/CurrencyConverter.csproj

COPY ./src/ ./
WORKDIR /CurrencyConverter/
RUN dotnet publish -c $BUILDCONFIG -o dist /p:Version=$VERSION

FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine
WORKDIR /app/
COPY --from=build /CurrencyConverter/dist ./

EXPOSE 5000
ENTRYPOINT [ "dotnet", "CurrencyConverter.dll"]
