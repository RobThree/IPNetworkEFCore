using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;

namespace IPNetworkEFCore
{
    public class IPNetworkTestDBContext : DbContext
    {
        public virtual DbSet<Network> Networks => Set<Network>();

        public void ClearNetworks() => Database.ExecuteSqlRaw($"TRUNCATE TABLE [{nameof(Networks)}]");

        public IEnumerable<Network> FindNetworksContaining(IPAddress ip)
            => Networks
                .FromSqlRaw(@$"SELECT * 
                            FROM [networks]
                            WHERE 0x{ip.AsHex()} BETWEEN [{nameof(Network.Prefix)}] AND [{nameof(Network.Last)}]
                            ORDER BY [id]"
                ).AsEnumerable();

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) => optionsBuilder.UseSqlServer(@"Server=(localdb)\MSSQLLocalDB;Database=IPNetworkTest;Trusted_Connection=True");

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }

    }
}
