using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using MongoDB.Driver;
using rp_api.DataBase;
using rp_api.Helper;
using rp_api.Mapper;
using rp_api.Middleware;
using rp_api.Repository;
using rp_api.Service;
using rp_api.Token;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// JWT configuration
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
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SecretKey"]))
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

builder.Services.Configure<MongoSettings>(builder.Configuration.GetSection("MongoDb"));

builder.Services.AddSingleton<IMongoClient, MongoClient>(sp =>
{
    var settings = builder.Configuration.GetSection("MongoDb").Get<MongoSettings>();
    return new MongoClient(settings.ConnectionString);
});

builder.Services.AddScoped<IUserRepository, UserMongoRepository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<IRoleRepository, RoleMongoRepository>();
builder.Services.AddScoped<IHelper, Helper>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddSingleton<IAuthorizationHandler, UserIdClaimHandler>();

var app = builder.Build();

app.UseMiddleware<ErrorHandlingMiddleware>();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
