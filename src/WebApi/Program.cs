
using System.Threading.Tasks;
using Infrastructure;
using Infrastructure.Logging;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace WebApi
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            await WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .ConfigureLoggingPlumbing()
                .Build()
                .RunAsync();
        }
 
    }
}