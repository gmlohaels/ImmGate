﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Reflection;
using System.Threading.Tasks;
using ImmGate.Base.Network.Tlv;

namespace ImmGate.Network
{

    public class AsyncTlvServer<T> where T : BaseTlvClient
    {

        private readonly List<T> clientList = new List<T>();



        public List<T> ConnectedClients
        {
            get
            {

                lock (clientList)
                {
                    return clientList.ToList();
                }
            }
        }



        public event EventHandler<Exception> OnException;
        public event EventHandler<T> OnClientConnected;
        public event EventHandler<T> OnClientDisconnected;



        private void DoOnClientConnected(T e)
        {
            OnClientConnected?.Invoke(this, e);
        }

        protected Func<AsyncTlvServer<T>, Socket, T> AcceptClient;




        private void DisconnectClient(T client)
        {


            client.Close();


            lock (clientList)
            {
                clientList.Remove(client);
            }
            DoOnClientDisconnected(client);
        }

        protected void ProcessClient(T client)
        {
            lock (clientList)
            {
                clientList.Add(client);
            }
            DoOnClientConnected(client);
            HandleConnection(client);
        }



        private readonly TcpListener listener;


        protected AsyncTlvServer(int port)
        {
            listener = TcpListener.Create(port);
        }

        public AsyncTlvServer(int port, Func<AsyncTlvServer<T>, Socket, T> acceptClient) : this(port)
        {
            AcceptClient = acceptClient;
        }

        private void HandleConnection(T context)
        {



            Task.Factory.StartNew(() =>
            {
                try
                {
                    context.DoOnConnected();
                    while (!stopped)
                    {
                        context.ProcessReceive();
                    }
                }

                catch (EndOfStreamException)
                {
                    //this is Socket Error, Just Disconnect in finally block 
                }
                catch (IOException)
                {

                    //this is Socket Error, Just Disconnect in finally block
                }

                catch (SocketException)
                {
                    //this is Socket Error, Just Disconnect in finally block
                }


                catch (TargetInvocationException te)
                {
                    DoOnException(te.InnerException ?? te);
                }

                catch (Exception e)
                {
                    DoOnException(e);
                }
                finally
                {
                    DisconnectClient(context);
                }
            }, TaskCreationOptions.LongRunning);
        }


        private async void AcceptLoop()
        {
            while (!stopped)
            {
                try
                {

                    var s = await listener.AcceptTcpClientAsync();

                    var client = AcceptClient(this, s.Client);

                    ProcessClient(client);

                }
                catch (Exception e)
                {
                    DoOnException(e);
                }
            }
        }




        private bool stopped;


        public void StartListen()
        {
            listener.Start();
            AcceptLoop();
        }

        public void Shutdown()
        {
            stopped = true;
            listener.Stop();
        }


        private void DoOnClientDisconnected(T e)
        {
            OnClientDisconnected?.Invoke(this, e);

        }

        private void DoOnException(Exception e)
        {
            OnException?.Invoke(this, e);
        }
    }
}
