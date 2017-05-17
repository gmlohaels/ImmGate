using System;
using ImmGate.Base.Network.Tlv;

namespace ImmGate.Base.Network
{
    public interface IPacketTypeDeterminer
    {
        Type GetDotNetTypeFrom(NetworkTlvPacket packet);
    }




}
