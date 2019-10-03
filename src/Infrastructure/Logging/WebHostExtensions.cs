using System.Reflection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.Graylog;
using Serilog.Sinks.Graylog.Core.Helpers;
using Serilog.Sinks.Graylog.Core.Transport;

namespace Infrastructure.Logging
{
    public static class WebHostExtensions
    {
        public static IWebHostBuilder ConfigureLoggingPlumbing(this IWebHostBuilder webHostBuilder)
        {
            return webHostBuilder.UseSerilog((context, loggerConfiguration) =>
            {
                var logLevel = context.Configuration.GetValue<LogEventLevel>("Serilog:MinimumLevel:Default");
                loggerConfiguration
                    .MinimumLevel.Verbose()
                    .Enrich.FromLogContext()
                    .Enrich.WithProperty("version",
                        Assembly.GetExecutingAssembly().GetCustomAttribute<AssemblyFileVersionAttribute>().Version)
                    .Enrich.WithProperty("env", context.HostingEnvironment.EnvironmentName)
                    .ReadFrom.Configuration(context.Configuration)
                    .WriteTo.Graylog(
                        new GraylogSinkOptions
                        {
                            HostnameOrAddress = context.Configuration["Serilog:GraylogIp"],
                            Port = int.Parse(context.Configuration["Serilog:GraylogPort"]),
                            TransportType = TransportType.Udp,
                            MinimumLogEventLevel = logLevel,
                            Facility = context.Configuration["Serilog:FacilityName"],
                            MessageGeneratorType = MessageIdGeneratortype.Timestamp
                        });

                if (context.Configuration.GetValue<bool>("Serilog:Console"))
                {
                    loggerConfiguration.WriteTo.ColoredConsole(logLevel,
                        "[{Timestamp:yyyy-MM-dd HH:mm:ss} {Level:u3}] {SourceContext}: {Message:lj}{NewLine}{Exception}");
                }
            });
        }
    }
}