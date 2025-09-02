using Microsoft.EntityFrameworkCore;
using NarutoApi.Infrastructure;
using NarutoApi.Services;
using NarutoApi.Repositories;   // <-- agrega esto
using SoapCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSoapCore();

// EF Core (Pomelo MySQL)
var conn = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<RelationalDbContext>(options =>
    options.UseMySql(conn, ServerVersion.AutoDetect(conn))
);

// DI
builder.Services.AddScoped<INinjaRepository, NinjaRepository>(); // <-- registra el repo
builder.Services.AddScoped<INinjaService, NinjaService>();

var app = builder.Build();

// Endpoint SOAP
app.UseSoapEndpoint<INinjaService>("/NinjaService.svc", new SoapEncoderOptions());

app.Run();
