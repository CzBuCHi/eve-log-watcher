using System;
using System.IO;
using System.Net.Sockets;
using System.Threading;
using System.Windows.Forms;

namespace eve_log_watcher.server
{
    internal class EveIntelServerConnector
    {
        public delegate void MessageHandler(string data);

        private static TcpClient _TcpClient;
        private static StreamWriter _Writer;
        private static bool _Closing;

        public static bool Connect() {
            try {
                _TcpClient = new TcpClient("127.0.0.1", 1234);
                var thread = new Thread(Read);
                thread.Start(_TcpClient);
                _Writer = new StreamWriter(_TcpClient.GetStream());
                return _TcpClient.Connected;
            } catch (Exception e) {
                MessageBox.Show(e.Message, @"Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return false;
        }

        public static void Disconect() {
            _Closing = true;
        }

        public static void SendMessage(string data) {
            if (_Writer != null) {
                _Writer.WriteLine(data);
                _Writer.Flush();
            }
        }

        private static void Read(object obj) {
            var tcpClient = (TcpClient) obj;
            var sReader = new StreamReader(tcpClient.GetStream());

            while (!_Closing) {
                try {
                    var data = sReader.ReadLine();
                    OnMessage(data);
                } catch (Exception e) {
                    MessageBox.Show(e.Message, @"Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;
                }
            }

            _Writer.Dispose();
            _TcpClient.Close();
        }

        public static event MessageHandler Message;

        private static void OnMessage(string data) {
            Message?.Invoke(data);
        }
    }
}
