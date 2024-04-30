using Microsoft.EntityFrameworkCore;

namespace User.Db.Database;

public class UserDbContext(DbContextOptions<UserDbContext> options) : DbContext(options)
{
    public DbSet<Model.User> Users { get; set; } = null!;
    public DbSet<Model.Address> Addresses { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Model.User>().HasKey(u => u.Id);
        modelBuilder.Entity<Model.User>().Property(p => p.Name).HasMaxLength(100).IsRequired();
        modelBuilder.Entity<Model.User>().Property(p => p.ServiceModelIds);
        modelBuilder.Entity<Model.User>().HasOne(u => u.Address).WithMany();

        modelBuilder.Entity<Model.Address>().HasKey(u => u.Id);
        modelBuilder.Entity<Model.Address>().Property(p => p.Street).HasMaxLength(50).IsRequired();
        modelBuilder.Entity<Model.Address>().Property(p => p.City).HasMaxLength(50).IsRequired();
        modelBuilder.Entity<Model.Address>().Property(p => p.PostalCode).HasMaxLength(50).IsRequired();
        modelBuilder.Entity<Model.Address>().Property(p => p.Country).HasMaxLength(50).IsRequired();
    }
}