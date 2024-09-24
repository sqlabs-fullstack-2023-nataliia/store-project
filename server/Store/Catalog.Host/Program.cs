using System.Text;
using Catalog.Host.Configurations;
using Catalog.Host.DbContextData;
using Catalog.Host.DbContextData.Entities;
using Catalog.Host.Dto;
using Catalog.Host.Repositories;
using Catalog.Host.Repositories.Interfaces;
using Catalog.Host.Services;
using Catalog.Host.Services.Interfaces;
using ExceptionHandler;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;


var configuration = GetConfiguration();
var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .WriteTo.Console()
    .CreateLogger();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Store- Catalog HTTP API",
        Description = "The Store Catalog Service HTTP API"
    });
    
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter 'Bearer' followed by a space and the JWT token"
    });
    
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
            },
            new string[] {}
        }
    });
});

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        ValidIssuer = "test",
        ValidAudience = "test",
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("I24GRS8PzNlFpLzEz9jlSsRbMlB5z9XK")),
        ClockSkew = TimeSpan.Zero
    };
});

builder.Services.AddAuthorization(options =>
{
    options.DefaultPolicy = new AuthorizationPolicyBuilder()
            .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
            .RequireAuthenticatedUser()
            .Build();

});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();


builder.Services.Configure<CatalogConfigurations>(configuration);
builder.Services.AddDbContextFactory<ApplicationDbContext>(opts => opts.UseNpgsql(configuration["ConnectionString"]));

builder.Services.AddTransient<ICatalogItemRepository, CatalogItemRepository>();
builder.Services.AddTransient<IItemRepository, ItemRepository>();
builder.Services.AddTransient<ICatalogRepository<ItemBrand>, BrandsRepository>();
builder.Services.AddTransient<ICatalogRepository<ItemType>, TypeRepository>();
builder.Services.AddTransient<ICatalogRepository<ItemCategory>, CategoryRepository>();

builder.Services.AddTransient<ICatalogService<Item, ItemDto>, ItemService>();
builder.Services.AddTransient<ICatalogService<CatalogItem, CatalogItemDto>, CatalogItemService>();
builder.Services.AddTransient<ICatalogService<ItemBrand, BrandDto>, BrandService>();
builder.Services.AddTransient<ICatalogService<ItemType, TypeDto>, TypeService>();
builder.Services.AddTransient<ICatalogService<ItemCategory, CategoryDto>, CategoryService>();
builder.Services.AddTransient<IBffService, BffService>();


builder.Services.AddCors((options =>
{
    options.AddPolicy("default", policy =>
    {
        policy.WithOrigins("http://localhost:5173")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
}));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Store- Catalog HTTP API");
        c.DefaultModelsExpandDepth(-1);  // Optional: To disable the default models in Swagger UI
    });
}

app.UseMiddleware<GlobalExceptionHandler>();

app.UseRouting();

app.UseCors(builder =>
    builder
        .WithOrigins("http://localhost:5173")
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowCredentials()
);

app.UseAuthentication();
app.UseAuthorization();

app.UseCors("default");

app.UseEndpoints(endpoints =>
{
    endpoints.MapDefaultControllerRoute().RequireAuthorization("ApiScope");
    endpoints.MapControllers();//.RequireAuthorization -> for all controllers
});

CreateDbIfNotExists(app);

app.Run();

Log.CloseAndFlush();

IConfiguration GetConfiguration()
{
    var builderConfig = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json", false, true)
        .AddEnvironmentVariables();

    return builderConfig.Build();
}

void CreateDbIfNotExists(IHost host)
{
    using var scope = host.Services.CreateScope();
    var services = scope.ServiceProvider;
    try
    {
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        DbInitializer.Initialize(context).GetAwaiter().GetResult();
    }
    catch (Exception e)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(e, "An error occured creating DB");
    }
}

