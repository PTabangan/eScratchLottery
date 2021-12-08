using eScratchLottery.Server.Database;
using eScratchLottery.Server.WebApi;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using static eScratchLottery.Server.WebApi.DirectoryDelegates;

namespace eScratchLottery.Server
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "eScratchLottery API", Version = "v1" });
            });

            services.AddSignalR();
            services.AddDbContext<ScratchLotteryDbContext>(config =>
            {
                config.UseInMemoryDatabase(databaseName: "ScratchLottery");
            });

            services.AddTransient<CardService>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            var wwwRoot = app.ApplicationServices.GetService<GetWwwRootDirectory>()();
            PhysicalFileProvider fileProvider = new PhysicalFileProvider(wwwRoot);

            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = fileProvider
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "eScratchLottery API"));

            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<PlayerHub>("/hubs/player");
            });
        }
    }
}
