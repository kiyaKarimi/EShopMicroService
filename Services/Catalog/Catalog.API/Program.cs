
using Catalog.API.Data;

namespace Catalog.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var Assembely = typeof(Program).Assembly;
            builder.Services.AddMediatR(config =>
            {
                config.RegisterServicesFromAssemblies(Assembely);
                config.AddOpenBehavior(typeof(ValidationBehavior<,>));
                config.AddOpenBehavior(typeof(LoggingBehavior<,>));
            });
            builder.Services.AddValidatorsFromAssembly(Assembely);
            builder.Services.AddCarter();

            builder.Services.AddMarten(opts =>
            {
                opts.Connection
            (builder.Configuration.GetConnectionString("Database")!);
            }
            ).UseLightweightSessions();
            if (builder.Environment.IsDevelopment())
                builder.Services.InitializeMartenWith<CatalogInitialData>();

            builder.Services.AddExceptionHandler<CustomExceptionHandler>();
            var app = builder.Build();

            app.MapCarter();
            app.UseExceptionHandler(options => { });
            app.MapGet("/", () => "Hello World!");

            app.Run();
        }
    }
}
