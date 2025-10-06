using Microsoft.EntityFrameworkCore;
using Delivery.Data;
using Npgsql.EntityFrameworkCore.PostgreSQL;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// CORS configuration
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowedOrigins", policy =>
    {
        policy.WithOrigins("http://localhost:3000", "https://localhost:3000") // Add your frontend URLs
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

// Rate Limiting configuration (implementação simplificada)
// TODO: Implementar rate limiting com middleware customizado ou usar pacote de terceiros como AspNetCoreRateLimit

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        // Configuração para lidar com ciclos de referência
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
    });


// Carrega variáveis do arquivo .env antes de qualquer configuração
DotNetEnv.Env.Load();

// Monta a connection string dinamicamente usando variáveis de ambiente
string dbHost = Environment.GetEnvironmentVariable("DB_HOST") ?? "localhost";
string dbPort = Environment.GetEnvironmentVariable("DB_PORT") ?? "5432";
string dbName = Environment.GetEnvironmentVariable("DB_NAME") ?? "postgres";
string dbUser = Environment.GetEnvironmentVariable("DB_USER") ?? "postgres";
string dbPassword = Environment.GetEnvironmentVariable("DB_PASSWORD") ?? "";
string dbSslMode = Environment.GetEnvironmentVariable("DB_SSLMODE") ?? "Require";

string connectionString = $"Host={dbHost};Port={dbPort};Database={dbName};Username={dbUser};Password={dbPassword};SslMode={dbSslMode}";

builder.Services.AddDbContext<Delivery.Data.DeliveryContext>(options =>
    options.UseNpgsql(connectionString));

// AutoMapper
builder.Services.AddAutoMapper(typeof(Delivery.Mapping.AutoMapperProfile));

// JWT Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = "JwtBearer";
    options.DefaultChallengeScheme = "JwtBearer";
})
    .AddJwtBearer("JwtBearer", options =>
    {
        options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!)),
            RoleClaimType = "http://schemas.microsoft.com/ws/2008/06/identity/claims/role"
        };
    });

// Adiciona os serviços de autorização necessários
builder.Services.AddAuthorization();
builder.Services.AddScoped<Delivery.Services.Interfaces.IAddressService, Delivery.Services.AddressService>();
builder.Services.AddScoped<Delivery.Repositories.Interfaces.IAddressRepository, Delivery.Repositories.AddressRepository>();
builder.Services.AddScoped<Delivery.Services.Interfaces.IEstablishmentService, Delivery.Services.EstablishmentService>();
builder.Services.AddScoped<Delivery.Repositories.Interfaces.IEstablishmentRepository, Delivery.Repositories.EstablishmentRepository>();
builder.Services.AddScoped<Delivery.Services.Interfaces.IUserService, Delivery.Services.UserService>();
builder.Services.AddScoped<Delivery.Repositories.Interfaces.IUserRepository, Delivery.Repositories.UserRepository>();
builder.Services.AddScoped<Delivery.Services.Interfaces.ICategoryService, Delivery.Services.CategoryService>();
builder.Services.AddScoped<Delivery.Repositories.Interfaces.ICategoryRepository, Delivery.Repositories.CategoryRepository>();
builder.Services.AddScoped<Delivery.Services.Interfaces.IProductService, Delivery.Services.ProductService>();
builder.Services.AddScoped<Delivery.Repositories.Interfaces.IProductRepository, Delivery.Repositories.ProductRepository>();
builder.Services.AddScoped<Delivery.Services.Interfaces.IOrderService, Delivery.Services.OrderService>();
builder.Services.AddScoped<Delivery.Repositories.Interfaces.IOrderRepository, Delivery.Repositories.OrderRepository>();
builder.Services.AddScoped<Delivery.Services.Interfaces.IOrderService, Delivery.Services.OrderService>();
builder.Services.AddScoped<Delivery.Repositories.Interfaces.IOrderRepository, Delivery.Repositories.OrderRepository>();

var app = builder.Build();
// Adiciona o middleware global de tratamento de exceções
app.UseMiddleware<Delivery.Middlewares.ExceptionHandlingMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Security Headers
app.Use(async (context, next) =>
{
    context.Response.Headers["X-Content-Type-Options"] = "nosniff";
    context.Response.Headers["X-Frame-Options"] = "DENY";
    context.Response.Headers["X-XSS-Protection"] = "1; mode=block";
    if (context.Request.IsHttps)
    {
        context.Response.Headers["Strict-Transport-Security"] = "max-age=31536000; includeSubDomains";
    }
    context.Response.Headers["Content-Security-Policy"] = "default-src 'self'; script-src 'self' 'unsafe-inline'; style-src 'self' 'unsafe-inline'";
    await next();
});

// Enable HTTPS Redirection
if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

// CORS
app.UseCors("AllowedOrigins");

// Rate Limiting será implementado em versão futura

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
// Carrega variáveis do arquivo .env
DotNetEnv.Env.Load();
