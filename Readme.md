This app registers users with User and General user .

Perform operation based on it.

DB will be created at first call in MSSQL Local db. Can be customized in app json.

dotnet tool install --global dotnet-ef --version 3.1.4
dotnet ef migrations add test

dotnet ef database update
