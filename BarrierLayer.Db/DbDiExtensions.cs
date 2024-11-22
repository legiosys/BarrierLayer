using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;

namespace BarrierLayer.Db;

public static class DbDiExtensions
{
    public static void AddDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        var sourceBuilder = new NpgsqlDataSourceBuilder(configuration.GetConnectionString("Barrier"));
        sourceBuilder.UseJsonNet();
        var source = sourceBuilder.Build();
        services.AddDbContext<DomainContext>(options => { options.UseNpgsql(source); });
    }
}