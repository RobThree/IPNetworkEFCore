using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Net;

namespace IPNetworkEFCore
{
    public record Network
    {
        private static readonly byte[] _none = IPAddress.None.GetAddressBytes();

        public int Id { get; init; }

        [MaxLength(255)]
        public string? Description { get; init; }

        [NotMapped]
        public IPAddress Prefix
        {
            get => new(_prefixbytes);
            init { _prefixbytes = value.GetAddressBytes(); }
        }

        [NotMapped]
        public IPAddress Last => new(_last);

        public int PrefixLength { get; init; }


        [Required, MinLength(4), MaxLength(16)]
        private byte[] _prefixbytes { get; init; } = _none;

        [Required, MinLength(4), MaxLength(16)]
        private byte[] _last
        {
            get => CalcLast(_prefixbytes, PrefixLength);
            init { }
        }

        private static byte[] CalcLast(byte[] prefixBytes, int prefixLength)
        {
            var result = new byte[prefixBytes.Length];
            var mask = CreateMask(prefixBytes, prefixLength);
            for (var i = 0; i < prefixBytes.Length; i++)
                result[i] = (byte)(prefixBytes[i] | ~mask[i]);
            return result;
        }

        private static byte[] CreateMask(byte[] prefixBytes, int prefixLength)
        {
            var mask = new byte[prefixBytes.Length];
            var remainingBits = prefixLength;
            var i = 0;
            while (remainingBits >= 8)
            {
                mask[i] = 0xFF;
                i++;
                remainingBits -= 8;
            }
            if (remainingBits > 0)
            {
                mask[i] = (byte)(0xFF << (8 - remainingBits));
            }

            return mask;
        }
    }

}
