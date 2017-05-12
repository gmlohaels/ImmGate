using System.Net.Sockets;
using ImmGate.Base.Serialization.ImmGate.Base.Serialization;

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

        protected AttributeOrientedTlvClient(Socket socket, IObjectSerializer serializer,
            IPacketTypeDeterminer typeDeterminer) : base(socket, serializer, typeDeterminer)
        {
        }

        protected AttributeOrientedTlvClient(IObjectSerializer serializer, IPacketTypeDeterminer typeDeterminer)
            : base(serializer, typeDeterminer)
        {
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
    }
}