var builder = WebApplication.CreateBuilder(args);

// Ajouter des services au conteneur
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var builder = WebApplication.CreateBuilder(args);

// Ajouter des services au conteneur
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Ajouter l'authentification basée sur le fichier JSON
builder.Services.AddAuthentication("JsonAuth")
    .AddScheme<AuthenticationSchemeOptions, JsonAuthenticationHandler>("JsonAuth", null);

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


