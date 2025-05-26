using Infrastructure.Appdbcontext;
using Infrastructure.Contracts;
using Infrastructure.Middelwares;
using Infrastructure.Models;
using Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Reflection;
using System.Text;
using UserManagmentService.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddIdentity<User, UserRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddMemoryCache();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("UserServiceConnection")));


builder.Services.AddCors(options=>
{
options.AddDefaultPolicy(builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
           
         
});
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };
});

builder.Services.AddScoped<IRegisterPreviliege, RegisterPrevilages>();
builder.Services.AddScoped<ITokenGeneretor, GenereteTokenService>();
builder.Services.AddScoped<IAuthorization, AuthorizationService>();
builder.Services.AddScoped<IRedisCache, RedisCacheService>();
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration["ConnectionStrings:RedisCache"];
});
builder.Services.AddControllers(op =>
{
    op.Filters.Add<CustomAuthorizeAttribute>();
});
builder.Services.AddControllers();
var app = builder.Build();
using (var scope = app.Services.CreateAsyncScope())
{
    var serviceProvider = scope.ServiceProvider;
    var controllerCollector = serviceProvider.GetRequiredService<IRegisterPreviliege>();
    var assembly = Assembly.GetExecutingAssembly();
    controllerCollector.RegisterPrivileges(assembly);
}
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors();

app.UseHttpsRedirection();

//app.UseMiddleware<AuthorizationMiddlWare>();

app.UseAuthorization();


app.MapControllers();


app.Run();
