using Company.Default.Infra.Configurations;
using Microsoft.EntityFrameworkCore;

namespace Company.Default.Infra.Contexts
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            //Set conventions for data types
            configurationBuilder
                .Properties<string>()
                .AreUnicode(false)
                .HaveMaxLength(1024);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Apply configurations from entities
            modelBuilder.ApplyConfiguration(new PersonConfiguration());
        }
    }
}
