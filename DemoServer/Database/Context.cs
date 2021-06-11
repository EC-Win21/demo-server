using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace DemoServer.Database
{
    public class Context : DbContext
    {
        public Context([NotNull] DbContextOptions options) : base(options)
        {
        }

        public DbSet<TestModel> TestTable { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<TestModel>().HasData(
                new TestModel { 
                    Message = "<h1>Testdata i en databas!</h1>" 
                });
        }
    }

    public class TestModel
    {
        [Key]
        public string Message { get; set; }
    }
}
