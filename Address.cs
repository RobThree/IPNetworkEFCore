using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Net;

namespace IPNetworkEFCore
{
    public record Address
    {
        private static readonly byte[] _none = IPAddress.None.GetAddressBytes();

        public int Id { get; init; }

        [MaxLength(255)]
        public string? Description { get; init; }

        [NotMapped]
        public IPAddress IP
        {
            get => new(_ipbytes);
            init { _ipbytes = value.GetAddressBytes(); }
        }

        [Required, MinLength(4), MaxLength(16)]
        private byte[] _ipbytes { get; init; } = _none;
    }

    internal class AddressConfig : IEntityTypeConfiguration<Address>
    {
        public void Configure(EntityTypeBuilder<Address> entity)
        {
            entity.Property("_ipbytes")
                .HasColumnName(nameof(Address.IP));
            entity.HasIndex(new[] { "_ipbytes" }).IsUnique();
        }
    }
}
