using Microsoft.EntityFrameworkCore;

namespace AplikacjaBazodanowa;

public class AppDbContext : DbContext
{
    public DbSet<Sensor> Sensors { get; set; }
    public DbSet<Measurement> Measurements { get; set; }

    public AppDbContext()
    {
        Database.EnsureCreated();
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        options.UseSqlite("Data Source=purpleair.db");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Sensor>()
            .HasMany(s => s.Measurements)
            .WithOne(m => m.Sensor)
            .HasForeignKey(m => m.SensorId);
    }
}