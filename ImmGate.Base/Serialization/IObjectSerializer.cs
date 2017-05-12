using System;

namespace ImmGate.Base.Serialization
{
    namespace ImmGate.Base.Serialization
    {
        public interface IObjectSerializer
        {
            object DeserializePacket(byte[] buffer, Type type);
            byte[] SerializePacket<T>(T packet);

        }
    }

}
