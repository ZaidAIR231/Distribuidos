using Microsoft.EntityFrameworkCore;
using NarutoApi.Infrastructure;
using NarutoApi.Repositories;
using NarutoApi.Services;
using SoapCore;

var builder = WebApplication.CreateBuilder(args); // preparar todo el aplicativo para levantar una app web
builder.Services.AddSoapCore();
builder.Services.AddScoped<INinjaRepository, NinjaRepository>();
builder.Services.AddScoped<INinjaService, NinjaService>();
var conn = builder.Configuration.GetConnectionString("DefaultConnection");

// ⚠️ No uses AutoDetect en diseño:
var serverVersion = new MySqlServerVersion(new Version(8, 0, 36)); // o 8,0,0

builder.Services.AddDbContext<RelationalDbContext>(options =>
    options.UseMySql(conn, serverVersion));
var app = builder.Build();
app.UseSoapEndpoint<INinjaService>("/NinjaService.svc", new SoapEncoderOptions());
app.Run();
