using Microsoft.EntityFrameworkCore;

namespace ToneTracker;

public class ToneTrackerDbContext : DbContext
{
    public ToneTrackerDbContext(DbContextOptions<ToneTrackerDbContext> options) : base(options) { }

    public DbSet<Pedal.Pedal> Pedals => Set<Pedal.Pedal>();
    public DbSet<Dial.Dial> Dials => Set<Dial.Dial>();
    public DbSet<Toggle.Toggle> Toggles => Set<Toggle.Toggle>();
    public DbSet<Setting> Settings => Set<Setting>();
    public DbSet<Amplifier.Amplifier> Amplifiers => Set<Amplifier.Amplifier>();
    public DbSet<Tone.Tone> Tones => Set<Tone.Tone>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Pedal.Pedal>()
            .HasMany(p => p.Dials)
            .WithOne(d => d.Pedal)
            .HasForeignKey(d => d.PedalId);

        modelBuilder.Entity<Pedal.Pedal>()
            .HasMany(p => p.Toggles)
            .WithOne(t => t.Pedal)
            .HasForeignKey(t => t.PedalId);
            
        modelBuilder.Entity<Setting>()
            .HasOne(s => s.Dial)
            .WithMany(d => d.Settings)
            .HasForeignKey(s => s.DialId)
            .IsRequired(false);

        modelBuilder.Entity<Setting>()
            .HasOne(s => s.Toggle)
            .WithMany(t => t.Settings)
            .HasForeignKey(s => s.ToggleId)
            .IsRequired(false);
        
    }
}