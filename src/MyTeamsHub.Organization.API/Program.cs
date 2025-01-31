using MyTeamsHub.Organization.API.Configurations;

try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.ConfigureServices();

    builder
        .Build()
        .UseWebApiPipeline()
        .Run();
}
catch (Exception exc)
{
    // TODO
}
finally
{
    // TODO
}