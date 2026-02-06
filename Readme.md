# Campus Room Booking System - Backend

ASP.NET Core API untuk sistem peminjaman ruangan kampus.

## Prerequisites
- .NET 10.0 SDK
- SQL Server / PostgreSQL / SQLite

## Setup
1. Clone repository
2. Copy `.env.example` to `.env` dan sesuaikan konfigurasi
3. Jalankan migration: `dotnet ef database update`
4. Jalankan aplikasi: `dotnet run`

## API Endpoints
### Room Bookings
- `GET /api/bookings` - List semua peminjaman
- `GET /api/bookings/{id}` - Detail peminjaman
- `POST /api/bookings` - Tambah peminjaman
- `PUT /api/bookings/{id}` - Update peminjaman
- `DELETE /api/bookings/{id}` - Hapus peminjaman

## Tech Stack
- ASP.NET Core 10.0
- Entity Framework Core
- SQL Server

## Development
Branch: `develop` (default)
Commit format: Conventional Commits