using BarrierLayer.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Barrier = BarrierLayer.Domain.Models.Barrier;

namespace BarrierLayer.Db
{
    public class DomainContext(DbContextOptions<DomainContext> options) : DbContext(options)
    {
        public DbSet<Barrier> Barriers { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Configuration> Settings { get; set; }
        public DbSet<UserBarrier> UserBarriers { get; set; }

        public DbSet<Guest> Guests { get; set; }
        //Database.EnsureDeleted();
        //Database.Migrate();

        protected override void OnModelCreating(ModelBuilder model)
        {
            model.Entity<UserBarrier>()
                .HasKey(t => new {t.BarrierId, t.UserId});
        }
    }
}