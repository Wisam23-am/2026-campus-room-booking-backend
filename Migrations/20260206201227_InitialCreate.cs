using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace _2026_campus_room_booking_backend.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RoomBookings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    RoomName = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    BookedBy = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Purpose = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true),
                    StartTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    EndTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Status = table.Column<int>(type: "INTEGER", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false, defaultValueSql: "datetime('now')"),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoomBookings", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RoomBookings_IsDeleted",
                table: "RoomBookings",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_RoomBookings_StartTime",
                table: "RoomBookings",
                column: "StartTime");

            migrationBuilder.CreateIndex(
                name: "IX_RoomBookings_Status",
                table: "RoomBookings",
                column: "Status");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RoomBookings");
        }
    }
}
