using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public class DataContext : DbContext
{

    public DataContext(DbContextOptions<DataContext> options) : base(options) { }

    public DbSet<User> Users { get; set; }
    public DbSet<Workout> Workouts { get; set; }
    public DbSet<Trainer> Trainers { get; set; }
    public DbSet<Membership> Memberships { get; set; }
    public DbSet<ClassSchedule> ClassSchedules { get; set; }
    public DbSet<Payment> Payments { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Membership>()
            .HasOne(m => m.User)
            .WithMany(u => u.Memberships)
            .HasForeignKey(m => m.UserId);

        modelBuilder.Entity<ClassSchedule>()
            .HasOne(cs => cs.Trainer)
            .WithMany(t => t.ClassSchedules)
            .HasForeignKey(cs => cs.TrainerId);

        modelBuilder.Entity<ClassSchedule>()
            .HasOne(cs => cs.Workout)
            .WithMany(w => w.ClassSchedules)
            .HasForeignKey(cs => cs.WorkoutId);

        modelBuilder.Entity<Payment>()
            .HasOne(p => p.User)
            .WithMany(u => u.Payments)
            .HasForeignKey(p => p.UserId);
    }
}