using Nethereum.ABI.Decoders;
using Nethereum.ABI.Encoders;

namespace Channels.Contracts
{
    /// <summary>
    /// Extension methods for converting byte[] to string and back
    /// </summary>
    public static class ContractExtensions
    {
        private static Bytes32TypeEncoder _encoder;
        private static StringBytes32Decoder _decoder;

        static ContractExtensions()
        {
            _encoder = new Bytes32TypeEncoder();
            _decoder = new StringBytes32Decoder();

        }

        public static byte[] ConvertToBytes(this string s)
        {
            if (s == null) return null;
            return _encoder.Encode(s);
        }

        public static string ConvertToString(this byte[] b)
        {
            if (b == null) return null;
            return _decoder.Decode(b);
        }
    }
}
