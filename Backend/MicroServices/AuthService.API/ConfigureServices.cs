using AuthService.Application;
using AuthService.Domain.Entites;
using AuthService.Infrastructure.AppContext;
using AuthService.Infrastructure.Configs;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using System.Text.Json;
namespace AuthService.API;

public static class ConfigureServices
{
    public static IServiceCollection AddApicontrollerServices(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment env)
    {
        services.AddHttpContextAccessor();
        services.AddDbContext<ApplicationDbContext>(builder => builder.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
        //Cấu hình config application
        services.AddApplicationServices();
        // Đăng ký SwaggerGen với cấu hình JWT Bearer
        // Swagger
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Auth Service API",
                Version = "v1",
                Description = "Authentication microservice"
            });

            // Cấu hình security để dùng JWT Bearer
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                In = Microsoft.OpenApi.Models.ParameterLocation.Header,
                Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                Description = "Nhập token dạng: Bearer {your token here}"
            });

            options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
            {
                {
                    new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                    {
                        Reference = new Microsoft.OpenApi.Models.OpenApiReference
                        {
                            Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    new string[] {}
                }
            });

        });
        /* services.AddAuthentication(options =>
         {
             options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
             options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
         })*/
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
             .AddJwtBearer(options =>
            {
                // inject JwtOptions từ DI
                var sp = services.BuildServiceProvider();
                var jwtOptions = sp.GetRequiredService<IOptions<JwtOptions>>().Value;

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtOptions.Issuer,
                    ValidAudience = jwtOptions.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.SecretKey))
                };

                options.Events = new JwtBearerEvents
                {
                    OnChallenge = context =>
                    {
                        context.HandleResponse();

                        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        context.Response.ContentType = "application/json";

                        var response = new
                        {
                            statusCode = 401,
                            message = "Bạn chưa đăng nhập hoặc token không hợp lệ",
                            traceId = context.HttpContext.TraceIdentifier
                        };

                        return context.Response.WriteAsync(
                            JsonSerializer.Serialize(response)
                        );
                    },

                    OnForbidden = context =>
                    {
                        context.Response.StatusCode = StatusCodes.Status403Forbidden;
                        context.Response.ContentType = "application/json";

                        var response = new
                        {
                            statusCode = 403,
                            message = "Bạn không có quyền truy cập",
                            traceId = context.HttpContext.TraceIdentifier
                        };

                        return context.Response.WriteAsync(
                            JsonSerializer.Serialize(response)
                        );
                    },

                    OnMessageReceived = context =>
                    {
                        var accessToken = context.Request.Query["access_token"].FirstOrDefault();
                        if (!string.IsNullOrEmpty(accessToken))
                        {
                            context.Token = accessToken;
                        }
                        return Task.CompletedTask;
                    }
                };
            });
        services.AddAuthorization();
        //Cấu hình cors
        services.AddCors(options =>
        {
            options.AddPolicy("AllowAll",
                 policy => policy

                                .WithOrigins()
                                .AllowAnyHeader()
                                .AllowAnyMethod()
                                .AllowCredentials());
        });
        services.AddMemoryCache();
        services.Configure<JwtOptions>(configuration.GetSection("Jwt"));
        //services.Configure<UrlWebOptions>(configuration.GetSection("url_web"));
        services.AddIdentityCore<nguoi_dung>(options =>
        {
            // Cấu hình password
            options.Password.RequireDigit = true;
            options.Password.RequireUppercase = false;
            options.Password.RequireLowercase = false;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequiredLength = 1;

            // User
            options.User.RequireUniqueEmail = true;

            // Lockout
            options.Lockout.AllowedForNewUsers = true;
            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
            options.Lockout.MaxFailedAccessAttempts = 5;
        })
        .AddRoles<IdentityRole<Guid>>()
        .AddEntityFrameworkStores<ApplicationDbContext>()
        .AddDefaultTokenProviders();


        services.ConfigureApplicationCookie(options =>
        {
            options.Cookie.Name = "AUTH_COOKIE";
            options.LoginPath = "/Auth/Auth_Login";
            options.Cookie.HttpOnly = true;
            options.Cookie.Domain = ".localhost";
            options.ExpireTimeSpan = TimeSpan.FromDays(7); // nhớ 7 ngày
            options.SlidingExpiration = true;
            if (env.IsDevelopment())
            {
                options.Cookie.SameSite = SameSiteMode.Lax;
                options.Cookie.SecurePolicy = CookieSecurePolicy.None;
            }
            else
            {
                options.Cookie.SameSite = SameSiteMode.None;
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
            }

        });
       

        return services;
    }
}