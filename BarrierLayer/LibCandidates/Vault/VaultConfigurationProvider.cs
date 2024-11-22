using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using VaultSharp;

namespace BarrierLayer.LibCandidates.Vault
{
    public class VaultConfigurationProvider : ConfigurationProvider
    {
        private readonly IVaultClient _vaultClient;
        private readonly VaultOptions _options;
        public int? ReloadInterval => _options.ReloadInterval;

        public VaultConfigurationProvider(VaultOptions vaultOptions)
        {
            _vaultClient = new VaultClient(vaultOptions.ClientSettings);
            _options = vaultOptions;
        }

        public override void Load()
        {
            LoadAsync(false).GetAwaiter().GetResult();
        }

        public async Task LoadAsync(bool reload)
        {
            var secretsManager = _options.SecretsManager;
            var sw = Stopwatch.StartNew();
            var loadDiscoveryTasks = secretsManager.Discoveries
                .ToDictionary(
                    k => k.Key,
                    v => _vaultClient.GetSecret(v, _options.DiscoveryMount));

            var loadSettingsTasks = secretsManager.Settings
                .ToDictionary(
                    k => k.Key,
                    v => _vaultClient.GetSecret(v, _options.SettingsMount));
            try
            {
                await Task.WhenAll(
                    loadDiscoveryTasks
                        .Select(x => x.Value as Task)
                        .Union(
                            loadSettingsTasks
                                .Select(x => x.Value))
                        .ToArray());
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                if (reload)
                    return;
                throw;
            }


            foreach (var discovery in loadDiscoveryTasks)
            {
                var data = JsonConfigurationParser.Parse(discovery.Value.Result,
                    ConfigurationPath.Combine(ConfigExtensions.Discovery, discovery.Key));
                SetRange(data);
            }

            foreach (var settings in loadSettingsTasks)
            {
                var data = JsonConfigurationParser.Parse(settings.Value.Result,
                    ConfigurationPath.Combine(ConfigExtensions.Settings, settings.Key));
                SetRange(data);
            }

            sw.Stop();
#if DEBUG
            Console.WriteLine($"Vault secrets loaded. Elapsed: {sw.ElapsedMilliseconds} ms");
#endif
            OnReload();
        }

        private void SetRange(IDictionary<string, string> elements)
        {
            foreach (var (key, value) in elements)
                Set(key, value);
        }
    }
}