using Ordering.Application;
using Ordering.Infrastructure;
using Ordering.Infrastructure.Data.Extensions;

namespace Ordering.API
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddInfrastructureServices(builder.Configuration).
                AddApplicationServices(builder.Configuration).
                AddApiServices(builder.Configuration);
            var app = builder.Build();
            app.UseApiServices();
            if (app.Environment.IsDevelopment())
            {
                await app.InitialiseDatabaseAsync();
            }
            app.MapGet("/", () => "Hello World!");

            app.Run();
        }
    }
}
