// --- 1. Usings necesarios ---
using Microsoft.EntityFrameworkCore;
using SistemaGestionTalento.Application.Interfaces;
using SistemaGestionTalento.Application.Interfaces.Services;
using SistemaGestionTalento.Application.Services;
using SistemaGestionTalento.Infrastructure.Persistence;
using SistemaGestionTalento.Infrastructure.Repositories;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// --- 2. Registra el DbContext ---
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


// --- 3. Add services to the container. ---
builder.Services
    .AddControllers()
    .AddJsonOptions(o =>
    {
        // Evita referencias circulares en JSON al serializar relaciones muchos-a-muchos
        o.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    });

// CORS para permitir peticiones del frontend en desarrollo
const string CorsPolicyName = "Frontend";
builder.Services.AddCors(options =>
{
    options.AddPolicy(CorsPolicyName, policy =>
        policy.WithOrigins(
                "http://localhost:4200", // Angular
                "http://localhost:5173", // Vite/React
                "http://127.0.0.1:5173",
                "http://localhost:3000", // React
                "http://localhost:8080", // Vue
                "http://localhost:9000", // Quasar (frontend indicado)
                "http://127.0.0.1:9000"
            )
            .AllowAnyHeader()
            .AllowAnyMethod()
            // Si usas auth basada en cookies, descomenta:
            //.AllowCredentials()
        );
});

builder.Services.AddAuthorization();

// --- 4. Registra tus Servicios (Modificado para Unit of Work) ---
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IMatchingService, MatchingService>();


// --- 5. Configura Swagger/OpenAPI (la forma estándar) ---
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Habilitar CORS antes de authorization
app.UseCors(CorsPolicyName);

app.UseAuthorization();

app.MapControllers();

app.Run();
