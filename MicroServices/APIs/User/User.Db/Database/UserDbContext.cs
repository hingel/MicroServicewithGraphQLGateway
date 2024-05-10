using Microsoft.EntityFrameworkCore;

namespace User.Db.Database;

public class UserDbContext(DbContextOptions<UserDbContext> options) : DbContext(options)
{
    public DbSet<Model.User> Users { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Model.User>().HasKey(u => u.Id);
        modelBuilder.Entity<Model.User>().Property(p => p.Name).HasMaxLength(100).IsRequired();
        modelBuilder.Entity<Model.User>().Property(p => p.ServiceModelId);
        modelBuilder.Entity<Model.User>().OwnsOne(u => u.Address, u =>
        {
            u.Property(p => p.City).HasMaxLength(100);
            u.Property(p => p.Country).HasMaxLength(100);
            u.Property(p => p.Street).HasMaxLength(100);
            u.Property(p => p.PostalCode).HasMaxLength(50);
        });
    }
}