using BackEnd.Services.Implementations;
using BackEnd.Services.Interfaces;
using DAL.Implementations;
using DAL.Interfaces;
using Entities.Entities;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

#region DI
builder.Services.AddDbContext<AgendaVirtualContext>(optionsAction =>
                     optionsAction
                     .UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))

     );
builder.Services.AddScoped<IUnidadDeTrabajo, UnidadDeTrabajo>();

builder.Services.AddScoped<IUsuarioService, UsuarioService>();
builder.Services.AddScoped<IUsuarioDAL, UsuarioDAL>();

builder.Services.AddScoped<IEquipoService, EquipoService>();
builder.Services.AddScoped<IEquipoDAL, EquipoDAL>();

builder.Services.AddScoped<IUsuarioEquipoService, UsuarioEquipoService>();
builder.Services.AddScoped<IUsuarioEquipoDAL, UsuarioEquipoDAL>();

#endregion

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ApiKeyMiddleware>();
app.UseAuthorization();

app.MapControllers();


// Esto lo puse para probar la conexión porque aún no hay CRUDs para probar en el swagger, se puede borrar una vez los CRUDs estén hechos :)
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AgendaVirtualContext>();
    context.Database.EnsureCreated(); // Crea la base de datos si no existe
    Debug.WriteLine("*************************************************************** Conexión a la base de datos establecida correctamente.");
}

app.Run();
