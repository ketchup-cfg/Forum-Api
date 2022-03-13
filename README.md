
# Somewhere

Some kind of forum that's a great place to go when you want to talk about something. I don't know, I don't get paid to come up with catchy names and slogans.

This application is a ASP.NET Core web application, with both an ASP.NET Core REST API web server (src/Somewhere.Api) and a full-stack Razor Pages web application (src/Somewhere.Pages).

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

## Somewhere.Pages (Work In Progress)

### Running the ASP.NET Core Razor Pages Application (Work In Progress)

***As a note, the work for the Razor Pages application has not progressed, yet, so as of right now, there is not much to see here.***

First, navigate to the [/src/Somewhere.Pages](https://github.com/ketchup-cfg/Somewhere/tree/main/src/Somewhere.Pages) directory, and create a file named appsettings.Development.json.

Then, add the following content in the file to point the application to a configured PostgreSQL database:
```json
{
   "ConnectionStrings": {
      "Somewhere": "<PostgreSQL Database Connection String>"
   }
}
```

After the appsettings file has been configured, run `dotnet run` if still in the [/src/Somewhere.Pages](https://github.com/ketchup-cfg/Somewhere/tree/main/src/Somewhere.Pages) directory to run the ASP.NET Core Razor Pages application. Or, you can just run `dotnet run --project src/Somewhere.Pages/Somewhere.Pages.csproj` from the app's root directory instead.

## Running Unit Tests

To run unit tests for the application, first, ensure that you have a PostgreSQL database configured, and that you have the following environment variables set to whatever values you would prefer for your test PostgreSQL system:
- `POSTGRES_HOST`
- `POSTGRES_PORT`
- `POSTGRES_USER`
- `POSTGRES_PASSWORD`
- `POSTGRES_DB`

Then, navigate to the root of the application file system and run `dotnet test` to run all unit tests for the application.
