using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace WebApi.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseAspNetInfrastructure(this IApplicationBuilder app,
            IHostingEnvironment hostingEnvironment,
            IConfiguration configuration)
        {
            return app.UseResponseCompression()
                .UseApplicationHeaders(hostingEnvironment, configuration)
                .UseSwaggerConf()
                .UseMvc()
                .UseHsts();
        }
        
        public static IApplicationBuilder UseSwaggerConf(this IApplicationBuilder app)
        {
            return app
                .UseSwagger()
                .UseSwaggerUI(c =>
                {
                    c.RoutePrefix = "";
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "PimReadProjector API Swagger v1");
                });
        }

        public static IApplicationBuilder UseApplicationHeaders(this IApplicationBuilder app,
            IHostingEnvironment hostingEnvironment,
            IConfiguration configuration)
        {
            var appVersion = configuration["APP_VERSION"] ??
                             Assembly.GetExecutingAssembly().GetCustomAttribute<AssemblyFileVersionAttribute>().Version;

            return app.Use(async (context, func) =>
            {
                context.Response.OnStarting(state =>
                {
                    var httpContext = (HttpContext) state;

                    httpContext.Response.Headers.Add("X-Environment", new[] {hostingEnvironment.EnvironmentName});
                    httpContext.Response.Headers.Add("X-Version", new[] {appVersion});
                    return Task.FromResult(0);
                }, context);

                await func.Invoke();
            });
        }
    }
}