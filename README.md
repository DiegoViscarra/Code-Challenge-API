# Scheduling - API

Scheduling API is a web API developed on .NET Core.

## Purpose

This application is used to manage the schedules between classes and students.

## Prerequisites

* Visual Studio 2019
* .NET Core SDK (3.1 version)
* SQL Server Management Studio

## Database

Create local database with the migration in Command Line:

```sh
$ dotnet ef database update
```

or in the Package Manager Console

```sh
$ update-database
```

## How To Run

* Open solution in Visual Studio 2019
* Run the SchedulingAPI project
* For swagger navigate to `http://localhost:64544/swagger/index.html`