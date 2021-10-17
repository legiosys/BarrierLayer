using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BarrierLayer.Models
{
    public class DomainContext : DbContext
    {
        public DbSet<Barrier> Barriers { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Configuration> Settings { get; set; }
        public DbSet<UserBarrier> UserBarriers { get; set; }
        public DbSet<Guest> Guests { get; set; }
        public DomainContext(DbContextOptions<DomainContext> options)
           : base(options)
        {
            //Database.EnsureDeleted();
            Database.Migrate();
        }
        protected override void OnModelCreating(ModelBuilder model)
        {
            model.Entity<UserBarrier>()
                .HasKey(t => new { t.BarrierId, t.UserId });

        }
    }
}
