using BarrierLayer.LibCandidates.Vault;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace BarrierLayer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Host.CreateDefaultBuilder(args)
                .AddVaultToConfiguration(new BarrierSecrets())
                .ConfigureWebHostDefaults(webBuilder => webBuilder.UseStartup<Startup>())
                .Build()
                .Run();
        }
    }
}