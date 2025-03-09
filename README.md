# TestTask – Employee Data Processor
This ASP.NET MVC project processes employee data from an external CSV file and imports it into a SQL Server database. It demonstrates a clean architecture with separated layers for Domain, Business Logic, Infrastructure, and the Presentation (APP) layer.

### Table of Contents
- [Overview](#overview)
- [Architecture](#architecture)
- [Features](#features)
- [Prerequisites](#prerequisites)
- [Installation and Setup](#installation-and-setup)
- [Running the Application](#running-the-application)
- [Running Tests](#running-tests)
- [CI/CD with GitHub Actions](#cicd-with-github-actions)
- [Logging and Error Handling](#logging-and-error-handling)

### Overview
The TestTask project processes CSV data from an external system, maps the data to a domain model using CsvHelper, and then inserts it into a SQL Server database. A simple ASP.NET MVC front-end allows users to upload the CSV file, view imported records in a sortable, searchable grid (with inline editing), and receive feedback on the number of records successfully imported.

### Architecture
The solution is organized into the following layers:

- Domain: Contains core entities (e.g., Employee) and interfaces for repositories and factories.
- BLL (Business Logic Layer): Implements business services (e.g., EmployeeImportService) and factories (e.g., EmployeeFactory).
- Infrastructure: Provides the EF Core DbContext (TestTaskDbContext), repository implementations, and entity configurations.
- APP (Presentation): ASP.NET MVC project providing the user interface (controllers, views) and integration of third-party libraries such as DataTables and Bootstrap.
### Features
- CSV Import: Users can upload a CSV file. The file is parsed using CsvHelper with a custom mapping (EmployeeImportDtoMap).
- Data Mapping: CSV fields are mapped to an EmployeeImportDto and then converted to a domain entity using a factory.
- Database Integration: Employee data is inserted into a SQL Server database.
- Grid Display: Imported records are displayed in a responsive grid that supports sorting and searching.
- Inline Editing: Users can edit individual employee records using a modal form, with changes saved via AJAX.
- Error Handling: Detailed errors are logged using Serilog while user-friendly messages are returned to the client.
- Unit Testing: The project includes unit tests to ensure service functionality.
- CI/CD: A GitHub Actions workflow is set up to automatically build and run tests on every push or pull request.

### Prerequisites
- .NET 8 SDK
- SQL Server Express or LocalDB (for development)
- Alternatively, use Docker for SQL Server.
- Visual Studio 2022 or your preferred IDE for ASP.NET Core development.
- (Optional) Chocolatey for package management on Windows.
### Installation and Setup
1. Clone the Repository:
```bash
git clone https://github.com/DenisBezdolny/TestTask.git
cd TestTask
```
2. Configure the Connection String:
Update the appsettings.json file in the startup project (TestTask) with your SQL Server connection string. For example, for LocalDB:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\MSSQLLocalDB;Database=TestTaskDB;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True;"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*"
}
```
3. Install Dependencies:
Restore NuGet packages by running:
```bash
dotnet restore
```
4. Database Migration:
```bash
dotnet ef migrations add InitialCreate --project TestTask.Infrastructure --startup-project TestTask
dotnet ef database update --project TestTask.Infrastructure --startup-project TestTask
```

### Running the Application
1. From Visual Studio:
Open the solution and run the TestTask.App project.

2. From Command Line:


```bash
dotnet run --project TestTask
```
Then navigate to the URL shown in the console output.

### Running Tests
Unit tests are located in the TestTask.Tests project. To run tests, execute:

```bash
dotnet test --no-build --configuration Release
```

### CI/CD with GitHub Actions
A GitHub Actions workflow is provided in .github/workflows/dotnet-ci.yml. The workflow is triggered on pushes and pull requests to the main branch. It checks out the repository, sets up .NET, restores dependencies, builds the solution, and runs tests.

Example snippet from the workflow:

```yaml
name: .NET CI

on:
  push:
    branches: [ main ]
  pull_request:
    branches: [ main ]

jobs:
  build-and-test:
    runs-on: windows-latest

    steps:
    - name: Checkout repository
      uses: actions/checkout@v3

    - name: Setup .NET
      uses: actions/setup-dotnet@v3
      with:
        dotnet-version: '8.0.x'

    - name: Restore dependencies
      run: dotnet restore TestTask.sln

    - name: Build
      run: dotnet build TestTask.sln --no-restore --configuration Release

    - name: Run tests
      run: dotnet test TestTask.sln --no-build --configuration Release --verbosity normal

```
### Logging and Error Handling
- Serilog:
The application uses Serilog for logging. Detailed errors are logged internally, while user-friendly messages are returned to the client.

- Error Handling in Editing:
The EditEmployee action catches database update exceptions and returns a generic error message to the user while logging the detailed SQL error (e.g., missing required field).
