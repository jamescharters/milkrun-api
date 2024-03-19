# MILKRUN Products API

This repository contains code for the MILKRUN Products API.

# Getting Started

1. Clone the repository
2. Open up the solution in VS or Rider
3. From the root of the cloned repository, run `dotnet build`, `dotnet run` or `dotnet test` depending on your needs

When run, the API application will automatically attempt to seed a local SQLite database using the JSON file located under `MilkrunApi\Data\databaseSeed.json`.

# Errata

Several design decisions have been made in the implementation of this software, namely:

* Use of traditional style controllers rather than e.g. Minimal API
* Input validation performed on controller endpoint and models via attributes
* Use of Service and Repository patterns for encapsulating business logic, separation of concerns
* Basic authentication scheme (i.e. 'Authorization: Basic test_user test_password' header) for privileged POST and PUT operations
* No support to response rate limiting, caching, CORS etc for sake of implementation simplicity
* Basic logging to console, for sake of simplicity
* Unit testing of Service logic
* Integration testing with in-memory instance of SQLite, to confirm that database interactions, seeding etc work as expected without cumbersome mocking
* Use of standard suite of NuGet packages

# Additional ideas

Some thoughts on improvements, extensions:

* Executing unit and integration tests via automated CI/CD process
* Move logging / monitoring to 3rd party service
* Implement rate limiting, CORS, caching and so forth, though these may also be addressed at an cloud infrastructure level
* Use Testcontainers in integration test suite to run API in real Docker containers (probably overkill for something so simple here)


