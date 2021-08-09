using Microsoft.AspNetCore.Hosting;

[assembly: HostingStartup(typeof(ASPNETIdentityPostgres.Areas.Identity.IdentityHostingStartup))]
namespace ASPNETIdentityPostgres.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
            });
        }
    }
}