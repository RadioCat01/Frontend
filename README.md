# Enhanzer Purchase Bill Application

Combined Angular and ASP.NET Core application for the Enhanzer assignment.

## Project structure

- `Frontend/` - Angular application
- `EnhanzerAPI/` - ASP.NET Core 8 Web API, database models, migrations, and SQL script

## Development

Start the API:

```powershell
cd EnhanzerAPI
dotnet run
```

Start Angular in a separate terminal:

```powershell
cd Frontend
npm install
npm start
```

The development frontend uses the API at `http://localhost:5259` through its configured service URLs.

## Unified production publish

The API project builds the Angular production bundle automatically and copies it into `wwwroot` during publish:

```powershell
dotnet publish .\EnhanzerAPI\EnhanzerAPI.csproj -c Release -o .\EnhanzerAPI\publish
```

Run the published application:

```powershell
cd EnhanzerAPI\publish
.\EnhanzerAPI.exe
```

The published application serves both the Angular UI and API from the same origin. For Oracle Linux, publish with a Linux runtime:

```powershell
dotnet publish .\EnhanzerAPI\EnhanzerAPI.csproj -c Release -r linux-x64 --self-contained true -o .\EnhanzerAPI\linux-publish
```

## Database

The default development configuration uses SQL Server LocalDB. The database script is available at `EnhanzerAPI/Database/Location_Details.sql`. Configure a production SQL Server connection through `ConnectionStrings__DefaultConnection` rather than committing production credentials.
