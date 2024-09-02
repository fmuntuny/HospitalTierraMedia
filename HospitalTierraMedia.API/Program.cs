using HospitalTierraMedia.Models;
using HospitalTierraMedia.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//JWT
builder.Configuration.AddJsonFile("appsettings.json");
var secretKey = builder.Configuration.GetSection("JWT").GetSection("SecretKey").ToString();
var keyBytes = Encoding.UTF8.GetBytes(secretKey);
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
        IssuerSigningKey = new SymmetricSecurityKey(keyBytes),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSwaggerGen(s =>
s.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
{
    In = ParameterLocation.Header,
    Description = "Ingresar Token",
    Name = "Autorización",
    Type = SecuritySchemeType.Http,
    BearerFormat = "JWT",
    Scheme = "bearer"
}));
builder.Services.AddSwaggerGen(w =>
w.AddSecurityRequirement(new OpenApiSecurityRequirement {
    { new OpenApiSecurityScheme{
    Reference=new OpenApiReference{
    Type=ReferenceType.SecurityScheme,
    Id="Bearer"
    }
    },
    new string[]{}
    }
}));
//INYECCION DE DEPENDENCIAS PARA LA BD
builder.Services.Configure<DataBaseSettings>(builder.Configuration.GetSection("HospitalDB"));
//INYECCION DE DEPENDENCIAS PARA LOS SERVICIOS
builder.Services.AddTransient<IPacienteService, PacienteService>();
builder.Services.AddTransient<IUsuarioService, UsuarioService>();

var app = builder.Build();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

//JWT
app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
