# Setup & Run Instructions
<img width="1912" height="968" alt="image" src="https://github.com/user-attachments/assets/3f493604-9f9b-472e-91aa-7f9d88b13311" />
<img width="1892" height="951" alt="image" src="https://github.com/user-attachments/assets/89811a93-6895-434d-9c1c-dcf2d79db1fb" />
<img width="1580" height="862" alt="image" src="https://github.com/user-attachments/assets/fe408bc0-c4be-44fe-bb5b-a60ab60fb3d5" />
<img width="1918" height="962" alt="image" src="https://github.com/user-attachments/assets/293324dd-c5e7-4bc8-bd2d-7e90d7463edc" />


## Prerequisites
- .NET 8 SDK
- Node.js & npm
- Visual Studio 2026 (optional, for debugging)

## Backend Setup
```powershell
cd EnhanzerAPI
dotnet restore
dotnet build
```

## Frontend Setup
```powershell
cd Frontend
npm install
npm start
```

## Run Development Mode
**Terminal 1 - Start API:**
```powershell
cd EnhanzerAPI
dotnet run
```
API runs on `http://localhost:5000`

**Terminal 2 - Start Frontend:**
```powershell
cd Frontend
npm start
```
Frontend runs on `http://localhost:4200`

## Publish & Run Release Build
```powershell
cd EnhanzerAPI
dotnet publish -c Release -o ./publish
cd publish
./EnhanzerAPI.exe
```
Access on `http://localhost:5000`

## Database
Ensure SQL Server is running. Connection string in `appsettings.json`.
Apply migrations if needed:
```powershell
cd EnhanzerAPI
dotnet ef database update
```
