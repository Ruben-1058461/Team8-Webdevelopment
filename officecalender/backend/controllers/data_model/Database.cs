using Microsoft.EntityFrameworkCore;

public class Database : DbContext
{
    public Database(DbContextOptions<Database> options)
        : base(options)
    {
    }

    public DbSet<Admin> Admins { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Event> Events { get; set; }
    public DbSet<Attendance> Attendances { get; set; }
    public DbSet<Event_Attendance> Event_Attendances { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Admin>().ToTable("admin");
        modelBuilder.Entity<User>().ToTable("user");
        modelBuilder.Entity<Event>().ToTable("event_data");
        modelBuilder.Entity<Attendance>().ToTable("attendance_data");
        modelBuilder.Entity<Event_Attendance>().ToTable("event_attendance_data");
    }
}
