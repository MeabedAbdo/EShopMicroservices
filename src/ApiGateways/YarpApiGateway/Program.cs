using Microsoft.AspNetCore.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

//register app services to container

builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));
builder.Services.AddRateLimiter(ratelimiteroptions =>
{
    ratelimiteroptions.AddFixedWindowLimiter("fixed", options =>
    {
        options.Window = TimeSpan.FromSeconds(30);
        options.PermitLimit = 5;
    });
});
//configure request pipeline

var app = builder.Build();
app.UseRateLimiter();

app.MapReverseProxy();


app.Run();
