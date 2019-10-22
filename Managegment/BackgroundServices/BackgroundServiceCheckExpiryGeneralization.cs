using Managegment.Services;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Managegment.BackgroundServices
{
    public class BackgroundServiceCheckExpiryGeneralization : BackgroundService
    {
        private Timer _timer;
        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            CheckExpiryGeneralizationService checkExpiryGeneralizationService = new CheckExpiryGeneralizationService();
            _timer = new Timer(async (e) => await checkExpiryGeneralizationService.CheckExpiryDateGeneralization(), null, TimeSpan.Zero, TimeSpan.FromMinutes(5));
            await Task.CompletedTask;
        }
        public async override Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Change(Timeout.Infinite, 0);
            await Task.CompletedTask;
        }
    }
}
