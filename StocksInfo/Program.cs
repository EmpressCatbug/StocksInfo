using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.DependencyInjection;
using StocksInfo.Services;
using System.Net.Http;

DotNetEnv.Env.Load();
string apikey =  System.Environment.GetEnvironmentVariable("ALPHA_VANTAGE_KEY");
string avBaseUrl = System.Environment.GetEnvironmentVariable("ALPHA_VANTAGE_BASE_URL");

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddAuthentication();
builder.Services.AddControllers();
builder.Services.AddHttpClient();
builder.Services.AddScoped<StockDataService>(provider => new StockDataService(provider.GetRequiredService<HttpClient>(), apikey, avBaseUrl));

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
