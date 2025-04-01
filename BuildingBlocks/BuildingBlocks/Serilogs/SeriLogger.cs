using Microsoft.Extensions.Hosting;
using Serilog.Sinks.Elasticsearch;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace BuildingBlocks.Serilogs
{
    public static class SeriLogger
    {
        public static Action<HostBuilderContext, LoggerConfiguration> Configure =>
           (context, configuration) =>
           {
               // آدرس Elasticsearch از تنظیمات خوانده می‌شود
               var elasticUri = context.Configuration.GetValue<string>("ElasticConfiguration:Uri") ?? "http://localhost:9200";

               configuration
                    .Enrich.FromLogContext()
                    .Enrich.WithMachineName()
                    .Enrich.WithEnvironmentUserName()  // افزودن نام کاربری محیط
                    .Enrich.WithProperty("Environment", context.HostingEnvironment.EnvironmentName)
                    .Enrich.WithProperty("Application", context.HostingEnvironment.ApplicationName)
                    .WriteTo.Debug()
                    .WriteTo.Console()
                    .WriteTo.File("logs/log-.txt", rollingInterval: RollingInterval.Day)
                    .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri(elasticUri))
                    {
                        IndexFormat = $"applogs-{context.HostingEnvironment.ApplicationName?.ToLower().Replace(".", "-")}-{context.HostingEnvironment.EnvironmentName?.ToLower().Replace(".", "-")}-{DateTime.UtcNow:yyyy-MM}",
                        AutoRegisterTemplate = true,
                        NumberOfShards = 2,
                        NumberOfReplicas = 1,
                        // در صورت نیاز تنظیمات اضافی مانند InlineFields را هم می‌توان اضافه کرد
                    })
                    .Enrich.WithProperty("Environment", context.HostingEnvironment.EnvironmentName)
                    .Enrich.WithProperty("Application", context.HostingEnvironment.ApplicationName)
                    .ReadFrom.Configuration(context.Configuration);
           };
    }
}
