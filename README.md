# My Teams Hub Project Template

This repository provides a template for building application using Clean Vertical Slice Architecture with .NET 8.

# Features
- CQRS: Command and Query Responsibility Segregation.
- MediatR Library: Implements the mediator pattern for handling requests and notifications.
- Result Pattern: Standardized way of handling operation results.
- Adapter for in memory(InMemory) and distributed cache(IDistributedCache) usage.
- Message Broker RabbitMQ with MassTransit library
- Entity Framework Core with MSSQL & PostgreSQL databases
- Centralized Package Management
- Containerized .NET Core API along with its dependencies(MSSQL, PostgreSQL, RabbitMQ, Redis etc.)
- Coding conventions & static code analysis
- Unit Tests using MoQ, InMemory database
- Integration tests using Test Containers library
- Dependencies tests

