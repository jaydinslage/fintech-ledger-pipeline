# Fintech Ledger Pipeline

Fintech Ledger Pipeline is a lightweight ASP.NET Core service for ingesting ledger entries and applying a simple processing pipeline before returning the enriched payload.

## Features

- Exposes a REST endpoint for ledger ingestion
- Validates required fields before processing
- Applies a lightweight pipeline of steps (validation, enrichment, routing)
- Uses a persistence abstraction with an in-memory implementation for future backing stores
- Emits structured JSON logs with correlation IDs and request-scoped context
- Includes OpenTelemetry tracing and metrics for request observability
- Returns clear validation and server error responses
- Includes OpenAPI support in development
- Includes a Dockerfile for container-based runs

## API Overview

### Endpoint

- POST /api/ledger

### Request Example

```http
POST /api/ledger HTTP/1.1
Content-Type: application/json

{
  "accountId": "acct-1001",
  "amount": 125.50,
  "currency": "USD",
  "timestamp": "2026-07-12T10:30:00Z",
  "sourceSystem": "core-banking"
}
```

### Response Example

```json
{
  "id": "4c35d1f4-3df7-4c1d-8d7e-a62d4e2f674f",
  "accountId": "acct-1001",
  "amount": 125.5,
  "currency": "USD",
  "timestamp": "2026-07-12T10:30:00Z",
  "sourceSystem": "core-banking",
  "metadata": {
    "processed": "true",
    "pipeline": "ledger-intake",
    "validation": "validated",
    "enrichment": "enriched",
    "routing": "routed"
  }
}
```

### Validation Rules

The service validates the incoming payload before processing:

- `accountId` is required
- `sourceSystem` is required
- `amount` must be greater than zero
- `currency` defaults to `USD` when omitted

Invalid requests return a `400 Bad Request` response with an error message.

## Architecture Overview

The service is organized around a small layered design:

- Controllers handle incoming HTTP requests
- Services contain the business processing logic behind an interface contract
- Models define the request and response payload contract
- A lightweight pipeline step pattern can be composed for validation, enrichment, and routing
- A repository abstraction is present with an in-memory implementation for future persistence
- Structured logs and OpenTelemetry instrumentation provide request-level observability

This keeps the entrypoint thin while allowing the processing behavior to evolve independently.

## Project Structure

```text
src/
  Fintech.LedgerPipeline.Service/
    Controllers/
    Events/
    Models/
    Pipelines/
    Repositories/
    Services/
    Program.cs
    appsettings.json
    appsettings.Development.json

tests/
  Fintech.LedgerPipeline.Service.Tests/
    LedgerProcessingServiceTests.cs
    LedgerPipelineTests.cs
    RequestLoggingMiddlewareTests.cs
```

## Running the Service

From the repository root, run:

```bash
dotnet run --project src/Fintech.LedgerPipeline.Service
```

Then send a request to the endpoint above using your preferred HTTP client.

To run the tests:

```bash
dotnet test tests/Fintech.LedgerPipeline.Service.Tests/Fintech.LedgerPipeline.Service.Tests.csproj
```

To build the container image:

```bash
docker build -t fintech-ledger-pipeline .
```
