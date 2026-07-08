

using Marten;
using Microsoft.OpenApi.Models;
using OptimumSolutions.Logging.Extensions;
using OptimumSolutions.Logging.Models;
using BuildingBlocks.Behaviors;
using OptimumSolutions.Logging.Behaviors;
using BuildingBlocks.Exceptions.Handlers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
// Add services to the container.
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
});

//add logging 
builder.Services.AddSharedLogging(new LoggingOptions
{
    ApplicationName = "Catalog.API",
    EnableSeq = true,
    SeqURL = builder.Configuration["Serilog:SeqServerUrl"] ?? "http://localhost:5341",
    EnableConsole = true,
});
builder.Services.AddMvc();
var assembly = typeof(Program).Assembly;
builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssembly(assembly);
    config.AddOpenBehavior(typeof(LoggingBehavior<,>));
    config.AddOpenBehavior(typeof(ValidationBehavior<,>));
});
builder.Services.AddValidatorsFromAssembly(assembly);
builder.Services.AddCarter();
builder.Services.AddMarten(options =>
{
    options.Connection(builder.Configuration.GetConnectionString("Database")!);
}).UseLightweightSessions();

builder.Services.AddExceptionHandler<CustomExceptionHandler>();



var app = builder.Build();

app.MapSwagger();
app.MapCarter();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(); // Access via https://localhost:xxxx/swagger
}
app.Run();
