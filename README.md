# CurrencyConverter
Web API for currency conversion

## Tooling

### Required

- .NET 8.0

### Optional

- Docker - Tested on Docker client 27.0.3, Docker engine 27.0.3, Docker Desktop 4.32.0 and Docker Compose v2.28.1-desktop.1
- GNU Make

## Build

```
make build
```

## Run the API with Docker

```
make run
```

## Run unit tests

```
make unit-test
```

### Access

- API: <http://{docker-machine-ip}:5000>

## Architecture
- Hexagonal Architecture (Ports and Adapters)

## Feature Highlights
- Validation using Fluent Validation
- Error handling
- Native .NET Dependency Injection
- Pagination
- Docker container to run unit tests

## Endpoints

### Retrieve the latest exchange rates

Method: GET

URI: /api/v1/currency/latest?from=EUR

### Convert amounts between different currencies

Method: GET

URI: /api/v1/currency/convert?amount=1&fromcurrency=USD&tocurrency=CHF

### Return historical rates for a given period using pagination based on a specific base currency

Method: GET

URI: /api/v1/currency/historical?base=CHF&startdate=2024-09-12&enddate=2024-09-16&page=2&pagesize=5

## Improvements

- Building Blocks and Franfurter API SDK projects can be separated so that they can be re-used by other teams for easy integration. They can be in their own repositories on GitHub with their own CI-CD pipeline publishing NuGet packages.
- Since the Franfurter API data refreshes at 16:00 CET every working day, we can cache the data on our side so that we don't need to call the API multiple times. We can invalidate the cache the next time we need to call the API.
- We can introduce a Domain layer and adopt a Domain-driven design approach if we can have well-defined user stories.
- We can use Redis for distributed rate limiting.
- We can add integration tests.