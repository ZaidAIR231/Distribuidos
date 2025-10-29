using MongoDB.Driver;
using Microsoft.Extensions.Options;
using ScrollsOfSealsApi.Services;       
using ScrollsOfSealsApi.Infrastructure; // MongoDBSettings 
using ScrollsOfSealsApi.Repositories;   

var builder = WebApplication.CreateBuilder(args);

// Repositorios 
builder.Services.AddScoped<IShinobiRepository, ShinobiRepository>(); 

// Config de Mongo
builder.Services.Configure<MongoDBSettings>(
    builder.Configuration.GetSection("MongoDB"));

builder.Services.AddSingleton<IMongoDatabase>(sp =>
{
    var settings = sp.GetRequiredService<IOptions<MongoDBSettings>>().Value;
    var client = new MongoClient(settings.ConnectionString);
    return client.GetDatabase(settings.DatabaseName);
});

// gRPC 
builder.Services.AddGrpc();

var app = builder.Build();

// Endpoints gRPC
app.MapGrpcService<ShinobiRegistryService>(); 


app.MapGet("/",
    () => "🍥 Scrolls of Seals API (gRPC).");

app.Run();
