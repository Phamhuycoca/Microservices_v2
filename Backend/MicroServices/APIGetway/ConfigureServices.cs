using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using System.Text.Json;

namespace APIGetway
{
    public static class ConfigureServices
    {
        public static IServiceCollection AddApicontrollerServices(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment env)
        {
            services.AddHttpContextAccessor();
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
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                 .AddJwtBearer("GatewayAuth",options =>
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
            services.Configure<JwtOptions>(configuration.GetSection("Jwt"));
            return services;
        }
    }
}
