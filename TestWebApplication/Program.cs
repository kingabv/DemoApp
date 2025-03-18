using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using TestWebApplication.Authentication;
using TestWebApplication.Extensions;
using TestWebApplication.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddDbContext<CompanyContext>(opt =>
{
    opt.UseSqlServer(connectionString: builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddAuthentication(authOptions =>
    {
        authOptions.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        authOptions.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        authOptions.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(jwtOptions =>
    {
        var key = builder.Configuration["JwtConfig:Key"];
        var keyBytes = Encoding.UTF8.GetBytes(key);
        jwtOptions.SaveToken = true;
        jwtOptions.TokenValidationParameters = new TokenValidationParameters
        {
            ValidIssuer = builder.Configuration["JwtConfig:Issuer"],
            ValidAudience = builder.Configuration["JwtConfig:Audience"],
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,         
            IssuerSigningKey = new SymmetricSecurityKey(keyBytes)
        };
       
    });

builder.Services.AddAuthorization();

builder.Services.AddSingleton(typeof(IJwtTokenManager), typeof(JwtTokenManager));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{   
    c.AddXmlDocumentationFiles();
    c.EnableAnnotations();    
    c.SwaggerDoc("v1", new OpenApiInfo
    { 
        Title = "TestWebApplication.WebAPI",
        Version = "v1"
    });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter here the token:"

    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header
            },
            new List<string>()
        }
     });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
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
