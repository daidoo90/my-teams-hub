using Duende.IdentityServer.Models;

var builder = WebApplication.CreateBuilder(args);

//builder.Services
//    .AddScoped<IUserService, UserService>()
//    .AddScoped<ICryptoService, CryptoService>()
//    .AddScoped<IIdentityService, IdentityService>()
//    .AddDbContext();

builder.Services
    .AddIdentityServer(options =>
    {
        options.IssuerUri = "https://engagement-hub-identity-server";
    })
    .AddInMemoryClients(
    [
        new()
        {
            ClientId = "client",
            AllowedGrantTypes = GrantTypes.ClientCredentials,
            ClientSecrets =
            {
                new Secret("secret".Sha256())
            },
            AllowedScopes = { "organization-api" }
        }
    ])
    .AddInMemoryApiScopes(
    [
        new ApiScope("organization-api")
    ]);


var app = builder.Build();

app.UseIdentityServer();
app.MapGet("/", () => "IdentityServer is running");

//app.Services.ApplyMigrations();

//app.MapPost("/login", async ([FromBody] LoginRequestDto loginRequestDto,
//    IUserService userService,
//    IIdentityService identityService,
//    CancellationToken cancellationToken) =>
//{
//    var userData = await userService.TryGetAsync(loginRequestDto.Email, loginRequestDto.Password, cancellationToken);
//    //        if (serviceResult.HasFailed)
//    //        {
//    //            return ApiBadRequest.WithErrorCode(serviceResult.ErrorCode!);
//    //        }

//    var tokenServiceResult = identityService.GetToken(userData);

//    return tokenServiceResult;
//});

app.Run();
