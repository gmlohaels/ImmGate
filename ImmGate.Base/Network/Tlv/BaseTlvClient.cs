using System;
using System.IO;
using System.Net.Sockets;
using System.Runtime.Remoting.Messaging;
using ImmGate.Base.Events;
using ImmGate.Base.Extensions;
using ImmGate.Base.Network.Extensions;

namespace ImmGate.Base.Network.Tlv
{

    public abstract class BaseTlvClient
    {
        private int MaxPacketSize = 1024 * 1024 * 2;

        private TcpClient tcpClient;


        private byte[] ReadComplete(int bytesRequired)
        {

            var resultBuffer = new byte[bytesRequired];
            int currentPosition = 0;

            while (currentPosition != bytesRequired)
            {
                int sizeToRead = bytesRequired - currentPosition;

                int bytesReaded = 0;


                if (tcpClient != null)
                    bytesReaded = tcpClient.GetStream().Read(resultBuffer, currentPosition, sizeToRead);
                if (bytesReaded < 1)
                    throw new EndOfStreamException("Read Error");
                currentPosition += bytesReaded;
            }
            return resultBuffer;
        }

        delegate void WriteDelegate(byte[] buffer, int offset, int count);
        public event ImmGateEventHandler OnConnected;
        public event ImmGateEventHandler OnDisconnected;


        public bool IsOnline => tcpClient?.Client?.Connected != null && tcpClient.Client.Connected;

        protected IAsyncResult SendTlvPacketAsync(byte[] packet)
        {

            var tlvPacket = NetworkTlvPacket.TlvPacketFrom(packet);

            try
            {

                var networkStream = tcpClient.GetStream();

                WriteDelegate write = networkStream.Write;
                //we DONT use BeginWrite because of buggy mono that sometimes does not Derive this message
                var result = write.BeginInvoke(tlvPacket, 0, tlvPacket.Length, WriteCallback, null);
                return result;
            }
            catch (Exception)
            {
                Close();
            }

            return null;

        }

        private void WriteCallback(IAsyncResult ar)
        {
            try
            {
                AsyncResult result = (AsyncResult)ar;
                WriteDelegate caller = (WriteDelegate)result.AsyncDelegate;
                caller.EndInvoke(ar);
            }
            catch (Exception)
            {
                Close();
            }


        }


        protected void Connect(string hostName, int port)
        {


            try
            {
                tcpClient = GetNewTcpSocket(SocketSettings.DefaultHigh);
                tcpClient.Connect(hostName, port);
            }
            catch (Exception)
            {
                DoOnDisconnected();
                throw;
            }


            DoOnConnected();
        }

        private TcpClient GetNewTcpSocket(SocketSettings settings)
        {
            var result = new TcpClient();
            result.Client.SetupSocketTimeouts(settings);
            return result;
        }


        public void Close()
        {
            if (tcpClient == null)
                return;


            if (IsOnline)
                tcpClient.Close();
            tcpClient = null;
            DoOnDisconnected();
            
        }

        protected void SendTlvPacket(byte[] packet)
        {
            try
            {
                var tlvPacket = NetworkTlvPacket.TlvPacketFrom(packet);

                //    Console.WriteLine("PACKET SIZE: " + packet.Length);

                tcpClient?.GetStream().Write(tlvPacket, 0, tlvPacket.Length);
            }
            catch (Exception)
            {

                Close();
                throw;
            }
        }


        public void ProcessReceive()
        {


            var headerBytes = ReadComplete(NetworkTypeLengthHeader.Size);
            var header = headerBytes.ToStructure<NetworkTypeLengthHeader>();




            if (header.MessageSize > 0 && header.MessageSize <= MaxPacketSize && header.Marker == NetworkTypeLengthHeader.ValidHeaderMarker)
            {

                var packet = new NetworkTlvPacket
                {
                    Header = header,
                    Value = ReadComplete(header.MessageSize)
                };
                OnTlvPacketReceived(packet);
            }
            else
            {
                var exception = new Exception("Invalid Packet Received");
                Close();
                throw exception;
            }

        }


        protected BaseTlvClient(Socket socket, SocketSettings settings)
        {
            tcpClient = new TcpClient { Client = socket };
            socket.SetupSocketTimeouts(settings);
        }

        protected BaseTlvClient(Socket socket) : this(socket, SocketSettings.DefaultHigh)
        {

        }

        protected BaseTlvClient()
        {

        }

        protected abstract void OnTlvPacketReceived(NetworkTlvPacket packet);


        public void DoOnConnected()
        {
            OnConnected?.Invoke(this);
        }

        protected virtual void DoOnDisconnected()
        {
            OnDisconnected?.Invoke(this);
        }


    }
}