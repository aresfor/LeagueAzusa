namespace LeagueAzusa
{
    using System;
    using System.Collections.Generic;

    using System.Globalization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    public partial class EventMsg
{
        [JsonProperty("EventID")]
        public int EventId { get; set; }

        [JsonProperty("EventName")]
        public string EventName { get; set; }

        [JsonProperty("EventTime")]
        public long EventTime { get; set; }

        [JsonProperty("KillerName", NullValueHandling = NullValueHandling.Ignore)]
        public string? KillerName { get; set; }

        [JsonProperty("TurretKilled", NullValueHandling = NullValueHandling.Ignore)]
        public string TurretKilled { get; set; }

        [JsonProperty("Assisters", NullValueHandling = NullValueHandling.Ignore)]
        public List<string> Assisters { get; set; }

        [JsonProperty("InhibKilled", NullValueHandling = NullValueHandling.Ignore)]
        public string InhibKilled { get; set; }

        [JsonProperty("DragonType", NullValueHandling = NullValueHandling.Ignore)]
        public string DragonType { get; set; }

        [JsonProperty("Stolen", NullValueHandling = NullValueHandling.Ignore)]
        public string Stolen { get; set; }

        [JsonProperty("VictimName", NullValueHandling = NullValueHandling.Ignore)]
        public string VictimName { get; set; }

        [JsonProperty("KillStreak", NullValueHandling = NullValueHandling.Ignore)]
        public int? KillStreak { get; set; }

        [JsonProperty("Acer", NullValueHandling = NullValueHandling.Ignore)]
        public string? Acer { get; set; }

        [JsonProperty("AcingTeam", NullValueHandling = NullValueHandling.Ignore)]
        public string AcingTeam { get; set; }
    }

    internal static class Converter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters =
            {
                //AcerConverter.Singleton,
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
        };
    }

    //internal class AcerConverter : JsonConverter
    //{
    //    public override bool CanConvert(Type t) => t == typeof(Acer) || t == typeof(Acer?);

    //    public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
    //    {
    //        if (reader.TokenType == JsonToken.Null) return null;
    //        var value = serializer.Deserialize<string>(reader);
    //        if (value == "Riot Tuxedo")
    //        {
    //            return Acer.RiotTuxedo;
    //        }
    //        throw new Exception("Cannot unmarshal type Acer");
    //    }

    //    public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
    //    {
    //        if (untypedValue == null)
    //        {
    //            serializer.Serialize(writer, null);
    //            return;
    //        }
    //        var value = (Acer)untypedValue;
    //        if (value == Acer.RiotTuxedo)
    //        {
    //            serializer.Serialize(writer, "Riot Tuxedo");
    //            return;
    //        }
    //        throw new Exception("Cannot marshal type Acer");
    //    }

    //    public static readonly AcerConverter Singleton = new AcerConverter();
    //}
}
