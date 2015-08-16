using System;
using System.IO;
using System.Net.Sockets;
using System.Threading;
using System.Windows.Forms;
using eve_log_watcher.eve_intel_server.messages;

namespace eve_log_watcher.eve_intel_server
{
    internal class EveIntelServerConnector : IDisposable
    {
        public delegate void MessageHandler(MessageBase message);

        private static EveIntelServerConnector _Instance;
        private readonly MessageHandler _Handler;
        private readonly TcpClient _TcpClient;
        private readonly Thread _Thread;
        private readonly StreamWriter _Writer;

        public EveIntelServerConnector(string ipAddress, int port, MessageHandler handler) {
            _Handler = handler;

            _TcpClient = new TcpClient(ipAddress, port);
            _Thread = new Thread(Read);
            _Thread.Start();
            _Writer = new StreamWriter(_TcpClient.GetStream());
        }

        public void Dispose() {
            _Thread?.Abort();
        }

        public static void Connect(string ipAddress, int port, MessageHandler handler) {
            try {
                _Instance = new EveIntelServerConnector(ipAddress, port, handler);
            } catch (Exception exc) {
                MessageBox.Show(exc.Message, @"Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public static void Disconnect() {
            _Instance?.Dispose();
        }

        public static void Send(MessageBase message) {
            if (_Instance?._Writer != null) {
                string encoded = message.Encode();
                _Instance._Writer.WriteLine(encoded);
                _Instance._Writer.Flush();
            }
        }

        private void Read() {
            StreamReader reader = null;
            try {
                reader = new StreamReader(_TcpClient.GetStream());
                while (true) {
                    try {
                        string data = reader.ReadLine();
                        if (!string.IsNullOrEmpty(data)) {
                            OnMessage(data);
                        }
                    } catch (Exception e) {
                        MessageBox.Show(e.Message, @"Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        break;
                    }
                }
            } catch (ThreadAbortException) {
            } finally {
                reader?.Dispose();
                _Writer.Dispose();
                _TcpClient.Close();
            }
        }

        private void OnMessage(string message) {
            MessageBase decoded = MessageBase.Decode(message);
            if (decoded != null) {
                _Handler(decoded);
            }
        }
    }
}
