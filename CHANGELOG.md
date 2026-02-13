# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [1.1.0] - 2026-02-14

### Added
- User entity and CRUD endpoints (`/api/users`)
- Room entity and CRUD endpoints (`/api/rooms`)
- Comprehensive API test cases
- Indexes for optimized queries (RoomName, BookedBy, StartTime)

### Fixed
- Input validation edge cases
- Error response standardization

### Changed
- Enhanced BookingStatus enum handling
- Improved database indexing strategy

## [1.0.0] - 2026-02-07

### Added
- Initial project setup with ASP.NET Core 10.0
- Entity Framework Core with SQLite database
- RoomBooking entity model with BookingStatus enum (Pending, Approved, Rejected)
- ApplicationDbContext with entity configuration and indexes
- Initial database migration
- Data Transfer Objects (DTOs) for API requests and responses
- CRUD endpoints for room bookings:
  - POST /api/roombooking - Create booking
  - GET /api/roombooking - List bookings with search, filter, sort, pagination
  - GET /api/roombooking/{id} - Get booking detail
  - PUT /api/roombooking/{id} - Update booking
  - DELETE /api/roombooking/{id} - Soft delete booking
- Search functionality (by RoomName, BookedBy)
- Filter by status and date range
- Sorting (CreatedAt, StartTime, RoomName, BookedBy)
- Pagination with metadata (totalCount, totalPages, hasNext, hasPrevious)
- Input validation with standardized error responses
- Custom validation: EndTime after StartTime, no booking in the past
- Global exception handling middleware
- Swagger UI for API documentation and testing
- Comprehensive API test cases in .http file
- API documentation in README.md

## [Unreleased]