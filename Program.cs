using EquipeTop14.Service;
using EquipeTop14.Authentication;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Ajouter des services au conteneur
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "API Top 14", Version = "v1" });

            // Définir le schéma d'authentification de base pour Swagger
            c.AddSecurityDefinition("BasicAuthentication", new OpenApiSecurityScheme
            {
                Type = SecuritySchemeType.Http,
                Scheme = "basic",
                Description = "Utilisation de l'authentification de base."
            });

            // Ajouter le schéma d'authentification requis à Swagger
            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "BasicAuthentication"
                        }
                    },
                    Array.Empty<string>()
                }
            });
        });

        // Ajouter le service UserService
        builder.Services.AddSingleton<IUserService, UserService>();

        builder.Services.AddAuthentication("BasicAuthentication")
            .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("BasicAuthentication", null);

        var app = builder.Build();

        // Configurer le pipeline HTTP
        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseHttpsRedirection();

        // Activer l'authentification
        app.UseAuthentication();

        app.UseAuthorization();

        app.MapControllers();

        // Activer Swagger et Swagger UI
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "API Top 14");
            // Spécifier que Swagger doit utiliser l'authentification de base
            c.DisplayOperationId();
            c.DisplayRequestDuration();
        });

        app.Run();
    }
}



