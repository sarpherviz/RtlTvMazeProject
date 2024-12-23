using RtlTvMaze.Services;
using RtlTvMaze.Services.Implementations;
using RtlTvMaze.Data.Infrastructure;
using RtlTvMaze.Repository.Interface;
using RtlTvMaze.Repository;
using Hangfire;
using RtlTvMaze.BackgroundService.Interface;
using RtlTvMaze.BackgroundService;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();

builder.Services.AddScoped<ITvShowService, TvShowService>();
builder.Services.AddScoped<ITvShowsRepository, TvShowsRepository>();
builder.Services.AddScoped<ITvMazeBackgroundService, TvMazeBackgroundService>();

DatabaseManagementService.AddDatabaseSettings(builder.Services, builder.Configuration);

HangfireSettings.ConfigureHangfire(builder.Services, builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHangfireDashboard();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

BackgroundJob.Enqueue<ITvMazeBackgroundService>(service => service.AddTvMazeData());

app.Run();
