using Microsoft.Extensions.Configuration;

namespace BarrierLayer.LibCandidates.Vault
{
    public static class ConfigExtensions
    {
        public const string Discovery = "Discovery";
        public const string Settings = "Settings";

        public static T GetDiscovery<T>(this IConfiguration configuration, string discoveryName)
            => configuration.GetSection(Discovery).GetSection(discoveryName).Get<T>();

        public static IConfiguration GetSetting(this IConfiguration configuration, string settingKey)
            => configuration.GetSection(Settings).GetSection(settingKey);
    }
}