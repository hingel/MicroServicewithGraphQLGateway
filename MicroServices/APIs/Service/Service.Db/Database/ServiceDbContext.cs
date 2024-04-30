using Microsoft.EntityFrameworkCore;
using Service.Db.Model;

namespace Service.Db.Database;

public class ServiceDbContext(DbContextOptions<ServiceDbContext> options) : DbContext(options)
{
    public DbSet<ServiceModel> ServiceModels { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<ServiceModel>().HasKey(s => s.Id);
        modelBuilder.Entity<ServiceModel>().Property(p => p.Name).HasMaxLength(50).IsRequired();
        modelBuilder.Entity<ServiceModel>().Property(p => p.Description).HasMaxLength(100).IsRequired();
    }
}