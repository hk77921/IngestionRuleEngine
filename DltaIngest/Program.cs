using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
//using Serilog;
using System.IO;
using DltaIngest.Repository;
using DltaIngest.Models;
using DltaIngest.AppService;

namespace DltaIngest
{
    public class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("** Init the process** ");

            Log.Logger = new LoggerConfiguration()
            .WriteTo.Console()
            .CreateLogger();


            var services = ConfigureServices();
            Log.Information("configure service");


            var serviceProvider = services.BuildServiceProvider();
            Log.Information("Build service provider");

             serviceProvider.GetRequiredService<DltaIngestionRuleDriver>().Run();
            Console.ReadKey();
        }
        public static IConfiguration LoadConfiguration()
        {
            var builder = new ConfigurationBuilder()

            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .AddJsonFile($"appsetting{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "production"}.json", optional: true)
            .AddEnvironmentVariables();

            return builder.Build();
        }

        private static IServiceCollection ConfigureServices()
        {
            IServiceCollection services = new ServiceCollection();

            var config = LoadConfiguration();

            services.AddSingleton(config);
            services.AddSingleton<SqlRepository>();
            services.AddSingleton<MongoDBInit>();
            services.AddScoped<IDatabaseSettings, DatabaseSettings>();

            services.AddTransient<DltaIngestionRuleDriver>();
            //services.AddScoped<IAzureAssets, AzureAssets>();
            //services.AddScoped<ICollectAzureAssests, CollectAzureAssests>();

            return services;
        }
    }
}
