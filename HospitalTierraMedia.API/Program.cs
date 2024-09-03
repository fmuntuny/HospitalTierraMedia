using HospitalTierraMedia.Models;
using HospitalTierraMedia.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MongoDB.Driver;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
builder.Services.Configure<DataBaseSettings>(
    builder.Configuration.GetSection("HospitalDB"));

// Registrar IMongoClient como un singleton
builder.Services.AddSingleton<IMongoClient>(s =>
    new MongoClient(builder.Configuration.GetSection("HospitalDB").GetValue<string>("ConnectionString")));

// Configuración de JWT
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

// Agregar servicios
builder.Services.AddControllers();
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

// Inyección de dependencias para los servicios
builder.Services.AddTransient<IPacienteService, PacienteService>();
builder.Services.AddTransient<IUsuarioService, UsuarioService>();

var app = builder.Build();

// Inicializar la base de datos y usuario admin por defecto
InitializeDatabase(app.Services);

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();

void InitializeDatabase(IServiceProvider serviceProvider)
{
    // Obtener los servicios necesarios para MongoDB
    var mongoClient = serviceProvider.GetRequiredService<IMongoClient>();
    var dbName = builder.Configuration.GetSection("HospitalDB").GetValue<string>("DatabaseName");
    var db = mongoClient.GetDatabase(dbName);

    // Verificar y crear las colecciones necesarias
    var collectionNames = db.ListCollectionNames().ToList();

    if (!collectionNames.Contains("PacientesCollection"))
    {
        db.CreateCollection("PacientesCollection");
    }

    if (!collectionNames.Contains("UsuariosCollection"))
    {
        db.CreateCollection("UsuariosCollection");
    }

    // Verificar si existe un usuario administrador y crearlo si no existe
    var usuarioCollection = db.GetCollection<Usuario>("UsuariosCollection");
    var adminExists = usuarioCollection.Find(u => u.Rol == "admin").Any();

    if (!adminExists)
    {
        var adminUser = new Usuario
        {
            Nombre = "admin",
            Email = "admin@hospital.com",
            Contrasena = "admin",
            Rol = "admin",
            Activo = true
        };
        usuarioCollection.InsertOne(adminUser);
    }
}