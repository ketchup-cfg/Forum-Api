
# some-web-api

This is a web API application built with ASP.NET Core that provides several REST API endpoints to support some kind of forum.

It currently supports several endpoints that allow for forum topics to be retrieved, created, updated, or deleted.

## Installing Dependencies

First, ensure that you have .NET 6 installed and configured on the target system. If not, please follow the instructions for your OS here: [Download .NET 6](https://dotnet.microsoft.com/en-us/download/dotnet/6.0).

Then, navigate to the root of the application file system and run `dotnet restore` to install the NuGet dependencies for all projects within the solution.

## Somewhere.Api

### Running the ASP.NET Core Web API Application

First, navigate to the [/src/Somewhere.Api](https://github.com/ketchup-cfg/Somewhere/tree/main/src/Somewhere.Api) directory, and create a file named appsettings.Development.json.

Then, add the following content in the file to point the application to a configured PostgreSQL database:
```json
{
   "ConnectionStrings": {
      "Somewhere": "<PostgreSQL Database Connection String>"
   }
}
```

After the appsettings file has been configured, either run `dotnet run` from the [/src/Somewhere.Api](https://github.com/ketchup-cfg/Somewhere/tree/main/src/Somewhere.Api) directory or run `dotnet run --project src/Somewhere.Api/Somewhere.Api.csproj` from the app's root directory to run the ASP.NET Core Web API application.

### Swagger API Documentation

This application provides functionality to serve documentation and a testing environment with Swagger at the `/swagger` endpoint when the application is run in a development environment.

To ensure that you are running the application in a development environment, ensure that the `ASPNETCORE_ENVIRONMENT` environment variable is set to `"Development"`.

Once this has been done, run `dotnet run --project src/Somewhere.Api/Somewhere.Api.csproj` to startup the Somewhere.Api application, the navigate to `https://localhost:<port>/swagger` in a web browser to view the Swagger documentation for the Somewhere.Api application.

## Running Unit Tests

To run unit tests for the application, all that is required is to navigate to the root of the application file system and run `dotnet test`, which will run all unit tests for the application.
