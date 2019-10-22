using Managegment.Services;
 
using System;
using Microsoft.Extensions.Hosting;
using System.Threading;
using System.Threading.Tasks;

namespace Managegment.BackgroundServicesLog
{
    public class SendSMSAndEmailService : BackgroundService
    {
        private Timer _timer;
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            
            ServiceSend serviceSend = new ServiceSend();
            _timer = new Timer(async (e)=>await serviceSend.SendMessageToEmailAndSMS(), null, TimeSpan.Zero, TimeSpan.FromMinutes(2));
             await Task.CompletedTask;
        }

        public async override Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Change(Timeout.Infinite, 0);
            await Task.CompletedTask;

        }
    }
}
