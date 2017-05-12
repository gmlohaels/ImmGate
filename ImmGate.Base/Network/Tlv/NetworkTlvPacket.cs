using System;

namespace ImmGate.Base.Network.Tlv
{
    public class NetworkTlvPacket
    {
        public NetworkTypeLengthHeader Header;
        public byte[] Value;
        public static byte[] TlvPacketFrom(byte[] value)
        {
            var header = NetworkTypeLengthHeader.GenerateHeaderBytesFrom(value);
            var result = new byte[header.Length + value.Length];
            Array.Copy(header, result, header.Length);
            Array.Copy(value, 0, result, header.Length, value.Length);
            return result;
        }


    }
}