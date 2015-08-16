using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using eve_intel_server.messages;

namespace eve_intel_server
{
    class Program
    {
        private static TcpListener _TcpListener;
        private static readonly List<TcpClient> _TcpClientsList = new List<TcpClient>();
        private static readonly object _TcpClientsListLock = new object();


        static void Main() {
            _TcpListener = new TcpListener(IPAddress.Any, 1234);
            _TcpListener.Start();

            Console.WriteLine("Server started");

            while (true) {
                TcpClient tcpClient = _TcpListener.AcceptTcpClient();
                lock (_TcpClientsListLock) {
                    _TcpClientsList.Add(tcpClient);
                }

                Thread thread = new Thread(ClientListener);
                thread.Start(tcpClient);
            }

            // TODO: server never exists ...
        }

        public static void ClientListener(object obj) {
            Console.WriteLine("Client connected");
            TcpClient tcpClient = (TcpClient)obj;

            int count;
            lock (_TcpClientsListLock) {
                count = _TcpClientsList.Count;
            }
            BroadCast(MessageInfo.Encode(count), null);

            StreamReader reader = new StreamReader(tcpClient.GetStream());
            
            while (tcpClient.Connected) {
                try {
                    string message = reader.ReadLine();
                    if (!string.IsNullOrEmpty(message)) {
                        BroadCast(message, tcpClient);
                        Console.WriteLine(message);
                    }
                } catch(Exception exc) {
                    Console.WriteLine(exc.Message);
                    break;
                }
            }

            lock (_TcpClientsListLock) {
                _TcpClientsList.Remove(tcpClient);
                count = _TcpClientsList.Count;
            }
            BroadCast(MessageInfo.Encode(count), null);
            Console.WriteLine("Client disconnected");
        }

        public static void BroadCast(string msg, TcpClient excludeClient) {
            lock (_TcpClientsListLock) {
                foreach (TcpClient client in _TcpClientsList) {
                    if (client != excludeClient) {
                        StreamWriter sWriter = new StreamWriter(client.GetStream());
                        sWriter.WriteLine(msg);
                        sWriter.Flush();
                    }
                }
            }
        }
    }
}