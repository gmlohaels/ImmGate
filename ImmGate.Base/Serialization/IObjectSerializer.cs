namespace ImmGate.Base.Serialization
{
    namespace ImmGate.Base.Serialization
    {
        public interface IObjectSerializer : IDeserializer, ISerializer
        {



        }


        public interface IObjectSerializer<T> : IDeserializer<T>, ISerializer<T>
        {

        }


    }

}
