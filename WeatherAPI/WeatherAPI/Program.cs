using Microsoft.EntityFrameworkCore;
using WeatherAPI;
using WeatherAPI.DAL;
using WeatherAPI.DbContexts;
using WeatherAPI.Entities;
using WeatherAPI.Services;

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

builder.Services.AddScoped<IWeatherScraper, WeatherScraper>();

builder.Services.AddTransient<DataSeeder>();

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddAutoMapper(typeof(Program));

var app = builder.Build();

if (args.Length == 1 && args[0].ToLower() == "seeddata")
{
    var scopedFactory = app.Services.GetService<IServiceScopeFactory>()
        ?? throw new NullReferenceException("scopedFactory");

    var scope = scopedFactory.CreateScope();

    var service = scope.ServiceProvider.GetService<DataSeeder>()
        ?? throw new NullReferenceException("service");

    service.Seed();
}


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
