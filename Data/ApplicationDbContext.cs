using Microsoft.EntityFrameworkCore;
using _2026_campus_room_booking_backend.Models;

namespace _2026_campus_room_booking_backend.Data;

/// <summary>
/// Application database context for managing room booking entities
/// </summary>
public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    /// <summary>
    /// DbSet for room bookings
    /// </summary>
    public DbSet<RoomBooking> RoomBookings { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<RoomBooking>(entity =>
        {
            // Configure table name
            entity.ToTable("RoomBookings");

            // Configure primary key
            entity.HasKey(e => e.Id);

            // Configure required fields
            entity.Property(e => e.RoomName)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(e => e.BookedBy)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(e => e.Purpose)
                .HasMaxLength(500);

            entity.Property(e => e.StartTime)
                .IsRequired();

            entity.Property(e => e.EndTime)
                .IsRequired();

            entity.Property(e => e.Status)
                .IsRequired();

            entity.Property(e => e.CreatedAt)
                .IsRequired()
                .HasDefaultValueSql("datetime('now')");

            entity.Property(e => e.IsDeleted)
                .IsRequired()
                .HasDefaultValue(false);

            // Add index for better query performance
            entity.HasIndex(e => e.IsDeleted);
            entity.HasIndex(e => e.Status);
            entity.HasIndex(e => e.StartTime);
        });
    }
}
