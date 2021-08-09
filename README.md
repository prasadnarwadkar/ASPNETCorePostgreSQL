# Work with PostgreSQL v12/13 using ASP.NET Core Identity and EF (Entity Framework) Core

Use the following commands from the Package Manager Console to add/modify a EF Core context.

```
PM> dotnet tool install --global dotnet-ef
PM> cd .\DataAccessLayer
PM> dotnet ef dbcontext scaffold "Host=localhost;Database=identitywithpgsql;Username=postgres;Password=Tetya1:2" Npgsql.EntityFrameworkCore.PostgreSQL -f -o  Models
```

## This project's architecture is based on the following concepts:
- ASP.NET Core Identity
- ASP.NET Core MVC
- Entity Framework Core
- [PostgreSQL Driver for .NET Core](https://www.npgsql.org/)

## EF Core
With Entity Framework Core, the code becomes simple to maintain.

Getting a list of objects is very simple. No raw SQL queries.

```
        private readonly identitywithpgsqlContext _context;

        public UnnaturalDeathsRepositoryEFCore(identitywithpgsqlContext context)
        {
            _context = context;
        }

        public async Task<IList<UnnaturalDeathsDto>> GetListAsync()
        {
            var listFromEF =  await _context.Unnaturaldeaths.ToListAsync();
            var list = new List<UnnaturalDeathsDto>();

            foreach (var death in listFromEF)
            {
                list.Add(new UnnaturalDeathsDto {

```

