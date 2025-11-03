# Congestion Tax Calculator API

This project is a robust, scalable, and well-tested RESTful API for calculating congestion tax fees. It is built using .NET 8, following the principles of Clean Architecture, Domain-Driven Design (DDD), and Test-Driven Development (TDD).

The application was initially refactored from a procedural, hardcoded script into a flexible system. The final version supports multiple cities with configurable tax rules, which are dynamically loaded from a database at runtime.

## Features

- **Dynamic Tax Calculation**: Calculates the total daily congestion tax for a vehicle based on multiple toll passages.
- **Configurable Multi-City Rules**: All tax parameters are stored in a database and can be configured per city, including:
  - Time-based toll fees.
  - The maximum daily tax amount.
  - The "Single Charge Rule" duration (e.g., only the highest fee within 60 minutes is charged).
  - Tax-exempt vehicle types.
  - Toll-free dates (weekends, specific months, and public holidays).
  - A specific rule for making the day before a public holiday toll-free.
- **RESTful API**: Exposes the calculation logic via a clear and simple `POST` endpoint.
- **Database Persistence**: Uses Entity Framework Core with a SQLite database, which is automatically seeded with rules for Gothenburg on first launch.
- **Containerized**: Fully containerized using Docker and Docker Compose for easy setup and consistent deployment.
- **Comprehensive Test Suite**: Includes a full suite of tests:
  - **Unit Tests** for domain logic and policies.
  - **Application Service Tests** with mocked dependencies.
  - **Infrastructure Integration Tests** to verify database queries.
  - **API Integration Tests** to validate the end-to-end request/response flow.

## Architectural Approach

The solution is built using **Clean Architecture** to ensure a strong separation of concerns, maintainability, and testability.

- **`Domain`**: Contains the core business logic, entities, and rules of the application. It has no dependencies on any other layer.
- **`Application`**: Orchestrates the domain logic to perform specific use cases. It contains application services, DTOs, and repository interfaces.
- **`Infrastructure`**: Handles all external concerns, primarily data access. It contains the Entity Framework `DbContext`, repository implementations, and database seeding logic.
- **`Api`**: The presentation layer. An ASP.NET Core project that exposes the application's functionality through a RESTful API.

This structure ensures that the core business logic is independent of the database, UI, or any external frameworks.

## Technology Stack

- **Backend**: .NET 8, ASP.NET Core 8
- **Database**: Entity Framework Core 8, SQLite
- **Testing**: xUnit, Moq, Microsoft.AspNetCore.Mvc.Testing
- **Containerization**: Docker, Docker Compose

## Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Docker Desktop](https://www.docker.com/products/docker-desktop/)

## How to Run

There are two ways to run the application. The recommended method is using Docker.

### Method 1: Using Docker (Recommended)

This is the simplest way to get the application running in a clean, isolated environment.

1.  Open a terminal in the root directory of the project (where `compose.yaml` is located).
2.  Run the following command:
    ```bash
    docker-compose up --build
    ```
3.  The application will build inside the container and start. The API will be available and the Swagger UI can be accessed at:
    **`http://localhost:8080/swagger`**

### Method 2: Running Locally (Without Docker)

1.  Clone the repository.
2.  Open a terminal in the root directory.
3.  Restore the .NET dependencies:
    ```bash
    dotnet restore
    ```
4.  Apply the EF Core migrations to create the SQLite database:
    ```bash
    dotnet ef database update --startup-project src/CongestionTaxCalculator.Api
    ```
5.  Run the API project:
    ```bash
    dotnet run --project src/CongestionTaxCalculator.Api
    ```
6.  The API will be available at the URL specified in your terminal (typically `https://localhost:7012` or `http://localhost:5184`).

## How to Run the Tests

To run the complete suite of unit and integration tests, navigate to the root directory in your terminal and run:

```bash
dotnet test
```

This command will automatically discover and execute all tests across the four test projects.

## API Usage

The API exposes a single endpoint for calculating the congestion tax.

### Calculate Tax

- **Endpoint**: `POST /api/tax/calculate`
- **Description**: Calculates the total congestion tax for a given vehicle, city, and list of passage times.

#### Example Request Body

```json
{
  "cityName": "Gothenburg",
  "vehicleType": "Car",
  "passages": [
    "2013-02-08T06:27:00Z",
    "2013-02-08T15:29:00Z",
    "2013-02-08T16:01:00Z"
  ]
}
```

#### Example Success Response (`200 OK`)

This response is for the request above. The fee is capped at 18 SEK for the 15:29 and 16:01 passages, plus 8 SEK for the 06:27 passage, totaling 26 SEK.

```json
{
  "taxAmount": 26
}
```

#### Example Error Response (`400 Bad Request`)

This is returned if the specified city is not found in the database.

```json
{
  "error": "City 'NonExistentCity' not found or has no tax rules configured."
}
```

## Project Structure

```
/CongestionTaxCalculator
├── src
│   ├── CongestionTaxCalculator.Api           # Presentation Layer (REST API)
│   ├── CongestionTaxCalculator.Application   # Application Logic, DTOs, Interfaces
│   ├── CongestionTaxCalculator.Domain        # Core Business Logic and Entities
│   └── CongestionTaxCalculator.Infrastructure  # Data Access, EF Core
│
└── tests
    ├── CongestionTaxCalculator.Api.Tests           # API Integration Tests
    ├── CongestionTaxCalculator.Application.Tests   # Application Service Unit Tests
    ├── CongestionTaxCalculator.Domain.Tests        # Domain Logic Unit Tests
    └── CongestionTaxCalculator.Infrastructure.Tests  # Repository Integration Tests
```
