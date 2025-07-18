using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Sales.Context.Data;
using Sales.Context.Helpers;
using Sales.Library.Models;
using Sales.Context.Services;
using System.Text;
using Sales.Context.Contracts;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configue dbContext
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDb>(options => options.UseSqlServer(connectionString),
    ServiceLifetime.Transient);

// Configure Jwt Section
builder.Services.Configure<JwtSection>(builder.Configuration.GetSection("JwtSection"));
var jwtSection = builder.Configuration.GetSection(nameof(JwtSection)).Get<JwtSection>();


// Add authentications
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSection!.Issuer,
        ValidAudience = jwtSection.Audience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSection.Key!)),
        ClockSkew = TimeSpan.Zero,
    };
});

// register Cors policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("SalesManagement", policy =>
    {
        policy.WithOrigins("http://localhost:5219", "https://localhost:7175")
        .AllowAnyMethod()
        .AllowCredentials()
        .AllowAnyHeader();
    });
});

// register services
builder.Services.AddTransient<IDbService, DbService>();
builder.Services.AddTransient<IAccountService, AccountService>();
builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddTransient<IUserRoleService, UserRoleService>();
builder.Services.AddTransient<IMainSettingService, MainSettingService>();
builder.Services.AddTransient<ICategoryService, CategoryService>();
builder.Services.AddTransient<Responses>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseCors("SalesManagement");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
