using RtlTvMaze.Data;
using RtlTvMaze.Services;
using RtlTvMaze.Services.Implementations;
using MongoDB.Driver;
using RtlTvMaze.Data.Infrastructure;
using RtlTvMaze.Repository.Interface;
using RtlTvMaze.Repository;
using Hangfire;
using Hangfire.Mongo;
using Hangfire.Mongo.Migration.Strategies;
using Hangfire.Mongo.Migration.Strategies.Backup;
using RtlTvMaze.BackgroundService.Interface;
using RtlTvMaze.BackgroundService;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddScoped<ITvShowService, TvShowService>();
builder.Services.AddScoped<ITvShowsRepository, TvShowsRepository>();
builder.Services.AddScoped<ITvMazeBackgroundService, TvMazeBackgroundService>();

DatabaseManagementService.AddDatabaseSettings(builder.Services, builder.Configuration);

var mongoUrlBuilder = new MongoUrlBuilder(string.Concat(
    builder.Configuration.GetSection(nameof(MongoDbSettings) + ":" + MongoDbSettings.ConnectionStringValue).Value, 
    builder.Configuration.GetSection(nameof(MongoDbSettings) + ":" + MongoDbSettings.DatabaseValue).Value));
var mongoClient = new MongoClient(mongoUrlBuilder.ToMongoUrl());

builder.Services.AddHangfire(configuration => configuration
    .SetDataCompatibilityLevel(CompatibilityLevel.Version_170).UseSimpleAssemblyNameTypeSerializer()
    .UseRecommendedSerializerSettings()
    .UseMongoStorage(mongoClient, mongoUrlBuilder.DatabaseName, new MongoStorageOptions
    {
        MigrationOptions = new MongoMigrationOptions
        {
            MigrationStrategy = new MigrateMongoMigrationStrategy(),
            BackupStrategy = new CollectionMongoBackupStrategy()
        },
        Prefix = "hangfire.mongo",
        CheckConnection = true
    })
);

builder.Services.AddHangfireServer();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

//hangfire

app.UseHangfireDashboard();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

BackgroundJob.Enqueue<ITvMazeBackgroundService>(service => service.AddTvMazeData());

app.Run();
