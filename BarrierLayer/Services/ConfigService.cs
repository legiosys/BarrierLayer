using System.Security.Authentication;
using System.Threading.Tasks;
using System.Web.Helpers;
using BarrierLayer.Db;
using BarrierLayer.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace BarrierLayer.Services
{
    public class ConfigService(DomainContext db)
    {
        public async Task SetMasterPassword(string newPassword, string oldPassword)
        {
            var master = await db.Settings.FirstOrDefaultAsync(x => x.Key.Equals("MasterPassword"));
            if (!string.IsNullOrWhiteSpace(master?.Value))
            {
                if (oldPassword == null || !Crypto.VerifyHashedPassword(master.Value, oldPassword))
                {
                    throw new InvalidCredentialException("Старый пароль не совпадает с введенным");
                }

                master.Value = Crypto.HashPassword(newPassword);
                db.Update(master);
            }
            else
            {
                master = new Configuration()
                {
                    Key = "MasterPassword",
                    Value = Crypto.HashPassword(newPassword),
                    Format = "string/password"
                };
                db.Add(master);
            }

            await db.SaveChangesAsync();
        }

        public async Task<string> GetMasterPassword()
        {
            return await GetValue("MasterPassword");
        }

        public async Task VerifyPassword(string password)
        {
            if (!Crypto.VerifyHashedPassword(await GetValue("MasterPassword"), password))
            {
                throw new InvalidCredentialException("Пароль не подходит");
            }
        }

        public async Task<string> GetValue(string key)
        {
            return (await db.Settings.FirstOrDefaultAsync(x => x.Key.Equals(key)))?.Value;
        }

        public async Task<string> GetValue(string key, string format)
        {
            return (await db.Settings.FirstOrDefaultAsync(x => x.Key.Equals(key) && x.Format.Contains(format)))?.Value;
        }

        public async Task SetValue(string key, string value, string format, string password)
        {
            await VerifyPassword(password);
            var property = new Configuration()
            {
                Key = key,
                Value = value,
                Format = format
            };
            db.Settings.AddOrUpdate(property, x => x.Key.Equals(property.Key));
            await db.SaveChangesAsync();
        }
    }
}