# Work with PostgreSQL v12/13 using ASP.NET Core Identity and EF (Entity Framework) Core

Use the following commands from the Package Manager Console to add/modify a EF Core context. Please change the command to suit your environment (database as well as fodler structure). 

```
PM> dotnet tool install --global dotnet-ef
PM> cd .\Common
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

# Bi-lingual Support
Support for `Dzongkha` as well as `English` can be added using the following concepts:

- Separate view for each language. (In most cases the views are just copied from the invariant culture view). In some cases, you might want to give a little different UI to your users of a particular language. 
- Controller need not be changed. Same controller is shared by view of multiple languages. 

# Service Bus using RabbitMQ
- Added two example APIs- One that does CRUD for an example entity and other receives messages from the first service via a RabbitMQ queue. No cross-API calls here. 
- Please change your RabbitMQ configuration in app settings in all the relevant API projects as per your environment. 
