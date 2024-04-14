using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.DependencyInjection;
using DotNetEnv;

DotNetEnv.Env.Load();

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddAuthentication();
builder.Services.AddControllers();

builder.Services.AddCors(options =>
{
    options.AddPolicy(
        "MyCorsPolicy",
        builder =>
        {
            builder
                .WithOrigins("https://www.stocksinfo.net")
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials();
        }
        );
    });

builder.Services.Configure<KestrelServerOptions>(options =>    
{        
    options.Limits.RequestHeadersTimeout = System.TimeSpan.FromMinutes(1);
    options.Limits.KeepAliveTimeout = System.TimeSpan.FromMinutes(3);
});

WebApplication app = builder.Build();

app.UseCors("MyCorsPolicy"); // Use the CORS policy you defined earlier

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();   
