using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

#if _EVE_INTEL_SERVER
namespace eve_intel_server.messages
#elif _EVE_LOG_WATCHER
namespace eve_log_watcher.eve_intel_server.messages
#else
#error Unknown project :(
#endif
{
    public abstract class MessageBase
    {
        #region json encode / decode

        private static readonly Dictionary<int, Type> _Messages = new Dictionary<int, Type> {
            {MessageInfo.MessageId, typeof (MessageInfo)},
            {MessageKos.MessageId, typeof (MessageKos)},
            // other message types goes here ...
        };

        public string Encode() {
            return JObject.FromObject(this).ToString(Formatting.None);
        }

        public static MessageBase Decode(string message) {
            if (string.IsNullOrEmpty(message)) {
                return null;
            }

            try {
                var jMessage = JObject.Parse(message);
                var jMessageId = jMessage["MessageId"];
                if (jMessageId == null) {
                    return null;
                }
                var messageId = jMessageId.Value<int>();

                Type messageType;
                if (!_Messages.TryGetValue(messageId, out messageType)) {
                    return null;
                }

                return (MessageBase)jMessage.ToObject(messageType);
            } catch {
                return null;
            }
        } 

        #endregion

        public abstract int Id { get; }
    }

    public class MessageInfo : MessageBase
    {
        public const int MessageId = 0;
        public override int Id => MessageId;

        public int Clients { get; set; }

        public static string Encode(int clients) {
            MessageInfo message = new MessageInfo {
                Clients = clients
            };
            return message.Encode();
        }
    }

    public class MessageKos : MessageBase
    {
        public const int MessageId = 1;
        public override int Id => MessageId;

        public long SystemId { get; set; }
        public string[] KosPlayers { get; set; }

        public static string Encode(int systemId, string[] kosPlayers) {
            MessageKos message = new MessageKos {
                SystemId = systemId,
                KosPlayers = kosPlayers
            };
            return message.Encode();
        }
    }
}