using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using eScratchLottery.Server.Database;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using static eScratchLottery.Server.WebApi.DirectoryDelegates;

namespace eScratchLottery.Server
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var configuration = BuildConfiguration(PathToContentRoot);
            var appSettings = configuration.Get<AppSettings>();

            var webHost = CreateHostBuilder(args)
                .ConfigureWebHost(builder => 
                {
                    builder.UseUrls(appSettings.BindUrl)
                    .ConfigureServices(services =>
                    {
                        services.AddSingleton<GetWwwRootDirectory>(service => () => GetWwwRootPath(appSettings));
                    });
                })
                .Build();

            using (var scope = webHost.Services.CreateScope())
            {
                Console.WriteLine("Initializing database");
                InitializeData(scope.ServiceProvider);
            }

            webHost.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });


        public static IConfigurationRoot BuildConfiguration(string basePath)
        {
            return new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddInMemoryCollection(new Dictionary<string, string>{
                    { "BindUrl", $"http://*:2020"}
                })
                .AddEnvironmentVariables("eSL_")
                .Build();
        }

        private static string PathToContentRoot => Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

        private static string GetWwwRootPath(AppSettings settings)
        {
            return Path.GetFullPath(settings.WwwRoot);
        }

        private static void InitializeData(IServiceProvider services) 
        {
            var dbContext = services.GetService<ScratchLotteryDbContext>();
            DataSeeder.SeedDemo(dbContext);
        }
    }
}
