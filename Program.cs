using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using _2026_campus_room_booking_backend.Data;
using _2026_campus_room_booking_backend.Middleware;
using BCrypt.Net;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Configure database context
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen();

// JWT Authentication
var jwtKey = builder.Configuration["Jwt:Key"] ?? Environment.GetEnvironmentVariable("JWT_SECRET") ?? "";
var jwtIssuer = builder.Configuration["Jwt:Issuer"] ?? Environment.GetEnvironmentVariable("JWT_ISSUER") ?? "RoomBookingAPI";
var jwtAudience = builder.Configuration["Jwt:Audience"] ?? Environment.GetEnvironmentVariable("JWT_AUDIENCE") ?? "RoomBookingClient";

if (string.IsNullOrWhiteSpace(jwtKey))
{
    jwtKey = "dev-secret-change-me-please-32-bytes-minimum-123456";
}

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtIssuer,
            ValidAudience = jwtAudience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
        };
    });

builder.Services.AddAuthorization();

// Configure CORS for frontend development
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        policy =>
        {
            policy.WithOrigins("http://localhost:3000", "http://localhost:3001")
                .AllowAnyMethod()
                .AllowAnyHeader();
        });
});

var app = builder.Build();

// Development-only: ensure seeded demo users can authenticate with default password.
if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

    var demoEmails = new[]
    {
        "admin@campus.local",
        "student@campus.local",
        "dosen@campus.local",
        "staff@campus.local",
        "mahasiswa.if@campus.local",
    };

    var demoUsers = await db.Users
        .Where(u => !u.IsDeleted && demoEmails.Contains(u.Email))
        .ToListAsync();

    var updated = false;
    foreach (var user in demoUsers)
    {
        if (!BCrypt.Net.BCrypt.Verify("Password123!", user.Password))
        {
            user.Password = BCrypt.Net.BCrypt.HashPassword("Password123!");
            user.UpdatedAt = DateTime.UtcNow;
            updated = true;
        }
    }

    if (updated)
    {
        await db.SaveChangesAsync();
    }
}

// Global exception handler
app.UseMiddleware<ExceptionHandlingMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Use CORS before UseHttpsRedirection
app.UseCors("AllowFrontend");

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
