using System.Text;
using BusinessObjects.Context;
using BusinessObjects.Entities;
using DAOs;
using eStore.Extensions;
using eStore.Middlewares;
using LoggerService;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NLog;
using Repositories.Implementation;
using Repositories.Interface;
using Services.Implementation;
using Services.Interface;
using Tools;

namespace eStore;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        LogManager.LoadConfiguration(string.Concat(Directory.GetCurrentDirectory(), "/nlog.config"));

        // Add services to the container.
        builder.Services.AddAuthorization();
        builder.Services.AddControllers();
        // builder.Services.AddScoped<ValidationFilterAttribute>();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddSingleton<ILoggerManager, LoggerManager>();
        ConfigurationManager configuration = builder.Configuration;
        builder.Services.AddDbContext<ApplicationDbContext>(options =>
        {
            var connectionString = configuration.GetConnectionString("eStore");
            options.UseNpgsql(connectionString);
        });
        
        // Add logging
        builder.Logging.AddConsole();
        
        builder.Services.Configure<ApiBehaviorOptions>(options =>
        {
            options.SuppressModelStateInvalidFilter = true;
        });

        #region JWT Authenthication
        builder.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(o =>
        {
            o.TokenValidationParameters = new TokenValidationParameters
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
        #endregion

        builder.Services.AddAutoMapper(typeof(Program));

        #region DAOs

        builder.Services.AddScoped<CategoryDao>();
        builder.Services.AddScoped<ProductDao>();
        builder.Services.AddScoped<OrderDao>();
        builder.Services.AddScoped<OrderDetailDao>();
        builder.Services.AddScoped<MemberDao>();

        #endregion

        #region Repositories

        builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
        builder.Services.AddScoped<IProductRepository, ProductRepository>();
        builder.Services.AddScoped<IOrderRepository, OrderRepository>();
        builder.Services.AddScoped<IOrderDetailRepository, OrderDetailRepository>();
        builder.Services.AddScoped<IMemberRepository, MemberRepository>();

        #endregion

        #region Services

        builder.Services.AddScoped<ICategoryService, CategoryService>();
        builder.Services.AddScoped<IProductService, ProductService>();
        builder.Services.AddScoped<IOrderService, OrderService>();
        builder.Services.AddScoped<IOrderDetailService, OrderDetailService>();
        builder.Services.AddScoped<IMemberService, MemberService>();

        #endregion

        #region Auth Services
        builder.Services.AddScoped<IAuthService, AuthService>();
        #endregion

        #region CORS

        builder.Services.AddCors(options =>
        {
            options.AddDefaultPolicy(builder =>
            {
                builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            });
        });

        #endregion

       

        var app = builder.Build();
        // Configure the HTTP request pipeline.
        var logger = app.Services.GetRequiredService<ILoggerManager>();
        app.UseMiddleware<ExceptionMiddleware>();
        app.ConfigureExceptionHandler(logger);
        #region Swagger
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "eStore-API-V1");
            c.RoutePrefix = "swagger";
        });


        #endregion
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        app.UseAuthorization();
        app.UseAuthentication();
        app.MapControllers();
        app.Run();
    }
}