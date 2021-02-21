using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MRHomePage.Service
{
    public class PriceTrackerService : IHostedService, IDisposable
    {
        private int executionCount = 0;
        private readonly NLog.Logger logPriceTracker = NLog.LogManager.GetCurrentClassLogger();
        private Timer _timer;
        private int runEverySeconds = 60; //1 min

        public PriceTrackerService()
        {
        }

        public Task StartAsync(CancellationToken stoppingToken)
        {
            logPriceTracker.Trace("Timed Hosted Service running.");

            _timer = new Timer(DoWork, null, TimeSpan.Zero,
                TimeSpan.FromSeconds(runEverySeconds));

            return Task.CompletedTask;
        }

        private void DoWork(object state)
        {
            try
            {
                var count = Interlocked.Increment(ref executionCount);

                logPriceTracker.Trace("Price tracker starting. Count: {Count}", count);
                Controllers.PriceDashboardController.GetDataWeb();
                logPriceTracker.Trace("Price tracker finish with success");
            }
            catch (Exception ex)
            {
                logPriceTracker.Error($"Error: {ex.Message}", ex.StackTrace);
            }
        }

        public Task StopAsync(CancellationToken stoppingToken)
        {
            logPriceTracker.Trace("Timed Hosted Service is stopping.");

            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    } 
}