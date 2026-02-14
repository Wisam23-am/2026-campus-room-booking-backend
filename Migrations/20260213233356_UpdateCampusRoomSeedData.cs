using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace _2026_campus_room_booking_backend.Migrations
{
    /// <inheritdoc />
    public partial class UpdateCampusRoomSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Rooms",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Building", "Capacity", "Category", "Description", "Floor", "Name" },
                values: new object[] { "Gedung D4", 40, "Classroom", "Ruang kelas baris A lantai 3, dilengkapi proyektor", 3, "A 301" });

            migrationBuilder.UpdateData(
                table: "Rooms",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Building", "Capacity", "Category", "Description", "Name" },
                values: new object[] { "Gedung D4", 40, "Classroom", "Ruang kelas baris A lantai 3", "A 302" });

            migrationBuilder.InsertData(
                table: "Rooms",
                columns: new[] { "Id", "Building", "Capacity", "Category", "CreatedAt", "Description", "Floor", "Name", "Status", "UpdatedAt" },
                values: new object[,]
                {
                    { 3, "Gedung D4", 35, "Classroom", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Ruang kelas baris B lantai 2", 2, "B 201", 0, null },
                    { 4, "Gedung D4", 30, "Lab", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Lab komputer baris C lantai 4", 4, "C 401", 0, null },
                    { 5, "Gedung D4", 120, "Lecture Hall", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Aula besar lantai 1 gedung D4", 1, "A 101", 0, null },
                    { 6, "Gedung D3", 40, "Classroom", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Ruang kelas gedung D3 lantai 3", 3, "HH 301", 0, null },
                    { 7, "Gedung D3", 40, "Classroom", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Ruang kelas gedung D3 lantai 3", 3, "HH 302", 0, null },
                    { 8, "Gedung D3", 30, "Lab", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Lab komputer gedung D3 lantai 2", 2, "HH 201", 0, null },
                    { 9, "Gedung D3", 50, "Seminar Room", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Ruang seminar gedung D3 lantai 1", 1, "HH 102", 0, null },
                    { 10, "Gedung SAW", 45, "Classroom", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Ruang kelas gedung SAW lantai 10 ruang 08", 10, "SAW 10.08", 0, null },
                    { 11, "Gedung SAW", 45, "Classroom", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Ruang kelas gedung SAW lantai 10 ruang 09", 10, "SAW 10.09", 0, null },
                    { 12, "Gedung SAW", 30, "Lab", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Lab gedung SAW lantai 5 ruang 01", 5, "SAW 5.01", 0, null },
                    { 13, "Gedung SAW", 60, "Seminar Room", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Ruang seminar gedung SAW lantai 3 ruang 05", 3, "SAW 3.05", 0, null },
                    { 14, "Gedung PascaSarjana", 25, "Meeting Room", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Ruang meeting PascaSarjana lantai 7 ruang 01", 7, "PS 7.01", 0, null },
                    { 15, "Gedung PascaSarjana", 35, "Classroom", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Ruang kelas PascaSarjana lantai 7 ruang 02", 7, "PS 7.02", 0, null }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedAt", "Email", "FullName", "Role", "UpdatedAt" },
                values: new object[,]
                {
                    { 3, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "dosen@campus.local", "Dosen Pembimbing", 0, null },
                    { 4, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "staff@campus.local", "Staff Akademik", 1, null },
                    { 5, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "mahasiswa.if@campus.local", "Mahasiswa Informatika", 0, null }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Rooms",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Rooms",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Rooms",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Rooms",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Rooms",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Rooms",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Rooms",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Rooms",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Rooms",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Rooms",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Rooms",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Rooms",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "Rooms",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.UpdateData(
                table: "Rooms",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Building", "Capacity", "Category", "Description", "Floor", "Name" },
                values: new object[] { "Main Building", 120, "Lecture Hall", "Large lecture hall with projector", 1, "Lecture Hall A" });

            migrationBuilder.UpdateData(
                table: "Rooms",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Building", "Capacity", "Category", "Description", "Name" },
                values: new object[] { "Engineering Hall", 30, "Lab", "Computer lab for practical sessions", "Computer Lab 304" });
        }
    }
}
