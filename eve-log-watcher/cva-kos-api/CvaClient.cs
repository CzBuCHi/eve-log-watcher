using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Net;

namespace eve_log_watcher.cva_kos_api
{
    public static class CvaClient
    {
        private const string urlFormat = "http://kos.cva-eve.org/api/?c=json&type={0}&q={1}";

        private static TResult GetInfo<TResult>(string name) where TResult : CvaBaseInfo, new() {
            string type;
            if (typeof(TResult) == typeof(CvaCharacterInfo)) {
                type = "unit";
            } else if (typeof(TResult) == typeof(CvaCorporationInfo)) {
                type = "corp";
            } else if (typeof(TResult) == typeof(CvaAllianceInfo)) {
                type = "alliance";
            } else {
                throw new Exception("Invalid result type " + typeof(TResult).FullName);
            }

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(string.Format(urlFormat, type, Uri.EscapeUriString(name)));
            var response = request.GetResponse();
            var stream = response.GetResponseStream();
            var reader = new StreamReader(stream);
            var jReader = new JsonTextReader(reader);
            var jObject = JObject.Load(jReader);

            var jMessage = jObject["message"];
            if (jMessage.Value<string>() != "OK") {
                throw new Exception("error message from kos.cva-eve.org: " + jMessage.Value<string>());
            }

            var jTotal = jObject["total"];
            if (jTotal.Value<int>() == 0) {
                return new TResult() {
                    Kos = false
                };
            }

            JArray jResults = (JArray)jObject["results"];
            var results = jResults.ToObject<TResult[]>();
            return results[0];
        }

        public static CvaCharacterInfo GetCharacterInfo(string name) {
            return GetInfo<CvaCharacterInfo>(name);
        }

        public static CvaCorporationInfo GetCorpInfo(string name) {
            return GetInfo<CvaCorporationInfo>(name);
        }

        public static CvaAllianceInfo GetAllianceInfo(string name) {
            return GetInfo<CvaAllianceInfo>(name);
        }
    }
}
