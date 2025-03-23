using System.Reflection;

using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.OpenApi.Models;

using Swashbuckle.AspNetCore.Filters;

namespace MyTeamsHub.Organization.API.Configurations;

internal static class SwaggerConfiguration
{
    private const string OpenApiTitle = "My Teams Hub API";

    internal static IServiceCollection AddSwagger(this IServiceCollection services)
    {
        //services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        //.AddJwtBearer(options =>
        //{
        //    options.TokenValidationParameters = new TokenValidationParameters
        //    {
        //        ValidateIssuer = true,
        //        ValidateAudience = true,
        //        ValidateLifetime = true,
        //        ValidateIssuerSigningKey = true,
        //        ValidIssuer = "My Teams Hub Issuer",// Configuration["Jwt:Issuer"],
        //        ValidAudience = "My Teams Hub Audience", //Configuration["Jwt:Audience"],
        //        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("at Microsoft.IdentityModel.Tokens.SymmetricSignatureProvider..ctor(SecurityKey key, String algorithm, Boolean willCreateSignatures)"))//Configuration["Jwt:Key"]))
        //    };
        //});

        //services.AddAuthorization(options =>
        //{
        //    options.AddPolicy("Bearer", policy =>
        //    {
        //        policy.AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme);
        //        policy.RequireAuthenticatedUser();
        //    });
        //});

        return services
            .AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = OpenApiTitle, Version = "v1" });
                c.ResolveConflictingActions(descriptions => descriptions.First());
                c.EnableAnnotations();

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    BearerFormat = "JWT",
                    Scheme = "Bearer"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });

                string filePath = System.IO.Path.Combine(AppContext.BaseDirectory, $"{Assembly.GetEntryAssembly()?.GetName().Name}.xml");
                c.IncludeXmlComments(filePath);
                c.CustomSchemaIds(type => type.ToString());
            })
            .AddSwaggerExamplesFromAssemblyOf(typeof(SwaggerConfiguration));
    }

    /// <summary>
    /// Register Swagger midleware
    /// </summary>
    internal static WebApplication ConfigureSwagger(this WebApplication app)
    {
        app.UseSwagger(c =>
        {
            //if (!app.Environment.IsDevelopment())
            //{
            //    c.PreSerializeFilters.Add((swaggerDoc, httpReq) =>
            //    {
            //        swaggerDoc.Servers = new List<OpenApiServer>
            //        {
            //                    new ()
            //                    {
            //                        Url = $"{httpReq.Scheme}://{httpReq.Host.Value}/api"
            //                    }
            //        };
            //    });
            //}
        })
        .UseSwaggerUI(c =>
        {
            c.RoutePrefix = string.Empty;
            c.DocumentTitle = OpenApiTitle;

            var versionProvider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();
            foreach (var description in versionProvider.ApiVersionDescriptions.Reverse())
            {
                c.SwaggerEndpoint($"swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
            }
        });

        return app;
    }
}
