using DemoServer.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Net.Http.Headers;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Xunit;

namespace Tests
{
    public class DatabaseTest : IClassFixture<IntegrationFixture<DemoServer.Startup>>
    {
        private readonly IntegrationFixture<DemoServer.Startup> _fixture;

        public DatabaseTest(
            IntegrationFixture<DemoServer.Startup> fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task Data_From_Db_Retreivable()
        {
            using (var scope = _fixture.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<Context>();
                context.Database.EnsureCreated();

                var content = await context.TestTable.FirstOrDefaultAsync();

                Assert.NotNull(content);
                Assert.NotEmpty(content.Message);
            }
        }

        [Fact]
        public async Task Index_Returns_TestHTML()
        {
            var client = _fixture.CreateClient();

            var response = await client.GetAsync("/");

            var mime = response.Content.Headers.ContentType;
            var html = await response.Content.ReadAsStringAsync();

            Assert.Equal("<h1>Testdata i en databas!</h1>", html);
            Assert.Equal("text/html", mime.MediaType);
        }
    }
}
