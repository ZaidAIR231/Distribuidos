using Microsoft.EntityFrameworkCore;
using NarutoApi.Infrastructure;
using NarutoApi.Repositories;
using NarutoApi.Services;
using SoapCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSoapCore();


var conn = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<RelationalDbContext>(options =>
    options.UseMySql(conn, ServerVersion.AutoDetect(conn))
);


builder.Services.AddScoped<INinjaRepository, NinjaRepository>();
builder.Services.AddScoped<INinjaService, NinjaService>();

var app = builder.Build();


app.UseSoapEndpoint<INinjaService>("/NinjaService.svc", new SoapEncoderOptions());

app.Run();

