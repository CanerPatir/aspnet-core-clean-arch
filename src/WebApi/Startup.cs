using Infrastructure;
using Infrastructure.AspNet;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddInfrastructure()
                .AddAspNetInfrastructure();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseAspNetInfrastructure(env, Configuration);
        }
    }
}