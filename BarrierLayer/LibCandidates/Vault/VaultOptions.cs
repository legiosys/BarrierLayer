using Microsoft.Extensions.Hosting;
using VaultSharp;
using VaultSharp.V1.AuthMethods.UserPass;

namespace BarrierLayer.LibCandidates.Vault
{
    public record VaultOptions
    {
        public VaultClientSettings ClientSettings { get; init; }
        public ISecretsManager SecretsManager { get; init; }
        public string EnvironmentName { get; init; }
        public int? ReloadInterval { get; init; }
        public string DiscoveryMount { get; init; }
        public string SettingsMount { get; init; }

        public static VaultOptions LoginPassDefault(IHostEnvironment environment, ISecretsManager secretsManager)
        {
            return new VaultOptions()
            {
                ClientSettings = new VaultClientSettings(secretsManager.Configuration["vaultUrl"],
                    new UserPassAuthMethodInfo(secretsManager.Configuration["vaultUserName"],
                        secretsManager.Configuration["vaultPassword"])),
                EnvironmentName = environment.EnvironmentName,
                ReloadInterval = 60000,
                SecretsManager = secretsManager,
                DiscoveryMount = "services",
                SettingsMount = "kv"
            };
        }
    }

    public interface IDiscovery
    {
    }

    public interface ISetting
    {
    }
}