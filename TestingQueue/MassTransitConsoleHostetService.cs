﻿using MassTransit;
using Microsoft.Extensions.Hosting;
using System.Threading;
using System.Threading.Tasks;

namespace TestingQueue
{
    public class MassTransitConsoleHostetService : IHostedService
    {
        private readonly IBusControl _busControl;
        public MassTransitConsoleHostetService(IBusControl busControl)
        {
            _busControl = busControl;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await _busControl.StartAsync(cancellationToken);
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await _busControl.StopAsync(cancellationToken);
        }
    }
}
