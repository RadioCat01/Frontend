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

## Oracle Linux with Nginx

The recommended production layout is Nginx on ports 80/443 and ASP.NET Core on `127.0.0.1:5000`.

Publish the Linux application from Windows:

```powershell
dotnet publish .\EnhanzerAPI\EnhanzerAPI.csproj -c Release -r linux-x64 --self-contained true -o .\EnhanzerAPI\linux-publish
```

Copy the contents of `linux-publish` to `/opt/enhanzer` on the Oracle Linux VM, then install the templates from `deploy/`:

```bash
sudo mkdir -p /etc/enhanzer
sudo cp deploy/enhanzer.service /etc/systemd/system/enhanzer.service
sudo cp deploy/enhanzer.env.example /etc/enhanzer/enhanzer.env
sudo chmod 600 /etc/enhanzer/enhanzer.env
sudo cp deploy/nginx/enhanzer.conf /etc/nginx/conf.d/enhanzer.conf
sudo systemctl daemon-reload
sudo systemctl enable --now enhanzer
sudo nginx -t && sudo systemctl reload nginx
```

Edit `/etc/enhanzer/enhanzer.env` with the production SQL Server connection string and JWT key before starting the service. Set `server_name` in the Nginx template to the public hostname.
