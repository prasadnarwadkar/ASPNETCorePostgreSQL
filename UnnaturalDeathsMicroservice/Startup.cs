// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using BusinessLogicLayer;
using Common;
using Common.Models;
using DataAccessLayer;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.Filters;
using System;
using System.IO;
using System.Reflection;
using System.Text.Json;
using UnnaturalDeathsMicroservice.ServiceBus;

namespace Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        readonly string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

        public void ConfigureServices(IServiceCollection services)
        {
            IdentityModelEventSource.ShowPII = true;
            
            services.AddCors(options =>
            {
                options.AddPolicy(name: MyAllowSpecificOrigins,
                                  builder =>
                                  {
                                      builder.WithOrigins("*").AllowAnyHeader().AllowAnyMethod();
                                  });
            });

            services.AddControllers().AddJsonOptions(option =>
            option.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase);

            // accepts any access token issued by identity server
            services.AddAuthentication("Bearer")
                .AddJwtBearer("Bearer", options =>
                {
                    options.Authority = "https://identityserverfhir.azurewebsites.net";
                    
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateAudience = false
                    };
                });


            services.AddSingleton<IConfiguration>(Configuration);

            // adds an authorization policy to make sure the token is for scope 'fhir'
            services.AddAuthorization(options =>
            {
                options.AddPolicy("ApiScope", policy =>
                {
                    policy.RequireAuthenticatedUser();
                    policy.RequireClaim("scope", "fhir");
                });
            });

            services.AddSwaggerGen(c =>
            {
                c.AddSecurityDefinition("oauth2", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                {
                    Description = "Standard Authorization header using the Bearer scheme. Example: 'bearer \\<token\\>'." +
                                                Environment.NewLine + Environment.NewLine + "To get token, do a POST as follows:" + 
                                                Environment.NewLine + Environment.NewLine +
                                                "POST /connect/token HTTP/1.1" + 
                                                Environment.NewLine + Environment.NewLine +
                                                "Host: identityserverfhir.azurewebsites.net" +
                                                Environment.NewLine + Environment.NewLine + 
                                                "Content-Type: application/x-www-form-urlencoded" +
                                                Environment.NewLine + Environment.NewLine +
                                                "client_id=<client_id_here>&client_secret=<client_secret_here>&grant_type=client_credentials&scope=fhir" +
                                                Environment.NewLine + Environment.NewLine +
                                                Environment.NewLine + Environment.NewLine + " To get client id and client secret, please send email to <a href='mailto:prasad.narwadkar@live.in'>me.</a>",
                    In = Microsoft.OpenApi.Models.ParameterLocation.Header,
                    Name = "Authorization",
                    Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey
                });

                c.OperationFilter<SecurityRequirementsOperationFilter>();


                c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Version = "v1",
                    Title = "FHIR API V1",
                    Description = "FHIR RESTful ASP.NET Core Web API",
                    TermsOfService = new System.Uri("https://example.com"),
                    Contact = new Microsoft.OpenApi.Models.OpenApiContact() { 
                        Name = "Prasad N", 
                        Email = "prasad.narwadkar@live.in", 
                        Url = new System.Uri("https://example.com")
                    }
                });

                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlFileCommon = "Common.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
                xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFileCommon);
                c.IncludeXmlComments(xmlPath);
            });

            

            // Add framework services.
            services.AddDbContext<identitywithpgsqlContext>(options =>
            options.UseNpgsql(Configuration.GetConnectionString("DefaultConnection")));

            services.AddScoped<IPatientLogic, PatientLogic>();
            services.AddScoped<IPatientRepository, PatientRepositoryEFCore>();

            services.AddScoped<IUnnaturalDeathsLogic, UnnaturalDeathsLogic>();
            services.AddScoped<IUnnaturalDeathsRepository, UnnaturalDeathsRepositoryEFCore>();

            var serviceClientSettingsConfig = Configuration.GetSection("RabbitMq");
            services.Configure<RabbitMqConfiguration>(serviceClientSettingsConfig);

            services.AddSingleton<IDeathDetailsSender, DeathSender>();
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseRouting();

            app.UseCors(MyAllowSpecificOrigins);

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers()
                    .RequireAuthorization("ApiScope");
            });

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "FHIR API V1");
            });
        }
    }
}
