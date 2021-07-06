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
        public virtual DbSet<Address> Addresses => Set<Address>();

        public void ClearNetworks() => Database.ExecuteSqlRaw($"TRUNCATE TABLE [{nameof(Networks)}]");
        public void ClearAddresses() => Database.ExecuteSqlRaw($"TRUNCATE TABLE [{nameof(Addresses)}]");

        public IEnumerable<Network> FindNetworksContaining(IPAddress ip)
            => Networks
                .FromSqlRaw(@$"SELECT * 
                            FROM [{nameof(Networks)}]
                            WHERE 0x{ip.AsHex()} BETWEEN [{nameof(Network.Prefix)}] AND [{nameof(Network.Last)}]
                            ORDER BY [id]"
                ).AsEnumerable();

        public Address? FindAddress(IPAddress ip)
            => Addresses
                .FromSqlRaw(@$"SELECT * 
                            FROM [{nameof(Addresses)}]
                            WHERE [{nameof(Address.IP)}] = 0x{ip.AsHex()}"
                ).SingleOrDefault();

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) => optionsBuilder.UseSqlServer(@"Server=(localdb)\MSSQLLocalDB;Database=IPNetworkTest;Trusted_Connection=True");

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }

    }
}
