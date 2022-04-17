using Lab4.Models;
using Microsoft.EntityFrameworkCore;

namespace Lab4.Data
{
    public class MarketDbContext : DbContext
    {
        public MarketDbContext(DbContextOptions<MarketDbContext> options) : base(options)
        {
            

        }

        public DbSet<Client> Clients { get; set; }
        public DbSet<Brokerage> Brokerages { get; set; }
        public DbSet<Subscription> Subscriptions { get; set; }
        public DbSet<Advertisement> Advertisements { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Client>().ToTable("clients");
            modelBuilder.Entity<Brokerage>().ToTable("brokers");
            modelBuilder.Entity<Advertisement>().ToTable("advertisements"); 
            modelBuilder.Entity<Subscription>()
                .ToTable("subscriptions")
                .HasKey(c => new { c.ClientId, c.BrokerageId });

        }

    }
}
