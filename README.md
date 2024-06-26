# Dotnet 8 Introduction

## API using Dotnet (cross platform)

[![LICENSE](https://img.shields.io/github/license/vitorserrano/task-manager?color=7159C1)](LICENSE.md)
![.Net](https://img.shields.io/badge/.NET-5C2D91?style=for-the-badge&logo=.net&logoColor=white)
![C#](https://img.shields.io/badge/c%23-%23239120.svg?style=for-the-badge&logo=c-sharp&logoColor=white)

## Description

A introduction Web API with Dotnet 8 LTS.

## TODO

- Authentication [In progress]
- AutoMapper use [In progress]
- JWT support [In progress]
- User login with privacy
- More features: Manage a buy list

## Version

- Dotnet 8.0 LTS

SDK version: `dotnet --list-sdks with result 8.0.105 [/usr/share/dotnet/sdk]`

## SGBD

- SQL Server

## Testing environment

Create a appsettings.Testing.json.
The Microsoft.EntityFrameworkCore.Sqlite dependency already available in dotnet8-introduction.csproj.

```json
{
    "ConnectionStrings": {
        "SqliteConnection": "Data Source=Testing.db"
    }
}

```

NOTE: Edit the Program.cs uncomment the lines.

No build:

`dotnet test --no-build --verbosity normal`

Or with build:

`dotnet test --verbosity normal`

## To run

`dotnet run`

Or

`dotnet watch run`

## To create migrations

- EF Core Migrations: `dotnet ef migrations add InitialMigration`

## To create tables

`dotnet ef database update`

## Routes

Accessing `http://localhost:5064/swagger/index.html`

## Instructions to use Docker

- [Docker images for ASP.NET Core](https://docs.microsoft.com/en-us/aspnet/core/host-and-deploy/docker/building-net-docker-images?view=aspnetcore-8.0)

## Requirements

- SQL Server
- Dotnet (version 8.0)
- Dotnet EF (Entity Framework)
- Visual Studio Code or Visual Studio 2019
- Postman or Insomnia

## LICENSE

See [LICENSE](LICENSE.md)

## Docs

- [Explore Microsoft Open Source projects and initiative](https://opensource.microsoft.com/)
