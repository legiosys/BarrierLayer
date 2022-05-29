using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace BarrierLayer.LibCandidates.Vault
{
    public class VaultConfigurationSource : IConfigurationSource
    {
        private readonly VaultOptions _vaultSettings;

        public VaultConfigurationSource(VaultOptions vaultSettings)
        {
            _vaultSettings = vaultSettings;
        }

        public IConfigurationProvider Build(IConfigurationBuilder builder)
            => new VaultConfigurationProvider(_vaultSettings);
    }

    public static class VaultExtensions
    {
        public static IHostBuilder AddVaultToConfiguration(this IHostBuilder builder, ISecretsManager secretsManager)
            => builder
                .ConfigureAppConfiguration((context, configuration) =>
                {
                    secretsManager.AttachConfiguration(configuration.Build());
                    var vaultSource = new VaultConfigurationSource(
                        VaultOptions.LoginPassDefault(context.HostingEnvironment, secretsManager));
                    configuration.Sources.Insert(1, vaultSource);
                })
                .ConfigureServices((context, services) =>
                {
                    secretsManager.AttachConfiguration(context.Configuration);
                    services.AddSingleton(secretsManager);
                    services.AddHostedService(_ => new VaultChangeWatcher(context.Configuration as IConfigurationRoot));
                    secretsManager.RegisterSettings(services);
                });
    }
}