namespace ImmGate.Base.Network.Tlv
{
    public class NetworkMessageBase
    {
        public string TypeName;

        protected NetworkMessageBase()
        {
            TypeName = GetType().Name;
        }
    }
}