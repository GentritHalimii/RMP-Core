using Carter;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using RMP.Core.Host.Database;
using RMP.Core.Host.Entities.Identity;
using RMP.Core.Host.Exceptions;
using RMP.Core.Host.Extensions;
using RMP.Core.Host.Features.Rating.CreateRate;
using RMP.Core.Host.Features.Rating.CreateRate.Strategy;
using RMP.Core.Host.Features.Rating.Extension.PredictionService;
using RMP.Core.Host.Features.Rating.Extension.PredictionService.Interface;
using RMP.Core.Host.Features.User.Common;

var builder = WebApplication.CreateBuilder(args);

var assembly = typeof(Program).Assembly;

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddDefaultIdentity<UserEntity>()
    .AddRoles<Role>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();
builder.Services.Configure<DataProtectionTokenProviderOptions>(x => x.TokenLifespan = TimeSpan.FromMinutes(2));
builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 6;
    options.Password.RequiredUniqueChars = 2;
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    options.Lockout.MaxFailedAccessAttempts = 7;
    options.Lockout.AllowedForNewUsers = true;
    options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
    options.User.RequireUniqueEmail = false;
});

var provider = builder.Services.BuildServiceProvider();
var configuration = provider.GetRequiredService<IConfiguration>();
builder.Services.AddCors(options =>
{
    var frontendURL = configuration.GetValue<string>("frontend_url");
    options.AddDefaultPolicy(builder =>
    {
        builder.WithOrigins(frontendURL).AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
    }); 
});

builder.Services.AddScoped<ITokenGenerator, JwtTokenGenerator>();
builder.Services.AddTransient<RateHandlerStrategyResolver>();
builder.Services.AddTransient<ConcreteRateProfessorStrategy>();
builder.Services.AddTransient<ConcreteRateUniversityStrategy>();
builder.Services.AddTransient<IRateHandlerStrategy, ConcreteRateProfessorStrategy>();
builder.Services.AddTransient<IRateHandlerStrategy, ConcreteRateUniversityStrategy>();
builder.Services.AddGrpcClient<RMP.Core.Host.PredictionService.PredictionServiceClient>(o =>
{
    o.Address = new Uri("http://localhost:5195"); 
});
builder.Services.AddScoped<IPredictionService, PredictionService>();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Your API", Version = "v1" });

    var securityScheme = new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Description = "JWT Authorization header using the Bearer scheme.",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT"
    };
    c.AddSecurityDefinition("Bearer", securityScheme);

    var securityRequirement = new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    };
    c.AddSecurityRequirement(securityRequirement);
}); 
builder.Services.
    AddSQLDatabaseConfiguration(builder.Configuration)
    .AddMediatRConfiguration(assembly)
    .AddValidatorsFromAssembly(assembly)
    .AddCarter()
    .AddExceptionHandler<GlobalExceptionHandler>()
    .AddHealthChecksConfiguration();

builder.Services.AddControllers(options =>
{
    options.Filters.Add(new IgnoreAntiforgeryTokenAttribute());
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

    /*app.UseSwagger();
    app.UseSwaggerUI();*/

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    db.Database.Migrate();
}

app.UseCors();
app.UseHttpsRedirection();
app.UseHealthChecks();
app.MapCarter();
app.UseExceptionHandler(options => { });
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});
app.UseStaticFiles();

app.Run();