using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace BarrierLayer.Models
{
    public static class DbExtensions
    {
        public static void AddOrUpdate<T>(this DbSet<T> dbSet, T entity, Expression<Func<T, bool>> expression) where T : class
        {
            var exist = dbSet.AsNoTracking().FirstOrDefault(expression);
            if (exist == null)
                dbSet.Add(entity);
            else
            {
                dbSet.Remove(exist);
                dbSet.Add(entity);
            }
                
        }

        public static async Task<User> GetUserByToken(this DomainContext db, Guid token)
        {
            return await db.Users.Include(y => y.Barriers).FirstOrDefaultAsync(u => u.Token == token);
        }
        public static async Task<User> GetUserByNumber(this DomainContext db, string number)
        {
            return await db.Users.Include(y => y.Barriers).FirstOrDefaultAsync(u => u.Number == number);
        }

        public static async Task<Barrier> GetBarrierById(this DomainContext db, int id)
        {
            return await db.Barriers.FirstOrDefaultAsync(b => b.Id == id);
        }
    }
}
