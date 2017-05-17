using System;
using System.Net.Sockets;
using System.Reflection;
using ImmGate.Base.Serialization.ImmGate.Base.Serialization;

namespace ImmGate.Base.Network.Tlv
{
    public class MessageOrientedTlvClient<T> : BaseTlvClient where T : class
    {

        protected readonly IObjectSerializer<T> Serializer;
        protected readonly IPacketTypeDeterminer TypeDeterminer;

        public string ReflectionPrefix = "OnTlv";


        public MessageOrientedTlvClient(INetworkPacketMaintainer<T> packetMaintainer)
        {
            Serializer = packetMaintainer;
            TypeDeterminer = packetMaintainer;
        }


        public MessageOrientedTlvClient(Socket sock, INetworkPacketMaintainer<T> packetMaintainer) : base(sock)
        {
            Serializer = packetMaintainer;
            TypeDeterminer = packetMaintainer;
        }


        public virtual IAsyncResult SendMessageAsync(T message)
        {
            var serializedPacket = Serializer.SerializePacket(message);
            return SendTlvPacketAsync(serializedPacket);
        }

        public virtual void SendMessage(T message)
        {
            var serializedPacket = Serializer.SerializePacket(message);
            SendTlvPacket(serializedPacket);
        }




        protected override void OnTlvPacketReceived(NetworkTlvPacket packet)
        {

            var t = TypeDeterminer.GetDotNetTypeFrom(packet);
            var p = Serializer.DeserializePacket(packet.Value, t) as T;


            if (p == null || t == null)
                throw new ArgumentOutOfRangeException(nameof(packet));

            OnMessageReceived(p);

        }

        protected virtual void CallTlvMethod(MethodInfo methodInfo, T p)
        {
            methodInfo.Invoke(this, new[] { p });
        }

        protected virtual void OnMessageReceived(T message)
        {
            var methodInfo = GetType().GetMethod(ReflectionPrefix + message.GetType().Name);
            if (methodInfo != null)
            {
                CallTlvMethod(methodInfo, message);
            }
        }
    }


}
