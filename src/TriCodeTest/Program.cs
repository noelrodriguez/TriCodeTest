using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;

namespace TriCodeTest
{
    /// <summary>
    /// Program starts here
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Main function that bootstraps the project
        /// </summary>
        /// <param name="args">args</param>
        public static void Main(string[] args)
        {
            var host = new WebHostBuilder()
                .UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseIISIntegration()
                .UseStartup<Startup>()
                .Build();

            host.Run();
        }
    }
}
