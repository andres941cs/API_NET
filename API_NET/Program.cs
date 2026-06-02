using System.Text;
using API_NET.Context;
using API_NET.Utilities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

// A PARTIR DEL BUILDER SE CONSTRUYE UNA INSTANCIA DE LA APLICACIÓN
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers(); // PERMITE USAR CONTROLADORES
builder.Services.AddEndpointsApiExplorer(); // PERMITE USAR ENDPOINTS
builder.Services.AddSwaggerGen(); // GENERA LA DOCUMENTACION
builder.Services.AddDbContext<AppDb>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));// CONEXION CON LA BBDD
//builder.Services.AddSqlite<AppDb>(builder.Configuration.GetConnectionString("DefaultConnection"));

// comando de nuget: 'Add-Migration nameMigration' para generar las migraciones.
// comando de nuget: 'Update-Database' Initial Genera la bbdd.

builder.Services.AddSingleton<Utils>();
builder.Services.AddAuthentication(config =>
{
    config.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    config.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(config =>
{
    config.RequireHttpsMetadata = false;
    config.SaveToken = true;
    config.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        ValidateIssuer = false, // SE LE AÑADE EL URL DEL API
        ValidateAudience = false,
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero,
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"]!)
        )
    };
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
/* Configuración de Middlewares */
app.UseAuthentication();// Importante: antes de Authorization
app.UseAuthorization();

/* Mapeo de Rutas */
app.MapControllers();

app.Run();


/* ANOTACIONES */
// PAQUETES NUGET -> ENTITY FRAMEWORK: EF_CORE, EF_SQLITE, TOOLS Y JWTBEARER