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

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var result = await client.GetAsync("https://www.iamtimcorey.com");
;                await Task.Delay(5000, stoppingToken);
            }
        }
    }
}
