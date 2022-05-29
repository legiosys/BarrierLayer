using System;
using System.Net;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using VaultSharp;
using VaultSharp.Core;

namespace BarrierLayer.LibCandidates.Vault
{
    public static class VaultClientExtensions
    {
        public static async Task<JObject> GetSecret(this IVaultClient vaultClient, ISecretDefinition secretDefinition,
            string mountPoint)
        {
            try
            {
                var secret =
                    await vaultClient.V1.Secrets.KeyValue.V2.ReadSecretAsync(secretDefinition.Key, null, mountPoint);
                return JObject.FromObject(secret.Data.Data);
            }
            catch (VaultApiException e)
            {
                if (e.HttpStatusCode == HttpStatusCode.NotFound)
                    return JObject.FromObject(secretDefinition.DefaultValue ?? throw new ArgumentNullException(
                        secretDefinition.Type.Name,
                        $"Не найден {secretDefinition.Type.Name} c ключом '{mountPoint + "/" + secretDefinition.Key}'"));
                throw;
            }
        }
    }
}