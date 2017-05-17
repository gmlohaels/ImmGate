using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ImmGate.Base.Exceptions;
using ImmGate.Base.Serialization.ImmGate.Base.Serialization;

namespace ImmGate.Base.Network.Tlv
{

    /// <summary>
    /// This Packet  Maintainer use TypeName field to determine type 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class DefaultNetworkPacketMaintainer<T> : INetworkPacketMaintainer<T> where T : NetworkMessageBase
    {
        private readonly IObjectSerializer serializer;
        private readonly IList<Assembly> assembliesToLookup;

        private readonly Dictionary<string, Type> cache = new Dictionary<string, Type>();


        public DefaultNetworkPacketMaintainer(IObjectSerializer serializer, IList<Assembly> assembliesToLookup)
        {
            this.serializer = serializer;
            this.assembliesToLookup = assembliesToLookup;
        }


        public Type GetDotNetTypeFrom(NetworkTlvPacket packet)
        {
            var basicMessage = (T)serializer.DeserializePacket(packet.Value, typeof(T));

            if (cache.ContainsKey(basicMessage.TypeName))
                return cache[basicMessage.TypeName];

            foreach (var assembly in assembliesToLookup)
            {
                var t = assembly.GetTypes().FirstOrDefault(z => z.Name == basicMessage.TypeName);
                if (t == null)
                    continue;

                cache.Add(basicMessage.TypeName, t);
                return t;
            }
            throw new TypeNotFoundException("Type not found: " + basicMessage.TypeName);
        }

        public T DeserializePacket(byte[] buffer, Type type)
        {
            return (T)serializer.DeserializePacket(buffer, type);
        }

        public byte[] SerializePacket(T packet)
        {
            return serializer.SerializePacket(packet);
        }
    }
}
