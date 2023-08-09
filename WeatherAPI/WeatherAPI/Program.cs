using Microsoft.EntityFrameworkCore;
using WeatherAPI;
using WeatherAPI.DbContexts;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var root = Directory.GetCurrentDirectory();
var dotenv = Path.Combine(root, "config.env");
DotEnv.Load(dotenv);

var connectionString = $"Host={Environment.GetEnvironmentVariable("POSTGRESQL_ENDPOINT")}; " +
    $"Database=WeatherDb; " +
    $"Username={Environment.GetEnvironmentVariable("POSTGRESQL_USER")}; " +
    $"Password={Environment.GetEnvironmentVariable("POSTGRESQL_PASSWORD")}; ";

builder.Services.AddDbContext<WeatherDbContext>(
    options => options.UseNpgsql(connectionString));

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
