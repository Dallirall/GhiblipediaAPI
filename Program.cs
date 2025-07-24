using GhiblipediaAPI.Data;
using GhiblipediaAPI.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Npgsql;
using System;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Sockets;
using System.Web;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAutoMapper(typeof(Program)); //For mapping between movie classes.

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins("https://localhost:5173", "https://ghiblipedia.onrender.com") // Frontend URL
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile("appsettings.Local.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables();

builder.Services.AddControllers();
builder.Services.AddControllers().AddNewtonsoftJson();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "My API",
        Version = "v1"
    });
});


builder.Services.AddOpenApi();

string? connectionString = builder.Configuration.GetConnectionString("DefaultConnection"); //Connection string is stored in the Render environment, and in 'appsettings.Local.json' for local environment.

if (string.IsNullOrEmpty(connectionString))
{
    throw new InvalidOperationException("The connection string is not set.");
}

//var jwtAuth = builder.Configuration["Jwt:JwtAuthority"]; 
//Console.WriteLine("Added authority " + jwtAuth);

//builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
//{
//    options.Authority = jwtAuth;
//    options.Audience = "https://ghiblipediaapi.onrender.com";
//});

//builder.Services.AddAuthorization(options =>
//{
//    options.AddPolicy("AdminOnly", policy =>
//    policy.RequireRole("admin"));
//});

builder.Services.AddScoped<IDbConnection>(sp =>
    new NpgsqlConnection(connectionString));

builder.Services.Configure<OmdbAPIOptions>(builder.Configuration.GetSection("OmdbApi")); //Sets the value to the OMDb API key

builder.Services.AddTransient<OmdbAPIService>();

builder.Services.AddTransient<IMovieRepository, MovieRepository>();

Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true; //For mapping purposes when using Dapper

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.MapOpenApi();
}

var port = Environment.GetEnvironmentVariable("PORT"); //This variable is stored in the Render environment.
if (!string.IsNullOrEmpty(port))
{
    app.Urls.Add($"http://*:{port}");
    Console.WriteLine($"Listening on port: {port}"); 
}
else
{   
    app.Urls.Add("http://*:8080");
    Console.WriteLine("PORT environment variable not set, falling back to default 8080.");
}

//When deployed on Render.com, this step is unnecessary (as I understand it).
if (!app.Environment.IsProduction() || app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

app.UseCors();

app.UseAuthorization();

app.MapControllers();

app.UseAuthentication();

app.Run();


