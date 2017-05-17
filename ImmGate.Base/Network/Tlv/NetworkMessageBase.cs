namespace ImmGate.Base.Network.Tlv
{
    public abstract class NetworkMessageBase
    {
        public string TypeName;

        protected NetworkMessageBase()
        {
            TypeName = GetType().Name;
        }
    }
}