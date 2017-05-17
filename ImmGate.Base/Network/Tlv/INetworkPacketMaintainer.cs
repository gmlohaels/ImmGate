using ImmGate.Base.Serialization.ImmGate.Base.Serialization;

namespace ImmGate.Base.Network.Tlv
{
    public interface INetworkPacketMaintainer<T> : IObjectSerializer<T>, IPacketTypeDeterminer
    {
    }
}
