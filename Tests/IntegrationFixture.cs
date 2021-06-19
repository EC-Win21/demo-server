using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using DbContext = DemoServer.Database.Context;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics;
using Microsoft.Data.Sqlite;

namespace Tests
{
    public class IntegrationFixture<TStartup>
       : WebApplicationFactory<TStartup> where TStartup : class
    {
        private SqliteConnection _keepAliveConnection;

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                RemoveService<DbContextOptions<DbContext>>(services);

                _keepAliveConnection = new SqliteConnection("DataSource=:memory:");
                _keepAliveConnection.Open();

                services.AddDbContext<DbContext>(options =>
                    options.UseSqlite(_keepAliveConnection));
            });
        }
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            _keepAliveConnection.Close();
        }

        private void RemoveService<TService>(IServiceCollection services)
        {
            var descriptor = services.SingleOrDefault(
                d => d.ServiceType ==
                    typeof(TService));

            var didRemove = services.Remove(descriptor);
            Debug.Assert(didRemove);
        }
    }
}
