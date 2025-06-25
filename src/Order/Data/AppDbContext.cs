using Microsoft.EntityFrameworkCore;
using Order.Domain;

namespace Order.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<OrderEntity> Orders { get; set; }
    public DbSet<Customer> Customers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
           => modelBuilder.Entity<OrderEntity>().OwnsMany(o => o.Items);
}
