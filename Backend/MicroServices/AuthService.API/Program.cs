using AuthService.API;
using AuthService.Infrastructure.IdentitySeed;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddApicontrollerServices(builder.Configuration, builder.Environment);
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Auth Service API v1");
        options.RoutePrefix = "swagger"; 
    });
}
//SeedData
using (var scope = app.Services.CreateScope())
{
    await IdentitySeed.SeedSuperAdminAsync(scope.ServiceProvider);
}
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAuthentication();
app.UseAuthorization();
app.UseCors("AllowAll");
app.MapControllers();
app.Run();
