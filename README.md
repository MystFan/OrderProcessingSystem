OrderProcessingSystem
=====================

Small, example Order Service built with .NET 10 using a layered architecture (API, Application, Domain, DataAccess) and common building blocks (validation, repository, unit-of-work, messaging). The service demonstrates: 

- HTTP minimal API endpoints (create order, query order and order items)
- MediatR-based command / query handlers
- EF Core (Npgsql) data access and migrations
- Outbox pattern + MassTransit for reliable event publishing to RabbitMQ
- Simple domain model with Order and OrderItem entities and status transitions

Quickstart
----------
Prerequisites
- .NET 10 SDK
- PostgreSQL (or a connection string to Postgres)
- RabbitMQ (for message publishing)

Build
- From solution root:
  dotnet build

Run API locally
- From solution root or from OrderService.Api project folder:
  dotnet run --project OrderService/OrderService.Api/OrderService.Api.csproj

Configuration
- The project reads configuration from appsettings.json and environment variables.
- Important configuration sections / environment variables:
  - database:ConnectionString => PostgreSQL connection string
  - RABBITMQ:HOST, RABBITMQ:USERNAME, RABBITMQ:PASSWORD => RabbitMQ host and credentials

Database migrations
- Migrations project: OrderService/OrderService.Migrations
- To apply migrations use dotnet-ef or run the migrations project (it is configured as a small console app). Example using dotnet ef:
  dotnet tool install --global dotnet-ef
  dotnet ef database update --project OrderService/OrderService.Migrations --startup-project OrderService/OrderService.Api

API Endpoints
- POST /create-order
  - Body: CreateOrderCommand (customerId, items[] with productId, productName, unitPrice, quantity)
  - Returns: { orderId }
- GET /get-order?orderId={guid}
  - Returns summary of order (order number, customer id, status, currency, total amount, updatedAt)
- GET /get-order-items?orderId={guid}
  - Returns list of order items for given order
- Swagger UI is available in Development environment (configured in Program.cs)

Tests
- Unit tests are under OrderService/OrderService.Tests
- Run tests:
  dotnet test

Notes
- The service uses an Outbox table to store events created during the command handling. A hosted background service (OutboxPublisherHostedService) reads pending outbox messages and publishes them to RabbitMQ using MassTransit. Outbox messages are marked as published after successful publish.
- The data access layer uses a generic repository / unit-of-work pattern implemented in BuildingBlocks.DataAccess and wired into the API project DI.

Project structure (high level)
- OrderService.Api - HTTP API, DI registration, hosted services, MassTransit wiring
- OrderService.Application - Commands, Queries, MediatR handlers, validators
- OrderService.Domain - Domain entities, domain exceptions, enums
- OrderService.DataAccess - EF Core DbContext, configurations, repository implementation
- OrderService.Migrations - EF Core migrations
- BuildingBlocks.* - small reusable abstractions (Application, Domain, DataAccess, Infrastructure, Messaging, Tests)

License
- Add an appropriate license file to the repository if needed.
