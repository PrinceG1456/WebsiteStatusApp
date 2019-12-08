using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;

namespace WebsiteStatus
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // To start using Serilog
            // Steps to override the Logger that is built into ASPNET Core.
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .Enrich.FromLogContext()
                .WriteTo.File(@"C:\temp\workerservice\LogFile.txt")
                .CreateLogger();

            try
            {
                Log.Information("Starting up the service");
                CreateHostBuilder(args).Build().Run();

            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "There was rpoblem starting the service");

                return;
            }
            finally
            {
                // Ensure that if there are any messages in the buffers
                // It gets written out of the application.
                // Logger ususally writes message in a group they don't immediately write it
                // If youare half way through the process and you close the application intentionally by shutting it down
                // or unintentionally by crashing it.
                // Therfore, we don't want to lose the messages.
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                // to run as a Windows service
                .UseWindowsService()
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddHostedService<Worker>();
                })
                .UseSerilog();
    }
}
