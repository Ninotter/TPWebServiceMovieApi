
# WebService MovieList API
Created using .NET 6, Entity Framework

## Installation
### Requirements
- WAMP/LAMP/XAMP
- [.NET 6](https://dotnet.microsoft.com/en-us/download/dotnet/6.0)
- .NET CLI (included in .NET SDK)
### Setup
Updating connection strings:
Open appsettings.json and update your connection strings with your MySQL database:
```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*",
  "ConnectionStrings": {
    "mysql": "server=localhost;database=webserviceapi;user=root;password=" <--- change
  }
}
```
Migrating the code-first schema to the database :
```bash
dotnet tool install --global dotnet-ef
dotnet add package Microsoft.EntityFrameworkCore.Design
dotnet ef migrations add InitialCreate
dotnet ef database update
```
Once the migration has been correctly done, building the application : 
```bash
dotnet build -c Release
```
Execute `bin\Release\net6.0\TPWebServiceApiRestMovie.exe` to start the server.
Once the server is running, open a browser tab to the URL https://localhost:5001/swagger .
