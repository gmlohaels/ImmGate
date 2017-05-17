using System;

namespace ImmGate.Base.Serialization
{
    public interface IDeserializer
    {
        object DeserializePacket(byte[] buffer, Type type);

    }


    public interface IDeserializer<out T>
    {
        T DeserializePacket(byte[] buffer, Type type);

    }
}