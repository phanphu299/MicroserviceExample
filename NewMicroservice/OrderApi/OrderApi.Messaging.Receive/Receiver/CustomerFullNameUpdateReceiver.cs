
namespace OrderApi.Messaging.Receive.Receiver
{
    using Microsoft.Extensions.Hosting;
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;

    public class CustomerFullNameUpdateReceiver : BackgroundService
    {
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            throw new NotImplementedException();
        }
    }
}
