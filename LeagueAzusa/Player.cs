using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeagueAzusa
{
    using System;
    using System.Collections.Generic;

    using System.Globalization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    public partial class Player
    {
        [JsonProperty("championName")]
        public string ChampionName { get; set; }

        [JsonProperty("isBot")]
        public bool IsBot { get; set; }

        [JsonProperty("isDead")]
        public bool IsDead { get; set; }

        [JsonProperty("items")]
        public List<Item> Items { get; set; }

        [JsonProperty("level")]
        public long Level { get; set; }

        [JsonProperty("position")]
        public string Position { get; set; }

        [JsonProperty("rawChampionName")]
        public string RawChampionName { get; set; }

        [JsonProperty("respawnTimer")]
        public double RespawnTimer { get; set; }

        [JsonProperty("runes")]
        public Runes Runes { get; set; }

        [JsonProperty("scores")]
        public Scores Scores { get; set; }

        [JsonProperty("skinID")]
        public long SkinId { get; set; }

        [JsonProperty("summonerName")]
        public string SummonerName { get; set; }

        [JsonProperty("summonerSpells")]
        public SummonerSpells SummonerSpells { get; set; }

        [JsonProperty("team")]
        public string Team { get; set; }
    }

    public partial class Item
    {
        [JsonProperty("canUse")]
        public bool CanUse { get; set; }

        [JsonProperty("consumable")]
        public bool Consumable { get; set; }

        [JsonProperty("count")]
        public long Count { get; set; }

        [JsonProperty("displayName")]
        public string DisplayName { get; set; }

        [JsonProperty("itemID")]
        public long ItemId { get; set; }

        [JsonProperty("price")]
        public long Price { get; set; }

        [JsonProperty("rawDescription")]
        public string RawDescription { get; set; }

        [JsonProperty("rawDisplayName")]
        public string RawDisplayName { get; set; }

        [JsonProperty("slot")]
        public long Slot { get; set; }
    }

    public partial class Runes
    {
        [JsonProperty("keystone")]
        public Keystone Keystone { get; set; }

        [JsonProperty("primaryRuneTree")]
        public Keystone PrimaryRuneTree { get; set; }

        [JsonProperty("secondaryRuneTree")]
        public Keystone SecondaryRuneTree { get; set; }
    }

    public partial class Keystone
    {
        [JsonProperty("displayName")]
        public string DisplayName { get; set; }

        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        public long? Id { get; set; }

        [JsonProperty("rawDescription")]
        public string RawDescription { get; set; }

        [JsonProperty("rawDisplayName")]
        public string RawDisplayName { get; set; }
    }

    public partial class Scores
    {
        [JsonProperty("assists")]
        public long Assists { get; set; }

        [JsonProperty("creepScore")]
        public long CreepScore { get; set; }

        [JsonProperty("deaths")]
        public long Deaths { get; set; }

        [JsonProperty("kills")]
        public long Kills { get; set; }

        [JsonProperty("wardScore")]
        public long WardScore { get; set; }
    }

    public partial class SummonerSpells
    {
        [JsonProperty("summonerSpellOne")]
        public Keystone SummonerSpellOne { get; set; }

        [JsonProperty("summonerSpellTwo")]
        public Keystone SummonerSpellTwo { get; set; }
    }
}

