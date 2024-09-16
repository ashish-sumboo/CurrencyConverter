FROM mcr.microsoft.com/dotnet/sdk:8.0-alpine AS build

COPY ./src/CurrencyConverter/CurrencyConverter.csproj ./CurrencyConverter/
COPY ./src/BuildingBlocks/BuildingBlocks.csproj ./BuildingBlocks/
COPY ./src/CurrencyConverter.UnitTests/CurrencyConverter.UnitTests.csproj ./CurrencyConverter.UnitTests/
RUN dotnet restore CurrencyConverter.UnitTests/CurrencyConverter.UnitTests.csproj

COPY ./src/ ./
WORKDIR /CurrencyConverter.UnitTests/

CMD ["dotnet", "test"]
