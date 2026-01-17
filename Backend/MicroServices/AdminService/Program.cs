var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddSwaggerGen();
builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Admin Service API v1");
    options.RoutePrefix = "swagger";
});
// Configure the HTTP request pipeline.

app.UseAuthorization();

app.MapControllers();

app.Run();
