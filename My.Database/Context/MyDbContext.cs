using My.Database.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace My.Database.Context;

public class MyDbContext : DbContext
{
    private readonly IConfiguration _config;

    public MyDbContext(DbContextOptions<MyDbContext> options) : base(options)
    {
    }


    #region Methods

    protected override void OnConfiguring(DbContextOptionsBuilder options)
{
    if (!options.IsConfigured)
    {
        options.UseSqlite("Data Source=app.db");
    }
}


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // User ↔ UserActivation
        modelBuilder.Entity<User>()
            .HasOne(a => a.Activation)
            .WithOne(a => a.User)
            .HasForeignKey<UserActivation>(a => a.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        // Seed Test Data
        modelBuilder.Entity<User>().HasData(
            new User
            {
                Id = 1,
        Email = "admin@example.com",
        FirstName = "Admin",
        LastName = "User",
        Role = UserRole.Admin,
        CreatedAt = new DateTime(2025, 01, 01, 0, 0, 0, DateTimeKind.Utc)
            }
        );

        modelBuilder.Entity<UserActivation>().HasData(
            new UserActivation
            {
                UserId = 1,
                ActivationToken = "test-token-123",
                ActivationTokenExpiration = new DateTime(2025, 01, 08, 0, 0, 0, DateTimeKind.Utc)
            }
        );
    }



    #endregion



    #region Datasets

    public DbSet<User> Users { get; set; }
    public DbSet<UserActivation> UserActivations { get; set; }

    #endregion
}