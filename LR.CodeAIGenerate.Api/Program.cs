using System.Text;
using FluentValidation;
using LR.CodeAIGenerate.Business.Interfaces;
using LR.CodeAIGenerate.Data;
using LR.CodeAIGenerate.Data.Validators;
using LR.CodeAIGenerate.Domain.Modelos;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Configuração básica de JWT em memória (chave/sissuer/audience de exemplo)
var jwtIssuer = builder.Configuration["Jwt:Issuer"] ?? "LR.CodeAIGenerate";
var jwtAudience = builder.Configuration["Jwt:Audience"] ?? "LR.CodeAIGenerate.Audience";
var jwtSecret = builder.Configuration["Jwt:Secret"] ?? "chave-super-secreta-nao-usar-em-producao-12345";
var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret));

// Add services to the container.
builder.Services.AddControllers();

// DbContext (SQLite)
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection") ?? "Data Source=app.db"));

// FluentValidation
builder.Services.AddScoped<IValidator<Pessoa>, PessoaValidator>();

// Repositórios
builder.Services.AddScoped<IRepositorioPessoa, RepositorioPessoa>();

// Autenticação JWT
builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtIssuer,
            ValidAudience = jwtAudience,
            IssuerSigningKey = signingKey
        };
    });

// Autorização e policies
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("add-pessoa", policy =>
    {
        policy.RequireAuthenticatedUser();
        policy.RequireClaim("scope", "pessoa.add");
    });
});

// OpenAPI / Swagger com segurança JWT
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "LR.CodeAIGenerate API v1");
    });
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
