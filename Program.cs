using Microsoft.AspNetCore.Authentication;

var builder = WebApplication.CreateBuilder(args);

// Ajouter des services au conteneur
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Ajouter l'authentification basée sur le fichier JSON

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
});

app.Run();
