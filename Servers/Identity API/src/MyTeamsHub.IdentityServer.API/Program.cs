using MyTeamsHub.IdentityServer.API.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddScoped<IUserService, UserService>()
    .AddScoped<ICryptoService, CryptoService>()
    .AddScoped<IIdentityService, IdentityService>();
//.AddDbContext();

var app = builder.Build();

//app.Services.ApplyMigrations();

app.Run();
