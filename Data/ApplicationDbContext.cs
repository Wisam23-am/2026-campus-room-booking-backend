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

            entity.Property(e => e.Password)
                .IsRequired()
                .HasMaxLength(255);

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
        // Default password for all users: "Password123!" (hashed using BCrypt for demo)
        // Production: implement proper password hashing service
        var hashedPassword = "$2a$11$LQv3c1yqBWVHxkd0LHAkCOYz6TtxMQJqhN8/LewY5YmMxSUaqzpva"; // BCrypt hash of "Password123!"

        modelBuilder.Entity<AppUser>().HasData(
            new AppUser
            {
                Id = 1,
                FullName = "Admin User",
                Email = "admin@campus.local",
                Password = hashedPassword,
                Role = UserRole.Admin,
                CreatedAt = seedCreatedAtUtc,
                IsDeleted = false
            },
            new AppUser
            {
                Id = 2,
                FullName = "Student User",
                Email = "student@campus.local",
                Password = hashedPassword,
                Role = UserRole.User,
                CreatedAt = seedCreatedAtUtc,
                IsDeleted = false
            },
            new AppUser
            {
                Id = 3,
                FullName = "Dosen Pembimbing",
                Email = "dosen@campus.local",
                Password = hashedPassword,
                Role = UserRole.User,
                CreatedAt = seedCreatedAtUtc,
                IsDeleted = false
            },
            new AppUser
            {
                Id = 4,
                FullName = "Staff Akademik",
                Email = "staff@campus.local",
                Password = hashedPassword,
                Role = UserRole.Admin,
                CreatedAt = seedCreatedAtUtc,
                IsDeleted = false
            },
            new AppUser
            {
                Id = 5,
                FullName = "Mahasiswa Informatika",
                Email = "mahasiswa.if@campus.local",
                Password = hashedPassword,
                Role = UserRole.User,
                CreatedAt = seedCreatedAtUtc,
                IsDeleted = false
            }
        );

        // Room naming convention:
        // Gedung D4: Code prefix by row letter, number = floor + sequence (e.g., A 301 = Row A, Lantai 3, Room 01)
        // Gedung D3 (code HH): HH 301 = Gedung D3, Lantai 3, Room 01
        // Gedung SAW / PascaSarjana: SAW 10.08 = Gedung SAW, Lantai 10, Room 08
        modelBuilder.Entity<Room>().HasData(
            // === Gedung D4 ===
            new Room
            {
                Id = 1,
                Name = "A 301",
                Building = "Gedung D4",
                Floor = 3,
                Capacity = 40,
                Category = "Classroom",
                Description = "Ruang kelas baris A lantai 3, dilengkapi proyektor",
                Status = RoomStatus.Active,
                CreatedAt = seedCreatedAtUtc,
                IsDeleted = false
            },
            new Room
            {
                Id = 2,
                Name = "A 302",
                Building = "Gedung D4",
                Floor = 3,
                Capacity = 40,
                Category = "Classroom",
                Description = "Ruang kelas baris A lantai 3",
                Status = RoomStatus.Active,
                CreatedAt = seedCreatedAtUtc,
                IsDeleted = false
            },
            new Room
            {
                Id = 3,
                Name = "B 201",
                Building = "Gedung D4",
                Floor = 2,
                Capacity = 35,
                Category = "Classroom",
                Description = "Ruang kelas baris B lantai 2",
                Status = RoomStatus.Active,
                CreatedAt = seedCreatedAtUtc,
                IsDeleted = false
            },
            new Room
            {
                Id = 4,
                Name = "C 401",
                Building = "Gedung D4",
                Floor = 4,
                Capacity = 30,
                Category = "Lab",
                Description = "Lab komputer baris C lantai 4",
                Status = RoomStatus.Active,
                CreatedAt = seedCreatedAtUtc,
                IsDeleted = false
            },
            new Room
            {
                Id = 5,
                Name = "A 101",
                Building = "Gedung D4",
                Floor = 1,
                Capacity = 120,
                Category = "Lecture Hall",
                Description = "Aula besar lantai 1 gedung D4",
                Status = RoomStatus.Active,
                CreatedAt = seedCreatedAtUtc,
                IsDeleted = false
            },
            // === Gedung D3 (code HH) ===
            new Room
            {
                Id = 6,
                Name = "HH 301",
                Building = "Gedung D3",
                Floor = 3,
                Capacity = 40,
                Category = "Classroom",
                Description = "Ruang kelas gedung D3 lantai 3",
                Status = RoomStatus.Active,
                CreatedAt = seedCreatedAtUtc,
                IsDeleted = false
            },
            new Room
            {
                Id = 7,
                Name = "HH 302",
                Building = "Gedung D3",
                Floor = 3,
                Capacity = 40,
                Category = "Classroom",
                Description = "Ruang kelas gedung D3 lantai 3",
                Status = RoomStatus.Active,
                CreatedAt = seedCreatedAtUtc,
                IsDeleted = false
            },
            new Room
            {
                Id = 8,
                Name = "HH 201",
                Building = "Gedung D3",
                Floor = 2,
                Capacity = 30,
                Category = "Lab",
                Description = "Lab komputer gedung D3 lantai 2",
                Status = RoomStatus.Active,
                CreatedAt = seedCreatedAtUtc,
                IsDeleted = false
            },
            new Room
            {
                Id = 9,
                Name = "HH 102",
                Building = "Gedung D3",
                Floor = 1,
                Capacity = 50,
                Category = "Seminar Room",
                Description = "Ruang seminar gedung D3 lantai 1",
                Status = RoomStatus.Active,
                CreatedAt = seedCreatedAtUtc,
                IsDeleted = false
            },
            // === Gedung SAW ===
            new Room
            {
                Id = 10,
                Name = "SAW 10.08",
                Building = "Gedung SAW",
                Floor = 10,
                Capacity = 45,
                Category = "Classroom",
                Description = "Ruang kelas gedung SAW lantai 10 ruang 08",
                Status = RoomStatus.Active,
                CreatedAt = seedCreatedAtUtc,
                IsDeleted = false
            },
            new Room
            {
                Id = 11,
                Name = "SAW 10.09",
                Building = "Gedung SAW",
                Floor = 10,
                Capacity = 45,
                Category = "Classroom",
                Description = "Ruang kelas gedung SAW lantai 10 ruang 09",
                Status = RoomStatus.Active,
                CreatedAt = seedCreatedAtUtc,
                IsDeleted = false
            },
            new Room
            {
                Id = 12,
                Name = "SAW 5.01",
                Building = "Gedung SAW",
                Floor = 5,
                Capacity = 30,
                Category = "Lab",
                Description = "Lab gedung SAW lantai 5 ruang 01",
                Status = RoomStatus.Active,
                CreatedAt = seedCreatedAtUtc,
                IsDeleted = false
            },
            new Room
            {
                Id = 13,
                Name = "SAW 3.05",
                Building = "Gedung SAW",
                Floor = 3,
                Capacity = 60,
                Category = "Seminar Room",
                Description = "Ruang seminar gedung SAW lantai 3 ruang 05",
                Status = RoomStatus.Active,
                CreatedAt = seedCreatedAtUtc,
                IsDeleted = false
            },
            // === Gedung PascaSarjana ===
            new Room
            {
                Id = 14,
                Name = "PS 7.01",
                Building = "Gedung PascaSarjana",
                Floor = 7,
                Capacity = 25,
                Category = "Meeting Room",
                Description = "Ruang meeting PascaSarjana lantai 7 ruang 01",
                Status = RoomStatus.Active,
                CreatedAt = seedCreatedAtUtc,
                IsDeleted = false
            },
            new Room
            {
                Id = 15,
                Name = "PS 7.02",
                Building = "Gedung PascaSarjana",
                Floor = 7,
                Capacity = 35,
                Category = "Classroom",
                Description = "Ruang kelas PascaSarjana lantai 7 ruang 02",
                Status = RoomStatus.Active,
                CreatedAt = seedCreatedAtUtc,
                IsDeleted = false
            }
        );
    }
}
