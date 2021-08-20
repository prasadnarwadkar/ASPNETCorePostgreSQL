using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using ASPNETIdentityPostgres.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Common;
using BusinessLogicLayer;
using DataAccessLayer;
using Common.Models;
using Microsoft.AspNetCore.Mvc.Razor;

namespace ASPNETIdentityPostgres
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //services.AddDbContext<ApplicationDbContext>(options =>
            //    options.UseSqlServer(
            //        Configuration.GetConnectionString("DefaultConnection")));

            // Add framework services.
            services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(Configuration.GetConnectionString("DefaultConnection")));

            services.AddDbContext<identitywithpgsqlContext>(options =>
            options.UseNpgsql(Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<ApplicationUser, ApplicationRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders()
                .AddDefaultUI();

            services.AddScoped<IPatientLogic, PatientLogic>();
            services.AddScoped<IPatientRepository, PatientRepositoryEFCore>();

            services.AddScoped<IUnnaturalDeathsLogic, UnnaturalDeathsLogic>();
            services.AddScoped<IUnnaturalDeathsRepository, UnnaturalDeathsRepositoryEFCore>();

            //services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
            //    .AddEntityFrameworkStores<ApplicationDbContext>();
            services.AddRazorPages().AddRazorRuntimeCompilation();

            #region snippet1    
            services.AddLocalization(options => options.ResourcesPath = "Resources");
            services.AddMvc()
                .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
                .AddDataAnnotationsLocalization();

            services.Configure<RequestLocalizationOptions>(options =>
            {
                var supportedCultures = new[] { "en-US", "dz-DZ" };
                options.SetDefaultCulture(supportedCultures[0])
                    .AddSupportedCultures(supportedCultures)
                    .AddSupportedUICultures(supportedCultures);
            });
            #endregion

            services.AddSingleton<IConfiguration>(Configuration);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            var supportedCultures = new[] { "en-US", "dz-DZ" };
            var localizationOptions = new RequestLocalizationOptions().SetDefaultCulture(supportedCultures[1])
                .AddSupportedCultures(supportedCultures)
                .AddSupportedUICultures(supportedCultures);

            app.UseRequestLocalization(localizationOptions);

            app.UseEndpoints(endpoints =>
            {
                
            });


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapAreaControllerRoute("identity_route", "Identity",
                    "{controller}/{action}/{id?}");
                endpoints.MapAreaControllerRoute("patients_route", "ADT",
                    "{controller}/{action}/{id?}");
                endpoints.MapAreaControllerRoute("unnaturaldeaths_route", "ADT",
                    "{controller}/{action}/{id?}");
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Patients}/{action=Index}/{id?}");
            });

            
        }
    }
}
