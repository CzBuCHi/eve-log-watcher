using Newtonsoft.Json;

namespace eve_log_watcher.cva_kos_api
{
    public abstract class CvaBaseInfo
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("label")]
        public string Label { get; set; }

        [JsonProperty("icon")]
        public string Icon { get; set; }

        [JsonProperty("kos")]
        public bool Kos { get; set; }

        [JsonProperty("eveid")]
        public long EveId { get; set; }
    }

    public sealed class CvaCharacterInfo : CvaBaseInfo
    {
        [JsonProperty("corp")]
        public CvaCorporationInfo Corp { get; set; }
    }

    public sealed class CvaCorporationInfo : CvaBaseInfo
    {
        [JsonProperty("ticker")]
        public string Ticker { get; set; }

        [JsonProperty("npc")]
        public bool Npc { get; set; }

        [JsonProperty("alliance")]
        public CvaAllianceInfo Alliance { get; set; }
    }

    public sealed class CvaAllianceInfo : CvaBaseInfo
    {
        [JsonProperty("ticker")]
        public string Ticker { get; set; }
    }
}
