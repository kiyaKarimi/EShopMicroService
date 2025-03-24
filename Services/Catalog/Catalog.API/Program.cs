namespace Catalog.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddCarter();
            builder.Services.AddMediatR(config =>
            {
                config.RegisterServicesFromAssemblies(typeof(Program).Assembly);
            });
            builder.Services.AddMarten(opts =>
            {
                opts.Connection
            (builder.Configuration.GetConnectionString("Database")!);
            }
            ).UseLightweightSessions();
            var app = builder.Build();

            app.MapCarter();
            app.MapGet("/", () => "Hello World!");

            app.Run();
        }
    }
}
