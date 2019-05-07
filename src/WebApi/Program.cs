using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace WebApi
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            await WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .Build()
                .RunAsync();
        }
 
    }
}