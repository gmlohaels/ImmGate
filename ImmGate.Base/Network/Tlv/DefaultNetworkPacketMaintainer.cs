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
    public class DefaultNetworkPacketMaintainer : INetworkPacketMaintainer<NetworkMessageBase>
    {
        private readonly IObjectSerializer<NetworkMessageBase> serializer;
        private readonly IList<Assembly> assembliesToLookup;

        private readonly Dictionary<string, Type> cache = new Dictionary<string, Type>();

        public DefaultNetworkPacketMaintainer(IObjectSerializer<NetworkMessageBase> serializer, Assembly assemblyWithNetworkMessages)
        {
            this.serializer = serializer;
            assembliesToLookup = new List<Assembly> { assemblyWithNetworkMessages };

        }


        public DefaultNetworkPacketMaintainer(IObjectSerializer<NetworkMessageBase> serializer,
            Type basicTypeForAssembly)
        {
            this.serializer = serializer;
            assembliesToLookup = new List<Assembly>() { Assembly.GetAssembly(basicTypeForAssembly) };

        }


        public DefaultNetworkPacketMaintainer(IObjectSerializer<NetworkMessageBase> serializer, IList<Assembly> assembliesToLookup)
        {
            this.serializer = serializer;
            this.assembliesToLookup = assembliesToLookup;
        }


        public Type GetDotNetTypeFrom(NetworkTlvPacket packet)
        {
            var basicMessage = serializer.DeserializePacket(packet.Value, typeof(NetworkMessageBase));

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

        public NetworkMessageBase DeserializePacket(byte[] buffer, Type type)
        {
            return serializer.DeserializePacket(buffer, type);
        }

        public byte[] SerializePacket(NetworkMessageBase packet)
        {
            return serializer.SerializePacket(packet);
        }
    }
}
