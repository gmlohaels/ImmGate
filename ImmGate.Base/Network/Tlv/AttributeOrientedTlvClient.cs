using System.Net.Sockets;

namespace ImmGate.Base.Network.Tlv
{
    public abstract class AttributeOrientedTlvClient<T> : MessageOrientedTlvClient<T> where T : class, new()
    {


        protected bool IsAuthorized;
        protected bool RequireAuthForNonMarkedMethods = true;

        protected void SetAuthorized(bool authorized)
        {
            IsAuthorized = authorized;
        }



        protected virtual void OnNotAuthorized()
        {


        }



        protected override void OnMessageReceived(T message)
        {
            var methodInfo = GetType().GetMethod(ReflectionPrefix + message.GetType().Name);
            if (methodInfo != null)
            {

                object[] attrs;
                bool allowCall = IsAuthorized;
                if (RequireAuthForNonMarkedMethods)
                {

                    if (!IsAuthorized)
                    {
                        attrs = methodInfo.GetCustomAttributes(typeof(TlvAuthenticationRoutineAttribute), true);
                        if (attrs.Length > 0)
                            allowCall = true;
                    }
                }
                else
                {
                    attrs = methodInfo.GetCustomAttributes(typeof(TlvAuthRequiredAttribute), true);
                    if (attrs.Length == 0 || IsAuthorized)
                        allowCall = true;
                }

                if (allowCall)
                {
                    base.OnMessageReceived(message);
                }
                else
                {
                    OnNotAuthorized();
                }
            }



        }

        protected AttributeOrientedTlvClient(INetworkPacketMaintainer<T> packetMaintainer) : base(packetMaintainer)
        {
        }

        protected AttributeOrientedTlvClient(Socket sock, INetworkPacketMaintainer<T> packetMaintainer) : base(sock, packetMaintainer)
        {
        }
    }
}