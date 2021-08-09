# Work with PostgreSQL v12/13 using ASP.NET Core Identity and EF (Entity Framework) Core

Use the following commands from the Package Manager Console to add/modify a EF Core context.
```
PM> dotnet tool install --global dotnet-ef
PM> cd .\DataAccessLayer
PM> dotnet ef dbcontext scaffold "Host=localhost;Database=identitywithpgsql;Username=postgres;Password=Tetya1:2" Npgsql.EntityFrameworkCore.PostgreSQL -f -o  Models
```
