# Campus Room Booking System - Backend API

[![.NET](https://img.shields.io/badge/.NET-10.0-512BD4?logo=dotnet)](https://dotnet.microsoft.com/)
[![Version](https://img.shields.io/badge/version-1.3.0-blue.svg)](https://github.com/Wisam23-am/2026-campus-room-booking-backend/releases/tag/v1.3.0)
[![License](https://img.shields.io/badge/license-MIT-green.svg)](LICENSE)

RESTful API untuk sistem peminjaman ruangan kampus yang dibangun menggunakan ASP.NET Core 10.0 dengan Entity Framework Core, JWT Authentication, dan SQLite database.

## 📖 Table of Contents

- [Features](#-features)
- [Tech Stack](#-tech-stack)
- [Prerequisites](#-prerequisites)
- [Installation](#-installation)
- [Configuration](#-configuration)
- [Running the Application](#-running-the-application)
- [API Documentation](#-api-documentation)
- [Database Schema](#-database-schema)
- [Authentication](#-authentication)
- [Testing](#-testing)
- [Project Structure](#-project-structure)
- [Contributing](#-contributing)

## ✨ Features

### Core Features
- ✅ **Room Booking Management** - CRUD operations untuk peminjaman ruangan
- ✅ **Room Management** - Kelola data ruangan (nama, kapasitas, fasilitas)
- ✅ **User Management** - User registration, profile management
- ✅ **Booking Status** - Pending, Approved, Rejected workflow
- ✅ **Search & Filter** - Advanced query dengan pagination
- ✅ **Data Validation** - Comprehensive input validation
- ✅ **Soft Delete** - Data retention dengan IsDeleted flag

### Authentication & Authorization
- 🔐 **JWT Authentication** - Secure token-based auth
- 👤 **Role-based Access Control** - Admin & User roles
- 🔒 **Password Hashing** - BCrypt encryption
- 🔑 **Change Password** - Self-service password update
- 👥 **User Profile** - Self-service profile endpoint

### Advanced Features
- 📅 **Room Schedule** - View booking schedule dengan date filtering
- ⏰ **Room Availability** - Check availability untuk time slots
- 📊 **Pagination** - Efficient data loading
- 🔍 **Full-text Search** - Search across multiple fields
- ⚡ **Performance** - Database indexes untuk optimized queries
- 🛡️ **Error Handling** - Global exception middleware
- 📝 **API Documentation** - Swagger/OpenAPI integration

## 🛠️ Tech Stack

| Technology | Version | Purpose |
|-----------|---------|---------|
| [.NET](https://dotnet.microsoft.com/) | 10.0 | Framework utama |
| [Entity Framework Core](https://docs.microsoft.com/ef/) | 10.0.2 | ORM & Database migrations |
| [SQLite](https://www.sqlite.org/) | Latest | Embedded database |
| [JWT Bearer](https://jwt.io/) | 10.0.2 | Authentication |
| [BCrypt.Net](https://github.com/BcryptNet/bcrypt.net) | 4.0.3 | Password hashing |
| [Swashbuckle](https://github.com/domaindrivendev/Swashbuckle.AspNetCore) | 7.2.0 | API documentation |

## 📦 Prerequisites

Pastikan sistem Anda memiliki:

- **[.NET 10.0 SDK](https://dotnet.microsoft.com/download)** atau lebih baru
- **[Git](https://git-scm.com/)** untuk version control
- **[Visual Studio 2022](https://visualstudio.microsoft.com/)** atau **[VS Code](https://code.visualstudio.com/)** (optional)
- **[Postman](https://www.postman.com/)** untuk testing API (optional)

### Install Entity Framework Core Tools

```bash
dotnet tool install --global dotnet-ef
```

Verifikasi instalasi:
```bash
dotnet --version      # Should output: 10.0.x
dotnet ef --version   # Should output: 10.0.x
```

## 🚀 Installation

### 1. Clone Repository

```bash
git clone https://github.com/Wisam23-am/2026-campus-room-booking-backend.git
cd 2026-campus-room-booking-backend
```

### 2. Restore Dependencies

```bash
dotnet restore
```

### 3. Setup Database

```bash
# Apply migrations
dotnet ef database update

# Database file akan dibuat di: roombooking.db
```

### 4. Verify Installation

```bash
dotnet build
```

## ⚙️ Configuration

### appsettings.json

Konfigurasi utama ada di `appsettings.json`:

```json
{
  "Jwt": {
    "Key": "your-super-secret-key-minimum-32-characters-long",
    "Issuer": "CampusRoomBookingAPI",
    "Audience": "CampusRoomBookingClient",
    "ExpiryHours": 24
  },
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=roombooking.db"
  }
}
```

⚠️ **IMPORTANT**: Jangan commit JWT secret key ke Git! Gunakan `.env.example` sebagai template.

## 🏃 Running the Application

### Development Mode

```bash
dotnet run
```

Atau dengan watch mode (auto-reload):

```bash
dotnet watch run
```

Server akan berjalan di:
- **API Base**: http://localhost:5168
- **Swagger UI**: http://localhost:5168/swagger

### Production Build

```bash
dotnet publish -c Release -o ./publish
cd publish
dotnet 2026-campus-room-booking-backend.dll
```

## 📚 API Documentation

### Quick Reference

**Base URL:** `http://localhost:5168/api`

**Authentication:** Bearer Token (JWT)

### 🔐 Authentication Endpoints

#### Register User
```http
POST /api/auth/register
Content-Type: application/json

{
  "fullName": "John Doe",
  "email": "john.doe@example.com",
  "password": "Password123!",
  "role": "User"
}
```

#### Login
```http
POST /api/auth/login
Content-Type: application/json

{
  "email": "john.doe@example.com",
  "password": "Password123!"
}
```

#### Get Current User
```http
GET /api/auth/me
Authorization: Bearer {token}
```

#### Change Password
```http
POST /api/auth/change-password
Authorization: Bearer {token}
Content-Type: application/json

{
  "currentPassword": "OldPassword123!",
  "newPassword": "NewPassword123!"
}
```

### 📋 Room Booking Endpoints

#### List Bookings
```http
GET /api/roombooking?search=meeting&status=Pending&page=1&pageSize=10
Authorization: Bearer {token}
```

#### Create Booking
```http
POST /api/roombooking
Authorization: Bearer {token}
Content-Type: application/json

{
  "roomId": 1,
  "purpose": "Team Meeting",
  "startTime": "2026-02-20T09:00:00Z",
  "endTime": "2026-02-20T10:00:00Z"
}
```

#### Update Booking Status (Admin Only)
```http
PATCH /api/roombooking/{id}/status
Authorization: Bearer {admin-token}
Content-Type: application/json

{
  "status": "Approved"
}
```

### 🏢 Room Endpoints

#### List Rooms
```http
GET /api/rooms?search=D4&status=Active&page=1
Authorization: Bearer {token}
```

#### Get Room Schedule
```http
GET /api/rooms/{id}/schedule?startDate=2026-02-17&endDate=2026-02-24
Authorization: Bearer {token}
```

#### Check Room Availability
```http
GET /api/rooms/{id}/availability?startTime=2026-02-20T09:00:00Z&endTime=2026-02-20T11:00:00Z
Authorization: Bearer {token}
```

### 👥 User Endpoints (Admin Only)

```http
GET /api/users?search=john&role=User
POST /api/users
PUT /api/users/{id}
DELETE /api/users/{id}
```

For complete API documentation, see [API Documentation](../2026-campus-room-booking-docs/api/api-documentation.md) or visit Swagger UI at http://localhost:5168/swagger

## 🗄️ Database Schema

### Main Tables

- **AppUsers** - User accounts with authentication
- **Rooms** - Campus rooms with facilities
- **RoomBookings** - Booking records with status

### Relationships

```
AppUsers (1) ───< (N) RoomBookings (N) >─── (1) Rooms
```

For complete schema, see [Database Schema](../2026-campus-room-booking-docs/architecture/database-schema.md)

## 🔐 Authentication

### JWT Token

Token expires in 24 hours. Include in Authorization header:

```
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
```

### Test Credentials

**Admin Users:**
- admin@campus.edu / Password123!
- superadmin@campus.edu / Password123!

**Regular Users:**
- john.doe@campus.edu / Password123!
- jane.smith@campus.edu / Password123!

### Role-based Access

| Endpoint | User | Admin |
|----------|------|-------|
| Booking CRUD | ✅ (own) | ✅ (all) |
| Approve/Reject | ❌ | ✅ |
| Room Management | ❌ | ✅ |
| User Management | ❌ | ✅ |

## 🧪 Testing

### Using Swagger UI

1. Run: `dotnet run`
2. Open: http://localhost:5168/swagger
3. Click "Authorize" and login
4. Test endpoints

### Using .http File

Open `2026-campus-room-booking-backend.http` in VS Code with REST Client extension.

## 📁 Project Structure

```
2026-campus-room-booking-backend/
├── Controllers/          # API Controllers
├── Data/                 # EF Core DbContext
├── DTOs/                 # Data Transfer Objects
├── Enums/                # Status enums
├── Middleware/           # Exception handling
├── Migrations/           # EF Core migrations
├── Models/               # Entity models
├── Program.cs            # Entry point
└── appsettings.json      # Configuration
```

## 🤝 Contributing

### Git Workflow

1. Branch dari `develop`: `git checkout -b feature/your-feature`
2. Commit dengan Conventional Commits: `feat(auth): add feature`
3. Push dan buat PR ke `develop`
4. Setelah review, merge ke `develop`
5. Release: PR dari `develop` → `main`

### Code Standards

- Follow C# Coding Conventions
- Add XML documentation for public APIs
- Write meaningful commit messages
- Keep controllers thin

## 📝 Changelog

See [CHANGELOG.md](CHANGELOG.md)

**Latest:** v1.3.0 (2026-02-17)
- Room schedule & availability
- Change password endpoint
- Self-service profile

## 🔗 Related Projects

- [Frontend](https://github.com/Wisam23-am/2026-campus-room-booking-frontend)
- [Documentation](https://github.com/Wisam23-am/2026-campus-room-booking-docs)

---

**Made with ❤️ for Campus Room Booking System**