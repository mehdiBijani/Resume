using KafkaOrderDemo.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace KafkaOrderDemo.Infrastructure.Persistence;

public class ApplicationDbContext : DbContext
{
    public DbSet<Order> Orders => Set<Order>();

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfiguration(new OrderEntityConfiguration());
    }
}