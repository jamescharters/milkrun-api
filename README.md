# MILKRUN Products API

This repository contains code for the MILKRUN Products API.

# Getting Started

This application has been built using .NET 8. 

1. Clone the repository
2. Open up the solution in VS or Rider
3. From the root of the cloned repository, run `dotnet build`, `dotnet run --project MilkrunApi` or `dotnet test` depending on your needs

When run, the API application will automatically attempt to seed a local SQLite database using the JSON file located under `MilkrunApi\Data\databaseSeed.json`.

# Errata

Several design decisions have been made in the implementation, namely:

* Assume a reasonable subset of fields are mandatory for write operations (title, brand, description, price)
* Use of controllers rather than e.g. Minimal API
* Input validation performed on controller endpoint and models via attributes
* Use of Service and Repository patterns for encapsulating business logic, separation of concerns
* Basic authentication scheme (i.e. `Authorization: Basic test_user:test_password` header) for privileged POST and PUT operations
* Standard model validation error response structure for POST and PUT endpoints, for sake of implementation simplicity
* No response rate limiting, caching, CORS etc for sake of implementation simplicity
* No upper bound on `page` and especially `limit` for the GET operation, for sake of implementation simplicity. Ideally one would not allow a user to select all records in the table with a sufficiently large `limit` parameter value
* Basic logging to console, for sake of simplicity
* Unit testing of Service logic is included, though this is implicitly tested via the integration tests
* Integration testing with in-memory instance of SQLite, to confirm that database interactions, seeding etc work as expected without cumbersome mocking
* Use of standard suite of NuGet packages

# Additional Ideas

Some thoughts on improvements, extensions, scaling...

* More robust authentication and authorisation on privileged API endpoints (e.g. JWT)
* A more robust datastore based on anticipated usage characteristics (e.g. Nosql, relational: will the API be read or write heavy?)
* Logging / monitoring via Grafana, Sentry, Datadog etc
* Implement rate limiting, CORS, caching and so forth, though these may also be addressed at an cloud infrastructure level
* Decoupling write operations from API requests, e.g. write-through cache, use event queue / message bus etc
* Executing unit and integration tests via automated CI/CD process
* Use [Testcontainers](https://testcontainers.com/guides/getting-started-with-testcontainers-for-dotnet/) in the integration test suite to run the API in real Docker containers (probably overkill for something so simple here)


