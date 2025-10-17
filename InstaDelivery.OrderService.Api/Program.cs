using InstaDelivery.OrderService.Api.Constants;
using InstaDelivery.OrderService.Application;
using InstaDelivery.OrderService.Application.MapperProfiles;
using InstaDelivery.OrderService.Messaging;
using InstaDelivery.OrderService.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Identity.Web;
using Microsoft.OpenApi.Models;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.ConfigureRepositories(builder.Configuration);
        builder.Services.ConfigureApplicationServices();
        builder.Services.ConfigureMessagingServices();

        // Register AutoMapper
        builder.Services.AddAutoMapper(
            cfg => { },
            typeof(MapperProfile).Assembly
        );

        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();

        builder.Services.AddSwaggerGen(c =>
        {
            c.AddSecurityDefinition("bearer", new OpenApiSecurityScheme
            {
                Type = SecuritySchemeType.Http,
                Scheme = "bearer",
                BearerFormat = "JWT",
                Description = "JWT Bearer token obtained from Azure AD"
            });

            var tenant = builder.Configuration["AzureAd:TenantId"];
            var authorizeUrl = $"https://login.microsoftonline.com/{tenant}/oauth2/v2.0/authorize";
            var tokenUrl = $"https://login.microsoftonline.com/{tenant}/oauth2/v2.0/token";
            var scopes = builder.Configuration["SwaggerClient:Scopes"];

            c.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
            {
                Type = SecuritySchemeType.OAuth2,
                Flows = new OpenApiOAuthFlows
                {
                    AuthorizationCode = new OpenApiOAuthFlow
                    {
                        AuthorizationUrl = new Uri(authorizeUrl),
                        TokenUrl = new Uri(tokenUrl),
                        Scopes = new Dictionary<string, string>
                        {
                            {
                                scopes, "Access API as the signed-in user" 
                            }
                        }
                    }
                }
            });

            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme { Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "oauth2" } },
                    new[] { builder.Configuration["SwaggerClient:Scopes"] }
                }
            });
        });

        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("AzureAd"));

        builder.Services.AddAuthorization(options =>
        {
            options.AddPolicy(AuthPolicy.BasicAccess, policy =>
                  policy.RequireRole("User", "Admin"));

            options.AddPolicy(AuthPolicy.ElevatedAccess, policy =>
                  policy.RequireRole("Admin"));
        });




        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "API V1");
                c.OAuthClientId(builder.Configuration["SwaggerClient:ClientId"]);
                c.OAuthUsePkce(); 
            });
        }

        app.UseHttpsRedirection();

        app.UseAuthentication();

        app.UseAuthorization();



        app.MapControllers();

        app.Run();
    }
}