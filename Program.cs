// --- ASEGÚRATE DE TENER ESTOS USINGS ---
using Microsoft.EntityFrameworkCore;
using SistemaDeGestionTalento.Data;
using System.Text.Json.Serialization;
// ---------------------------------------

var builder = WebApplication.CreateBuilder(args);

// --- CÓDIGO DE CONEXIÓN ---
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<SgiDbContext>(options =>
    options.UseSqlServer(connectionString));
// -------------------------


// Add services to the container.
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        // Esta es la línea mágica que ignora los bucles infinitos
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    });

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
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

app.UseAuthorization();

app.MapControllers();

app.Run();