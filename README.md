
# Forum-Web-App

This application is a ASP.NET Core web application, with both an ASP.NET Core REST API web server (src/Forum.Api) and a full-stack Razor Pages web application (src/Forum.Pages).

First, ensure that you have .NET 6 installed and configured on the target system. If not, please follow the instructions for your OS here: https://dotnet.microsoft.com/en-us/download/dotnet/6.0.

Then, navigate to the root of the application file system and run `dotnet restore` to install the NuGet dependencies for all projects within the solution.

## Running the ASP.NET Core Web API Application

First, navigate to the /src/Forum.Api directory, and create a file named appsettings.Development.json.

Then, add the following content in the file to point the application to a configured PostgreSQL database:
```json
{
   "ConnectionStrings": {
      "Forum": "<PostgreSQL Database Connection String>"
   }
}
```

After the appsettings file has been configured, run `dotnet run` while in the /src/Forum.Api directory to run the ASP.NET Core Web API application.

## Running the ASP.NET Core Razor Pages Application (Work In Progress)

***As a note, the work for the Razor Pages application has not progressed, yet, so as of right now, there is not much to see here.***

First, navigate to the /src/Forum.Pages directory, and create a file named appsettings.Development.json.

Then, add the following content in the file to point the application to a configured PostgreSQL database:
```json
{
   "ConnectionStrings": {
      "Forum": "<PostgreSQL Database Connection String>"
   }
}
```

After the appsettings file has been configured, run `dotnet run` while in the /src/Forum.Pages directory to run the ASP.NET Core Razor Pages application.

## Running Unit Tests

To run unit tests for the application, first, ensure that you have a PostgreSQL database configured, and that you have the following environment variables set to whatever values you would prefer for your test PostgreSQL system:
- POSTGRES_HOST
- POSTGRES_PORT
- POSTGRES_USER
- POSTGRES_PASSWORD
- POSTGRES_DB

Then, navigate to the root of the application file system and run `dotnet test` to run the unit tests for the application.
