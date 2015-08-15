using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace eve_intel_server
{
    class Program
    {
        private static TcpListener _tcpListener;
        private static readonly List<TcpClient> TcpClientsList = new List<TcpClient>();

        static void Main() {
            _tcpListener = new TcpListener(IPAddress.Any, 1234);
            _tcpListener.Start();

            Console.WriteLine("Server started");

            while (true) {
                TcpClient tcpClient = _tcpListener.AcceptTcpClient();
                TcpClientsList.Add(tcpClient);

                Thread thread = new Thread(ClientListener);
                thread.Start(tcpClient);
            }
        }

        public static void ClientListener(object obj) {
            TcpClient tcpClient = (TcpClient)obj;
            StreamReader reader = new StreamReader(tcpClient.GetStream());

            Console.WriteLine("Client connected");

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
        }

        public static void BroadCast(string msg, TcpClient excludeClient) {
            foreach (TcpClient client in TcpClientsList) {
                if (client != excludeClient) {
                    StreamWriter sWriter = new StreamWriter(client.GetStream());
                    sWriter.WriteLine(msg);
                    sWriter.Flush();
                }
            }
        }
    }
}