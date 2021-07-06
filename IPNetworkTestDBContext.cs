using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace IPNetworkEFCore
{
    public class IPNetworkTestDBContext : DbContext
    {
        public virtual DbSet<Network> Networks => Set<Network>();

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) => optionsBuilder.UseSqlServer(@"Server=(localdb)\MSSQLLocalDB;Database=IPNetworkTest;Trusted_Connection=True");

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }

    }
}
