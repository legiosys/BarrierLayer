using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BarrierLayer.LibCandidates.Vault
{
    public class BarrierSecrets : ISecretsManager
    {
        public IConfiguration Configuration { get; private set; }

        public List<ISecretDefinition> Discoveries => new()
        {
            new SecretDefinition<DbSettings>("postgres")
        };

        public List<ISecretDefinition> Settings => new()
        {
            new SecretDefinition<DbSettings>("services/postgre").Default(new DbSettings()),
            new SecretDefinition<BarrierSettings>("barrier").RegisterAsOption()
        };

        public void AttachConfiguration(IConfiguration configuration)
        {
            Configuration = configuration;
        }
    }

    public interface ISecretsManager
    {
        IConfiguration Configuration { get; }
        List<ISecretDefinition> Discoveries { get; }
        List<ISecretDefinition> Settings { get; }

        void AttachConfiguration(IConfiguration configuration);

        void RegisterSettings(IServiceCollection services)
            => Settings.ForEach(secret => secret.RegisterOption(services, Configuration, this));
    }

    public class BarrierSettings
    {
        public string SomeSecret { get; init; }
    }

    public interface ISecretDefinition
    {
        string Key { get; }
        Type Type { get; }
        object DefaultValue { get; }
        bool AsOption { get; }

        void RegisterOption(IServiceCollection services, IConfiguration config, ISecretsManager secretsManager);
    }

    public record SecretDefinition<T>(string Key) : ISecretDefinition where T : class
    {
        public object DefaultValue { get; init; } = null;
        public bool AsOption { get; init; } = false;
        public Type Type => typeof(T);

        public void RegisterOption(IServiceCollection services, IConfiguration config, ISecretsManager secretsManager)
        {
            if (!AsOption) return;
            if (secretsManager.Settings.OfType<T>().Count() > 1)
                services.Configure<T>(Key, config.GetSetting(Key));
            else
                services.Configure<T>(config.GetSetting(Key));
        }

        public SecretDefinition<T> Default(object defaultValue) => this with {DefaultValue = defaultValue};
        public SecretDefinition<T> RegisterAsOption(bool asOption = true) => this with {AsOption = asOption};
    }
}