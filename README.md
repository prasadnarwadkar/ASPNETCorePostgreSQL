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

## Data Synchroization Background Hosted Service (Example)
- Traverse through a source db patient list to see if each patient is in the destination db. If yes, update it if its last modified date is earlier than this record (destination record is stale). If not, update the source record. 
- If this record is not in the destination db, insert it. 
- You can use a UHID or Name or similar ID (not driven by database) to check whether the given entity is there in the destination db. Names can be repeated. So, using UHID or similar unique ID is recommended.
- The same steps can be used for each and every entity.
- The IDs automatically set by the database are unique within the same database but not across two databases. So these need to be avoided. UHID is the best bet.

## EF Core: No Raw SQL Queries please
With Entity Framework Core, the code becomes simple to maintain.

Getting a list of objects is very simple. **No raw SQL queries**.

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

# Zuul API Gateway Added
The following Zuul API gateway configuration works here in this project. Please change the settings below as per your environment.

```
...

server.port = 8091

ribbon.eureka.enabled=false
zuul.sensitiveHeaders=Cookie,Set-Cookie

zuul.routes.api1.url = http://localhost:6001/api/unnaturaldeaths

zuul.routes.index.path = /**
zuul.routes.index.url = http://localhost:5000/Index

...
```

- Please run your Zuul API gateway application on the port 8091. 
- [Unnatural Deaths API](http://localhost:6001/api/unnaturaldeaths) is running on the port 6001. This is called the `downstream API endpoint`. 
- The UI project's configuration (`appsettings.json`) has been updated to use the following:
```
...
,
"ConnectionStrings": {
    "DefaultConnection": "Server=127.0.0.1;Port=5432;Database=identitywithpgsql;User Id=postgres;Password=Tetya1:2;"
},
"Logging": {
    "LogLevel": {
    "Default": "Information",
    "Microsoft": "Warning",
    "Microsoft.Hosting.Lifetime": "Information"
    }
},
"AllowedHosts": "*",
"GatewayUriApi1": "http://localhost:8091/api1"
}
...
```
 See the configuration above which uses the `Zuul API Gateway` endpoint rather than the `downstream API uri`. 
