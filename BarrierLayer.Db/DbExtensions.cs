using System.Linq.Expressions;
using BarrierLayer.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Barrier = BarrierLayer.Domain.Models.Barrier;

namespace BarrierLayer.Db
{
    public static class DbExtensions
    {
        public static void AddOrUpdate<T>(this DbSet<T> dbSet, T entity, Expression<Func<T, bool>> expression)
            where T : class
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

        public static async Task<List<Barrier>> GetUserBarriers(this DomainContext db, Guid token)
        {
            var user = await db.GetUserByToken(token);
            if (user?.Barriers == null) return new List<Barrier>();
            var barrierIds = user.Barriers.Select(x => x.BarrierId).ToList();
            return await db.Barriers.Where(b => barrierIds.Contains(b.Id)).ToListAsync();
        }

        public static async Task<User> GetUserByNumber(this DomainContext db, string number)
        {
            return await db.Users.Include(y => y.Barriers).FirstOrDefaultAsync(u => u.Number == number);
        }

        public static async Task<Barrier> GetBarrierById(this DomainContext db, int id)
        {
            return await db.Barriers.FirstOrDefaultAsync(b => b.Id == id);
        }

        public static Task<Guest> Guest_ById(this DomainContext db, Guid id)
            => db.Guests.Include(x => x.Barrier).FirstOrDefaultAsync(x => x.Id == id);
    }
}