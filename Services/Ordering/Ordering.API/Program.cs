using Ordering.Application;
using Ordering.Infrastructure;

namespace Ordering.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddInfrastructureServices(builder.Configuration).
                AddApplicationServices().
                AddApiServices();
            var app = builder.Build();

            app.MapGet("/", () => "Hello World!");

            app.Run();
        }
    }
}
