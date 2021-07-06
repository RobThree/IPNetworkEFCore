using System.ComponentModel.DataAnnotations;
using System.Net;

namespace IPNetworkEFCore
{
    public record Network
    {
        public int Id { get; init; }

        [MaxLength(255)]
        public string? Description { get; init; }

        public IPAddress Prefix { get; init; }

        public int PrefixLength { get; init; }
    }
}
