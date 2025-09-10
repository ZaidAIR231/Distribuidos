using Microsoft.EntityFrameworkCore;
using NarutoApi.Infrastructure;
using NarutoApi.Repositories;
using NarutoApi.Services;
using SoapCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSoapCore();
builder.Services.AddScoped<INinjaRepository, NinjaRepository>();
builder.Services.AddScoped<INinjaService, NinjaService>();

var conn = builder.Configuration.GetConnectionString("DefaultConnection");


var serverVersion = MySqlServerVersion.AutoDetect(conn);

builder.Services.AddDbContext<RelationalDbContext>(options =>
    options.UseMySql(conn, serverVersion));

var app = builder.Build();
app.UseSoapEndpoint<INinjaService>("/NinjaService.svc", new SoapEncoderOptions());
app.Run();
