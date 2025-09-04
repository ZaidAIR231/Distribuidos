using Microsoft.EntityFrameworkCore;
using NarutoApi.Infrastructure;
using NarutoApi.Services;
using SoapCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSoapCore();

// EF Core (Pomelo MySQL)
var conn = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<RelationalDbContext>(options =>
    options.UseMySql(conn, ServerVersion.AutoDetect(conn))
);

// Registraremos la implementación real en el siguiente paso
builder.Services.AddScoped<INinjaService, NinjaService>(); // ← la creamos al rato

var app = builder.Build();

// Endpoint SOAP (como en tu repo)
app.UseSoapEndpoint<INinjaService>("/NinjaService.svc", new SoapEncoderOptions());

app.Run();

