using Backend.Enums;
using Backend.Models;
using Backend.Repository;
using Backend.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add db
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

var dataSourceBuilder = new Npgsql.NpgsqlDataSourceBuilder(connectionString);

dataSourceBuilder.MapEnum<Result>("result");

var dataSource = dataSourceBuilder.Build();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(dataSource, o => o.MapEnum<Result>("result")));


// Add services to the container.

builder.Services.AddControllers();

// Add service layer

builder.Services.AddHttpClient<IHuggingFaceService, HuggingFaceService>();
builder.Services.AddScoped<IHistoryService, HistoryService>();

// Register history repository
builder.Services.AddScoped<IHistoryRepository, HistoryRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
