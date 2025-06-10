using My.Database.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace My.Database.Context;

public class MyDbContext : DbContext
{
    private readonly IConfiguration _config;

    public MyDbContext(DbContextOptions<MyDbContext> options, IConfiguration config) : base(options)
    {
        _config = config;
    }



    #region Methods

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // User ↔ UserActivation
        modelBuilder.Entity<User>()
            .HasOne(a => a.Activation)
            .WithOne(a => a.User)
            .HasForeignKey<UserActivation>(a => a.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Filename=MyDatabase.db");
    }

    #endregion



    #region Datasets

    public DbSet<User> Users { get; set; }
    public DbSet<UserActivation> UserActivations { get; set; }

    #endregion
}