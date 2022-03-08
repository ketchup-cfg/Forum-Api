# Forum-Web-App

This application is a ASP.NET Core web application, with both an ASP.NET Core REST API web server (src/Forum.Api) and a full-stack Razor Pages web application (src/Forum.Pages).

To run this application:

1. Ensure that you have .NET 6 installed and configured on the target system.
2. Run: `dotnet restore` to install all NuGet project dependencies.
3. Create a file named appsettings.Development.json in both the src/Forum.Data and test/Data.Tests directories with the following content to point the application to a configured PostgreSQL database
```json
{
   "ConnectionStrings": {
   "Forum": "<PostgreSQL Database Connection String>"
   }
}
```
4. If running the REST API web server, navigate to the src/Forum.Api directory, or navigate to the src/Forum.Pages directory for the Razor Pages web application.
5. Run `dotnet run` to run the given application.
