var builder = WebApplication.CreateBuilder(args);

// Escuchar en HTTP plano para que el emulador Android llegue vía 10.0.2.2
builder.WebHost.UseUrls("http://0.0.0.0:5000");

builder.Services.AddControllers();

// CORS abierto: la app del workshop corre en un emulador, no en un navegador con origen fijo
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
        policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});

var app = builder.Build();

app.UseCors();
app.MapControllers();

app.Run();
