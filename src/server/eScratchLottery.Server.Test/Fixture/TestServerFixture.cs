using System;
using System.Net.Http;
using System.Threading.Tasks;
using eScratchLottery.Server.Database;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using static eScratchLottery.Server.WebApi.DirectoryDelegates;

namespace eScratchLottery.Server.Test.Fixture
{
    [CollectionDefinition(nameof(ScratchLotteryApiTestCollection))]
    public class ScratchLotteryApiTestCollection : ICollectionFixture<TestServerFixture> { }

    public class TestServerFixture : IAsyncLifetime
    {
        private TestWebApiServer _webApiServer;

        public Task InitializeAsync()
        {
            return Task.Run(() =>
            {
                _webApiServer = new TestWebApiServer();

                using (var scope = _webApiServer.Server.Services.CreateScope())
                {
                    var dbContext = scope.ServiceProvider.GetService<ScratchLotteryDbContext>();

                    DataSeeder.SeedTest(dbContext);
                }
            });
        }

        public HttpClient CreateHttpClient()
        {
            return _webApiServer.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false,
            });
        }

        public Task DisposeAsync()
        {
            return Task.Run(() =>
            {
                _webApiServer.Dispose();
            });
        }

    }

    public class TestWebApiServer : WebApplicationFactory<Startup> 
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                services.AddSingleton<GetWwwRootDirectory>(service => () => AppDomain.CurrentDomain.BaseDirectory);
            });
        }
    }

}
