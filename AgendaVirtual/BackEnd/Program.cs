using BackEnd.Services.Implementations;
using BackEnd.Services.Interfaces;
using DAL.Implementations;
using DAL.Interfaces;
using Entities.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Diagnostics;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

#region BD
builder.Services.AddDbContext<AgendaVirtualContext>(optionsAction =>
                    optionsAction
                    .UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))

    );

builder.Services.AddDbContext<AuthDBContext>(optionsAction =>
                    optionsAction
                    .UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))

    );

#endregion


#region Identity

builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<AuthDBContext>()
    .AddDefaultTokenProviders();

builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 3;

});
#endregion

#region  JWT
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;

})

    .AddJwtBearer(options =>
    {
        options.SaveToken = true;
        options.RequireHttpsMetadata = false;
        options.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidAudience = builder.Configuration["JWT:ValidAudience"],
            ValidIssuer = builder.Configuration["JWT:ValidIssuer"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Secret"]))
        };
    });



#endregion

#region DI

builder.Services.AddScoped<IUnidadDeTrabajo, UnidadDeTrabajo>();

builder.Services.AddScoped<IUsuarioService, UsuarioService>();
builder.Services.AddScoped<IUsuarioDAL, UsuarioDAL>();

builder.Services.AddScoped<IEquipoService, EquipoService>();
builder.Services.AddScoped<IEquipoDAL, EquipoDAL>();

builder.Services.AddScoped<IUsuarioEquipoService, UsuarioEquipoService>();
builder.Services.AddScoped<IUsuarioEquipoDAL, UsuarioEquipoDAL>();

builder.Services.AddScoped<ITareaService, TareaService>();
builder.Services.AddScoped<ITareaDAL, TareaDAL>();

builder.Services.AddScoped<IRecordatorioService, RecordatorioService>();
builder.Services.AddScoped<IRecordatorioDAL, RecordatorioDAL>();

builder.Services.AddScoped<IEventoService, EventoService>();
builder.Services.AddScoped<IEventoDAL, EventoDAL>();

builder.Services.AddScoped<IComentarioService, ComentarioService>();
builder.Services.AddScoped<IComentarioDAL, ComentarioDAL>();

builder.Services.AddScoped<ITokenService, TokenService>();

#endregion

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseAuthentication();
app.UseMiddleware<ApiKeyMiddleware>();
app.UseAuthorization();

app.MapControllers();

app.Use(async (context, next) =>
{
    var user = context.User;
    if (user.Identity != null && user.Identity.IsAuthenticated)
    {
        Debug.WriteLine($"********************************************Usuario autenticado: {user.Identity.Name}");
    }
    else
    {
        Debug.WriteLine("*****************************************************No autenticado");
    }
    await next();
});

app.Run();
