using BarrierLayer.LibCandidates.Vault;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;

namespace BarrierLayer.LibCandidates
{
    public static class DbConnectExtensions
    {
        public static void AddPostgres<T>(this IServiceCollection services, IConfiguration config, string discoveryName)
            where T : DbContext
        {
            var dbSettings = config.GetDiscovery<DbSettings>(discoveryName);

            var connection = new NpgsqlConnectionStringBuilder
            {
                Host = dbSettings.Url,
                Port = dbSettings.Port,
                Username = dbSettings.Login,
                Password = dbSettings.Password,
                Database = config["dbName"],
                PersistSecurityInfo = true
            };
            services.AddDbContext<T>(opt => { opt.UseNpgsql(connection.ConnectionString); });
        }
    }
}