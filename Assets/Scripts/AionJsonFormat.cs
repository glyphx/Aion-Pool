namespace AionJsonFormat
{
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;
    using System;
    using System.Collections.Generic;
    using System.Globalization;   

    public partial class Root
    {
        [JsonProperty("pendingShares")]
        public long PendingShares { get; set; }

        [JsonProperty("pendingBalance")]
        public double PendingBalance { get; set; }

        [JsonProperty("totalPaid")]
        public double TotalPaid { get; set; }

        [JsonProperty("lastPayment")]
        public DateTimeOffset LastPayment { get; set; }

        [JsonProperty("performance")]
        public Performance Performance { get; set; }

        [JsonProperty("performanceSamples")]
        public Performance[] PerformanceSamples { get; set; }
    }

    public partial class Performance
    {
        [JsonProperty("created")]
        public DateTimeOffset Created { get; set; }

        [JsonProperty("workers")]
        public Dictionary<string, Worker> Workers { get; set; }
    }

    public partial class Worker
    {
        [JsonProperty("hashrate")]
        public double Hashrate { get; set; }

        [JsonProperty("sharesPerSecond")]
        public double SharesPerSecond { get; set; }
    }

    public partial class Root
    {
        public static Root FromJson(string json) => JsonConvert.DeserializeObject<Root>(json, Converter.Settings);
    }

    public static class Serialize
    {
        public static string ToJson(this Root self) => JsonConvert.SerializeObject(self, Converter.Settings);
    }

    internal static class Converter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters = {
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
        };
    }
}