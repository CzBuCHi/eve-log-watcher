using System;
using System.IO;
using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace eve_log_watcher.cva_kos_api
{
    public static class CvaClient
    {
        private const string cUrlFormat = "http://kos.cva-eve.org/api/?c=json&type={0}&q={1}";

        private static TResult GetInfo<TResult>(long eveId, string name) where TResult : CvaBaseInfo, new() {
            string type;
            if (typeof (TResult) == typeof (CvaCharacterInfo)) {
                type = "unit";
            } else if (typeof (TResult) == typeof (CvaCorporationInfo)) {
                type = "corp";
            } else if (typeof (TResult) == typeof (CvaAllianceInfo)) {
                type = "alliance";
            } else {
                throw new Exception("Invalid result type " + typeof (TResult).FullName);
            }

            HttpWebRequest request = (HttpWebRequest) WebRequest.Create(string.Format(cUrlFormat, type, Uri.EscapeUriString(name)));
            JObject jObject;
            using (WebResponse response = request.GetResponse()) {
                using (Stream stream = response.GetResponseStream()) {
                    using (StreamReader reader = new StreamReader(stream)) {
                        using (JsonTextReader jReader = new JsonTextReader(reader)) {
                            jObject = JObject.Load(jReader);
                        }
                    }
                }
            }

            JToken jMessage = jObject["message"];
            if (jMessage.Value<string>() != "OK") {
                throw new Exception("error message from kos.cva-eve.org: " + jMessage.Value<string>());
            }

            JToken jTotal = jObject["total"];
            if (jTotal.Value<int>() == 0) {
                return new TResult {
                    Id = -1,
                    EveId = eveId,
                    Type = type,
                    Kos = false
                };
            }

            JArray jResults = (JArray) jObject["results"];
            TResult[] results = jResults.ToObject<TResult[]>();
            return results[0];
        }

        public static CvaCharacterInfo GetCharacterInfo(long eveId,string name) {
            return GetInfo<CvaCharacterInfo>(eveId,name);
        }

        public static CvaCorporationInfo GetCorpInfo(long eveId, string name) {
            return GetInfo<CvaCorporationInfo>(eveId, name);
        }

        public static CvaAllianceInfo GetAllianceInfo(long eveId, string name) {
            return GetInfo<CvaAllianceInfo>(eveId, name);
        }
    }
}