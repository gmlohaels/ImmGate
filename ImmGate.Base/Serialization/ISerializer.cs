namespace ImmGate.Base.Serialization
{
    public interface ISerializer
    {
        byte[] SerializePacket(object packet);

    }

    public interface ISerializer<in T>
    {
        byte[] SerializePacket(T packet);

    }
}