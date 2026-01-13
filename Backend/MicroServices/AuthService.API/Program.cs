using AuthService.API;
using AuthService.API.Middleware;
using AuthService.Infrastructure.IdentitySeed;
using Microsoft.OpenApi.Models;
using Serilog; // Thêm dòng này

// Khởi tạo Serilog logger sớm để bắt cả log từ host startup
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information() // ⬅️ ĐỪNG dùng Debug
    .MinimumLevel.Override("Microsoft", Serilog.Events.LogEventLevel.Warning)
    .MinimumLevel.Override("Microsoft.EntityFrameworkCore", Serilog.Events.LogEventLevel.Warning)
    .MinimumLevel.Override("Microsoft.AspNetCore", Serilog.Events.LogEventLevel.Warning)
    .WriteTo.Console()
    .WriteTo.File(
    "logs/log-.txt",
    rollingInterval: RollingInterval.Day) // Ghi log ra file theo ngày
    .CreateLogger();

try
{
    Log.Information("Starting web host");
    var builder = WebApplication.CreateBuilder(args);
    builder.Host.UseSerilog();
    builder.Services.AddControllers();

    builder.Services.AddApicontrollerServices(builder.Configuration, builder.Environment);
    var app = builder.Build();

    app.UseMiddleware<GlobalExceptionMiddleware>();

    //if (app.Environment.IsDevelopment())
    //{
    //    app.UseSwagger();
    //    app.UseSwaggerUI(options =>
    //    {
    //        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Auth Service API v1");
    //        options.RoutePrefix = "swagger"; 
    //    });
    //}
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Auth Service API v1");
        options.RoutePrefix = "swagger";
    });
    //SeedData
    using (var scope = app.Services.CreateScope())
    {
        await IdentitySeed.SeedSuperAdminAsync(scope.ServiceProvider);
    }
    app.UseHttpsRedirection();

    //app.UseStaticFiles();

    app.UseCors("AllowAll");

    app.UseRouting();

    app.UseAuthentication();
    app.UseAuthorization();



    app.MapControllers();

    app.Run();
}catch(Exception ex)
{
    Log.Fatal(ex, "Host terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}
