using MongoDB.Driver;
using MyTeamsHub.ReviewAPI.Services;
using MyTeamsHub.ReviewAPI.Settings;

var builder = WebApplication.CreateBuilder(args);

var mongoDbSection = builder.Configuration.GetSection("MongoDB");

builder
    .Services
    .Configure<MongoDBSettings>(builder.Configuration.GetSection("MongoDB"));

builder.Services.AddSingleton<IMongoClient>(sp =>
{
    var settings = mongoDbSection.Get<MongoDBSettings>();
    return new MongoClient(settings!.ConnectionString);
});

builder
    .Services
    .AddSingleton(sp =>
    {
        var client = sp.GetRequiredService<IMongoClient>();
        var settings = mongoDbSection.Get<MongoDBSettings>();
        return client.GetDatabase(settings!.DatabaseName);
    })
    .AddScoped<IReviewService, ReviewService>();

var app = builder.Build();

app.UseHttpsRedirection();

app.MapGet("/reviews", async (IReviewService reviewService) =>
{
    var reviews = await reviewService.GetAllAsync();
    return reviews;
});

app.Run();