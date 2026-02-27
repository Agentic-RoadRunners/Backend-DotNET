
using Microsoft.OpenApi.Models;
using SafeRoad.WebApi.Swagger;

namespace SafeRoad.WebApi.Extensions;

public static class SwaggerExtension
{
    public static IServiceCollection AddSwaggerConfiguration(this IServiceCollection services)
    {
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "SafeRoad API",
                Version = "v1",
                Description = "Crowdsourced Traffic Safety Platform API. Includes incident reporting, verification, comments, analytics, and admin management endpoints.",
                Contact = new OpenApiContact
                {
                    Name = "SafeRoad Team",
                    Email = "info@saferoad.com"
                }
            });

            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "Enter your JWT token. Example: eyJhbGciOiJIUzI1NiIs..."
            });

            options.AddSecurityRequirement(new OpenApiSecurityRequirement
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

            // ── Schema filters: realistic examples + enum descriptions ──
            options.SchemaFilter<SwaggerExampleSchemaFilter>();
            options.SchemaFilter<EnumSchemaFilter>();

            var xmlFile = "SafeRoad.WebApi.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            if (File.Exists(xmlPath))
                options.IncludeXmlComments(xmlPath);
        });

        return services;
    }
}