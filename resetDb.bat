del crunchy.db
del Migrations /F /Q
dotnet ef migrations add DevStart
dotnet ef database update