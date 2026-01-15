using Ocelot.DependencyInjection;
using Ocelot.Middleware;
var builder = WebApplication.CreateBuilder(args);
builder.Configuration
    .AddJsonFile("ocelot.json", optional: false, reloadOnChange: true)
    .AddJsonFile("ocelot.Swagger.json", optional: false, reloadOnChange: true);
// Add services to the container.

builder.Services.AddControllers();
// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// Ocelot + SwaggerForOcelot
builder.Services.AddOcelot(builder.Configuration);
builder.Services.AddSwaggerForOcelot(builder.Configuration);
var app = builder.Build();

// Swagger UI của Gateway
app.UseSwagger();
app.UseSwaggerForOcelotUI(opt =>
{
    opt.PathToSwaggerGenerator = "/swagger/docs";
});

await app.UseOcelot();

app.Run();
