# Campus Room Booking System - Backend

REST API untuk sistem peminjaman ruangan kampus, dibangun menggunakan ASP.NET Core 10.0 dengan Entity Framework Core dan SQLite.

## Prerequisites

- [.NET 10.0 SDK](https://dotnet.microsoft.com/download)
- Entity Framework Core CLI: `dotnet tool install --global dotnet-ef`

## Setup & Installation

```bash
# 1. Clone repository
git clone https://github.com/Wisam23-am/2026-campus-room-booking-backend.git
cd 2026-campus-room-booking-backend

# 2. Restore packages
dotnet restore

# 3. Apply database migration
dotnet ef database update

# 4. Jalankan aplikasi
dotnet run
```

Aplikasi berjalan di: `http://localhost:5168`  
Swagger UI: `http://localhost:5168/swagger`

## Tech Stack

- ASP.NET Core 10.0
- Entity Framework Core (SQLite)
- Swashbuckle (Swagger UI)

## API Endpoints

### Base URL: `/api/roombooking`

| Method | Endpoint | Deskripsi | Status Code |
|--------|----------|-----------|-------------|
| GET | `/api/roombooking` | List semua peminjaman (dengan search, filter, sort, pagination) | 200 |
| GET | `/api/roombooking/{id}` | Detail peminjaman berdasarkan ID | 200 / 404 |
| POST | `/api/roombooking` | Buat peminjaman baru | 201 / 400 |
| PUT | `/api/roombooking/{id}` | Update peminjaman | 200 / 400 / 404 |
| DELETE | `/api/roombooking/{id}` | Hapus peminjaman (soft delete) | 204 / 404 |

### Query Parameters (GET /api/roombooking)

| Parameter | Tipe | Default | Deskripsi |
|-----------|------|---------|----------|
| search | string | - | Cari berdasarkan RoomName atau BookedBy |
| status | int | - | Filter status: 0=Pending, 1=Approved, 2=Rejected |
| startDate | datetime | - | Filter dari tanggal |
| endDate | datetime | - | Filter sampai tanggal |
| sortBy | string | CreatedAt | Sort: CreatedAt, StartTime, RoomName, BookedBy |
| sortOrder | string | desc | Urutan: asc / desc |
| page | int | 1 | Nomor halaman |
| pageSize | int | 10 | Jumlah per halaman (max 50) |

**Contoh:**
```
GET /api/roombooking?search=Lab&status=0&sortBy=StartTime&sortOrder=asc&page=1&pageSize=10
```

### Request Body

#### POST /api/roombooking (Create Booking)

```json
{
  "roomName": "Lab Komputer 1",
  "bookedBy": "Ahmad Wisam",
  "purpose": "Praktikum Pemrograman Web",
  "startTime": "2026-02-10T08:00:00",
  "endTime": "2026-02-10T10:00:00"
}
```

#### PUT /api/roombooking/{id} (Update Booking)

```json
{
  "roomName": "Lab Komputer 1",
  "bookedBy": "Ahmad Wisam",
  "purpose": "Praktikum Pemrograman Web",
  "startTime": "2026-02-10T08:00:00",
  "endTime": "2026-02-10T10:00:00",
  "status": 1
}
```

### Response Format

#### Success Response (Single Booking)
```json
{
  "id": 1,
  "roomName": "Lab Komputer 1",
  "bookedBy": "Ahmad Wisam",
  "purpose": "Praktikum Pemrograman Web",
  "startTime": "2026-02-10T08:00:00",
  "endTime": "2026-02-10T10:00:00",
  "status": 0,
  "createdAt": "2026-02-07T10:00:00",
  "updatedAt": null
}
```

#### Paginated Response (List Bookings)
```json
{
  "data": [ ... ],
  "currentPage": 1,
  "pageSize": 10,
  "totalCount": 25,
  "totalPages": 3,
  "hasPrevious": false,
  "hasNext": true
}
```

#### Error Response
```json
{
  "statusCode": 400,
  "message": "Validation failed",
  "errors": {
    "EndTime": ["End time must be after start time"],
    "StartTime": ["Cannot book in the past"]
  }
}
```

### Booking Status

| Value | Status | Deskripsi |
|-------|--------|----------|
| 0 | Pending | Menunggu persetujuan |
| 1 | Approved | Disetujui |
| 2 | Rejected | Ditolak |

## Validasi

- `RoomName`: Wajib, maksimal 100 karakter
- `BookedBy`: Wajib, maksimal 100 karakter
- `Purpose`: Opsional, maksimal 500 karakter
- `StartTime`: Wajib, tidak boleh di masa lalu
- `EndTime`: Wajib, harus setelah StartTime
- `Status`: Wajib (pada update)

## Project Structure

```
2026-campus-room-booking-backend/
├── Controllers/
│   └── RoomBookingController.cs
├── Data/
│   └── ApplicationDbContext.cs
├── DTOs/
│   ├── BookingQueryDto.cs
│   ├── BookingResponseDto.cs
│   ├── CreateBookingDto.cs
│   ├── ErrorResponseDto.cs
│   ├── PaginatedResponseDto.cs
│   └── UpdateBookingDto.cs
├── Enums/
│   └── BookingStatus.cs
├── Middleware/
│   └── ExceptionHandlingMiddleware.cs
├── Migrations/
├── Models/
│   └── RoomBooking.cs
├── Program.cs
├── appsettings.json
└── .gitignore
```

## Database Migration

```bash
# Buat migration baru
dotnet ef migrations add NamaMigration

# Apply migration
dotnet ef database update

# Rollback migration
dotnet ef database update PreviousMigrationName
```

## Development

- **Default Branch:** `develop`
- **Commit Format:** [Conventional Commits](https://www.conventionalcommits.org/)
- **Versioning:** [Semantic Versioning](https://semver.org/)

## Testing

Gunakan file `2026-campus-room-booking-backend.http` untuk testing API endpoints via VS Code REST Client, atau buka Swagger UI di `http://localhost:5168/swagger`.

## License

This project is for educational purposes.