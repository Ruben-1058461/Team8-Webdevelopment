using Microsoft.EntityFrameworkCore;

public class Database : DbContext
{
    public Database(DbContextOptions<Database> options)
        : base(options)
    {
    }

    public DbSet<Admin> Admins { get; set; }
    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Admin>().ToTable("admin");
        modelBuilder.Entity<User>().ToTable("user");
    }
}
