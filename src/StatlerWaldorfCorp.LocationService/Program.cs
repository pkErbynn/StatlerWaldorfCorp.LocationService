using Microsoft.EntityFrameworkCore;
using StatlerWaldorfCorp.LocationService.Persistence;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//builder.Services.AddDbContext<LocationDbContext>(options =>
//options.UseNpgsql(builder.Configuration.GetConnectionString("PostgresConnection")));

//builder.Services.AddScoped<ILocationRecordRepository, LocationRecordRepository>();
//builder.Services.AddScoped<ILocationRecordRepository, InMemoryLocationRecordRepository>();

var configuration = builder.Configuration;

//var logger = builder..Logging.CreateLogger<Program>();
using ILoggerFactory factory = LoggerFactory.Create(builder => builder.AddConsole());
ILogger logger = factory.CreateLogger("Program");

var transient = configuration.GetValue<bool?>("transient") ?? true;

if (transient)
{
    logger.LogInformation("Using transient location record repository.");
    builder.Services.AddScoped<ILocationRecordRepository, InMemoryLocationRecordRepository>();
}
else
{
    var connectionString = configuration.GetValue<string>("postgres:cstr");
    builder.Services.AddDbContext<LocationDbContext>(options =>
        options.UseNpgsql(connectionString));
    logger.LogInformation("Using '{0}' for DB connection string.", connectionString);
    builder.Services.AddScoped<ILocationRecordRepository, LocationRecordRepository>();
}

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
