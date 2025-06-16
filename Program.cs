using GhiblipediaAPI.Data;
using Microsoft.Data.SqlClient;
using Microsoft.OpenApi.Models;
using Npgsql;
using System;
using System.Data;
using System.Web;

var builder = WebApplication.CreateBuilder(args);

string? connectionString = Environment.GetEnvironmentVariable("CONNECTION_STRING");

if (string.IsNullOrEmpty(connectionString))
{
    throw new InvalidOperationException("Environment variable CONNECTION_STRING is not set.");
}

builder.Services.AddScoped<IDbConnection>(sp =>
    new NpgsqlConnection(connectionString));


//127.0.0.1
//builder.Services.AddScoped<IDbConnection>(sp =>
//    new NpgsqlConnection("Server=aws-0-eu-north-1.pooler.supabase.com;Port=6543;Database=postgres;User Id=postgres.wannbtwmxjqfxlqnsbsn;Password=7n1J3IYeq98Kyz2GhZQCCzEYLbJCVwuO;SslMode=Require;Trust Server Certificate=true;"));

//builder.Services.AddScoped<IMovieRepository, MovieRepository>();
builder.Services.AddTransient<IMovieRepository, MovieRepository>();
builder.Services.AddAutoMapper(typeof(Program));

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins("http://localhost:5173") // Frontend URL
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// Add services to the container.
builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "My API",
        Version = "v1"
    });
});

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.MapOpenApi();
}



app.UseHttpsRedirection();

app.UseCors();

app.UseAuthorization();

app.MapControllers();

app.Run();
