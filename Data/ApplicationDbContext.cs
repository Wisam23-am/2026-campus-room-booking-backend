using System;
using Microsoft.EntityFrameworkCore;
using _2026_campus_room_booking_backend.Enums;
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

    /// <summary>
    /// DbSet for rooms
    /// </summary>
    public DbSet<Room> Rooms { get; set; }

    /// <summary>
    /// DbSet for application users
    /// </summary>
    public DbSet<AppUser> Users { get; set; }

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

        modelBuilder.Entity<Room>(entity =>
        {
            entity.ToTable("Rooms");
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(120);

            entity.Property(e => e.Building)
                .IsRequired()
                .HasMaxLength(120);

            entity.Property(e => e.Category)
                .IsRequired()
                .HasMaxLength(80);

            entity.Property(e => e.Description)
                .HasMaxLength(500);

            entity.Property(e => e.Status)
                .IsRequired();

            entity.Property(e => e.CreatedAt)
                .IsRequired()
                .HasDefaultValueSql("datetime('now')");

            entity.Property(e => e.IsDeleted)
                .IsRequired()
                .HasDefaultValue(false);

            entity.HasIndex(e => e.IsDeleted);
            entity.HasIndex(e => e.Status);
            entity.HasIndex(e => new { e.Building, e.Floor });
        });

        modelBuilder.Entity<AppUser>(entity =>
        {
            entity.ToTable("Users");
            entity.HasKey(e => e.Id);

            entity.Property(e => e.FullName)
                .IsRequired()
                .HasMaxLength(120);

            entity.Property(e => e.Email)
                .IsRequired()
                .HasMaxLength(180);

            entity.HasIndex(e => e.Email)
                .IsUnique();

            entity.Property(e => e.Role)
                .IsRequired();

            entity.Property(e => e.CreatedAt)
                .IsRequired()
                .HasDefaultValueSql("datetime('now')");

            entity.Property(e => e.IsDeleted)
                .IsRequired()
                .HasDefaultValue(false);

            entity.HasIndex(e => e.IsDeleted);
            entity.HasIndex(e => e.Role);
        });

        // Minimal data seeding (Task 6 in modul/panduan-proyek.md)
        var seedCreatedAtUtc = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        modelBuilder.Entity<AppUser>().HasData(
            new AppUser
            {
                Id = 1,
                FullName = "Admin User",
                Email = "admin@campus.local",
                Role = UserRole.Admin,
                CreatedAt = seedCreatedAtUtc,
                IsDeleted = false
            },
            new AppUser
            {
                Id = 2,
                FullName = "Student User",
                Email = "student@campus.local",
                Role = UserRole.User,
                CreatedAt = seedCreatedAtUtc,
                IsDeleted = false
            }
        );

        modelBuilder.Entity<Room>().HasData(
            new Room
            {
                Id = 1,
                Name = "Lecture Hall A",
                Building = "Main Building",
                Floor = 1,
                Capacity = 120,
                Category = "Lecture Hall",
                Description = "Large lecture hall with projector",
                Status = RoomStatus.Active,
                CreatedAt = seedCreatedAtUtc,
                IsDeleted = false
            },
            new Room
            {
                Id = 2,
                Name = "Computer Lab 304",
                Building = "Engineering Hall",
                Floor = 3,
                Capacity = 30,
                Category = "Lab",
                Description = "Computer lab for practical sessions",
                Status = RoomStatus.Active,
                CreatedAt = seedCreatedAtUtc,
                IsDeleted = false
            }
        );
    }
}
