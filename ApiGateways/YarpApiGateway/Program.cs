using Microsoft.AspNetCore.RateLimiting;

namespace YarpApiGateway
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddReverseProxy()
                .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));
            builder.Services.AddRateLimiter(rateLimiterOptions =>
            {
                rateLimiterOptions.AddFixedWindowLimiter("fixed", options =>
                {
                    options.Window = TimeSpan.FromSeconds(10);
                    options.PermitLimit = 5;
                });
            });
            var app = builder.Build();
            app.UseRateLimiter();
            app.MapReverseProxy();
            app.MapGet("/", () => "Hello World!");

            app.Run();
        }
    }
}
