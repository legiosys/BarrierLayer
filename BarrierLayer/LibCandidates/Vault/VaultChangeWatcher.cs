using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace BarrierLayer.LibCandidates.Vault
{
    public class VaultChangeWatcher : BackgroundService
    {
        private readonly List<VaultConfigurationProvider> _vaultProviders;

        public VaultChangeWatcher(IConfigurationRoot configurationRoot)
        {
            _vaultProviders = configurationRoot.Providers.OfType<VaultConfigurationProvider>()
                .Where(p => p.ReloadInterval.HasValue)
                .ToList();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var delay = _vaultProviders.Min(x => x.ReloadInterval);
            if (!delay.HasValue) return;
            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(delay.Value, stoppingToken);
                await Task.WhenAll(_vaultProviders.Select(p => p.LoadAsync(true)));
            }
        }
    }
}