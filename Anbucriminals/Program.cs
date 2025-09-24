using Anbucriminals.Services;
using Anbucriminals.Gateways;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();

builder.Services.AddScoped<INinjaService, NinjaService>();
builder.Services.AddScoped<INinjaGateway, NinjaGateway>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.MapControllers();

app.Run();
