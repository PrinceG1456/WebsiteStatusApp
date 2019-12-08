using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Net.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace WebsiteStatus
{

    // Worker Service is a service that runs in the background forever
    // In windows - Services that run in background
    // In Linux they are Deamon
    // today we are creating a service to monitor our website to monitor that it is up
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private HttpClient client;

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
        }


        public override Task StartAsync(CancellationToken cancellationToken)
        {
            client = new HttpClient();
            return base.StartAsync(cancellationToken);
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            client.Dispose();
            _logger.LogInformation("Ther Service has been stopped...");
            return base.StopAsync(cancellationToken);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var result = await client.GetAsync("https://www.youtube.com");
                if (result.IsSuccessStatusCode)
                {
                    _logger.LogInformation("The website is up. Status Code {StatusCode}", result.StatusCode);
                }
                else
                {
                    // This is not the right way, rather you can have is DBLogging.
                    // And also you can send a Email saying the Website is down.
                    // Or a text Messsage
                    _logger.LogError("The Wenbsite is down. Status code {StatusCode}", result.StatusCode);
                }
;                await Task.Delay(5000, stoppingToken);
            }
        }
    }
}
