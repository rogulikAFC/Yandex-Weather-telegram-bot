using Microsoft.EntityFrameworkCore;
using WeatherAPI;
using WeatherAPI.DAL;
using WeatherAPI.DbContexts;
using WeatherAPI.Entities;
using WeatherAPI.Middlewares;
using WeatherAPI.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

var root = Directory.GetCurrentDirectory();
var dotenv = Path.Combine(root, ".env");
DotEnv.Load(dotenv);

var connectionString = $"Host={Environment.GetEnvironmentVariable("POSTGRESQL_ENDPOINT")}; " +
    $"Database=WeatherDb; " +
    $"Username={Environment.GetEnvironmentVariable("POSTGRESQL_USER")}; " +
    $"Password={Environment.GetEnvironmentVariable("POSTGRESQL_PASSWORD")}; ";

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

builder.Services.AddDbContext<WeatherDbContext>(
    options => options.UseNpgsql(connectionString));

builder.Services.AddScoped<IWeatherScraper, WeatherScraper>();

builder.Services.AddTransient<DataSeeder>();

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddAutoMapper(typeof(Program));

builder.Services.AddCors(options =>
{
    options.AddPolicy(MyAllowSpecificOrigins,
        policy =>
        {
            // policy.WithOrigins("http://localhost:5173");
            policy
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowAnyOrigin();
            /* policy.AllowAnyOrigin()
           .AllowAnyMethod()
           .AllowAnyHeader(); */
        });
});

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

app.UseUserSignInCheckedMiddleware();

app.UseHttpsRedirection();

app.UseOptions();

app.UseCors(MyAllowSpecificOrigins);

app.UseAuthorization();

app.MapControllers();

app.Run();
