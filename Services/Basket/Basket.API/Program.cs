
using BuildingBlocks.Exceptions.Handler;

namespace Basket.API
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

            builder.Services.AddMarten(opts =>
            {
                opts.Connection
            (builder.Configuration.GetConnectionString("Database")!);
                opts.Schema.For<ShoppingCart>().Identity(x => x.UserName);
            }
            ).UseLightweightSessions();
            builder.Services.AddScoped<IBasketRepository, BasketRepository>();
            builder.Services.Decorate<IBasketRepository, BasketRepository>();
            builder.Services.AddValidatorsFromAssembly(Assembely);
            builder.Services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = builder.Configuration.GetConnectionString("Redis");
            });

            builder.Services.AddCarter();
            builder.Services.AddExceptionHandler<CustomExceptionHandler>();
            var app = builder.Build();

            app.MapCarter();
            app.UseExceptionHandler(options => { });
            app.Run();
        }
    }
}
