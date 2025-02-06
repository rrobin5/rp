using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using rp_api.DataBase;
using rp_api.Helper;
using rp_api.Mapper;
using rp_api.Middleware;
using rp_api.Repository;
using rp_api.Service;
using rp_api.Token;
using System.Net;
using System.Text;

System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;


var builder = WebApplication.CreateBuilder(args);

// CORS

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy => // modificar al desplegar el front
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// JWT configuration

var jwtIssuer = Environment.GetEnvironmentVariable("JWT_ISSUER")
                ?? builder.Configuration["Jwt:Issuer"];

var jwtAudience = Environment.GetEnvironmentVariable("JWT_AUDIENCE")
                  ?? builder.Configuration["Jwt:Audience"];

var jwtSecretKey = Environment.GetEnvironmentVariable("JWT_SECRET_KEY")
                   ?? builder.Configuration["Jwt:SecretKey"];


builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false; // solo en desarrollo, para producción debería ser true
        options.SaveToken = true;
        options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtIssuer,  
            ValidAudience = jwtAudience,  
            IssuerSigningKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtSecretKey))
        };
        options.Events = new JwtBearerEvents
        {
            OnTokenValidated = context =>
            {
                var claims = context.Principal.Claims.Select(c => new { c.Type, c.Value });
                foreach (var claim in claims)
                {
                    Console.WriteLine($"{claim.Type}: {claim.Value}");
                }
                return Task.CompletedTask;
            }
        };

    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("UserIdPolicy", policy =>
        policy.RequireClaim("id")); 
});

builder.Services.AddAutoMapper(typeof(MappingProfile));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Mongo DB

var connectionString = Environment.GetEnvironmentVariable("MONGO_CONNECTION_STRING")
                                ?? builder.Configuration["MongoDb:ConnectionString"];
var databaseName = Environment.GetEnvironmentVariable("MONGO_DATABASE_NAME")
                           ?? builder.Configuration["MongoDb:DatabaseName"];


builder.Services.Configure<MongoSettings>(options =>
{
    options.ConnectionString = connectionString;
    options.DatabaseName = databaseName;
});

builder.Services.AddSingleton<IMongoClient, MongoClient>(sp =>
{
    var settings = sp.GetRequiredService<IOptions<MongoSettings>>().Value;
    return new MongoClient(settings.ConnectionString);
});

//Services

builder.Services.AddScoped<IUserRepository, UserMongoRepository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<IRoleRepository, RoleMongoRepository>();
builder.Services.AddScoped<IHelper, Helper>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddSingleton<IAuthorizationHandler, UserIdClaimHandler>();

var app = builder.Build();

app.UseCors("AllowAll");

app.UseMiddleware<ErrorHandlingMiddleware>();


app.UseSwagger();
app.UseSwaggerUI();


app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
