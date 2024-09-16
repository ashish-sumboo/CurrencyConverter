SHELL		    := bash
BUILD_CONFIG	:= Release

VERSION	= 1.0.0

build: ## Builds the solution
	@echo "building CurrencyConverter v$(VERSION)..."
	dotnet build --configuration $(BUILD_CONFIG)

unit-test: ## Runs unit tests
	@echo "running unit tests..."
	dotnet test ./src/CurrencyConverter.UnitTests/CurrencyConverter.UnitTests.csproj \
		--configuration $(BUILD_CONFIG)

run: ## Runs the services in development mode
	@echo "building the CurrencyConverter..."
	@(docker compose up -d --build --force-recreate)

stop: ## Stops the services
	@echo "stopping the services..."
	@docker compose down -v

help: ## Shows this help screen
	@fgrep -E '^[a-zA-Z_-]+:.*?## .*$$' $(MAKEFILE_LIST) | sort | awk \
		'BEGIN {FS = ":.*?## "}; {printf "\033[36m%-30s\033[0m %s\n", $$1, $$2}'
