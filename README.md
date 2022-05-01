
# Heavy Metal Machine: A Music Forum

This is a web application built with ASP.NET Core that provides several REST API endpoints to support the world's least favorite music forum.

The web application is currently built with a RESTful web API service providing backend services, and a planned frontend to be built with Blazor.

## Installing Dependencies

First, ensure that you have .NET 6 installed and configured on the target system. If not, please follow the instructions for your OS here: [Download .NET 6](https://dotnet.microsoft.com/en-us/download/dotnet/6.0).

Then, navigate to the root of the application file system and run `dotnet restore` to install the NuGet dependencies for all projects within the solution.

## HeavyMetalMachine.Api

### Running the ASP.NET Core Web API Application

First, navigate to the [/src/HeavyMetalMachine.Api](https://github.com/ketchup-cfg/Somewhere/tree/main/src/HeavyMetalMachine.Api) directory, and create a file named appsettings.Development.json.

Then, add the following content in the file to point the application to a configured PostgreSQL database:
```json
{
   "ConnectionStrings": {
      "HeavyMetalMachine": "<PostgreSQL Database Connection String>"
   }
}
```

After the appsettings file has been configured, either run `dotnet run` from the [/src/HeavyMetalMachine.Api](https://github.com/ketchup-cfg/Somewhere/tree/main/src/HeavyMetalMachine.Api) directory or run `dotnet run --project src/HeavyMetalMachine.Api/HeavyMetalMachine.Api.csproj` from the app's root directory to run the ASP.NET Core Web API application.

## Running Unit Tests

To run unit tests for the application, all that is required is to navigate to the root of the application file system and run `dotnet test`, which will run all unit tests for the application.
