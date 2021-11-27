//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Newtonsoft.Json;
//using Newtonsoft.Json.Converters;
//using Newtonsoft.Json.Linq;

//namespace LeagueAzusa
//{
//    //[JsonObject(MemberSerialization.Fields)]
//    internal class Player1
//    {
//        public static string blueTeam = "ORDER";
//        public static string redTeam = "CHAOS";

//        //静态数据
//        public string summonerName;
//        public string championName;
//        [JsonConverter(typeof(MyJsonConverter<Runes>))]
//        public Runes runes;
//        public string team;
//        //当作不可变的,暂时不考虑换召唤师技能基石
//        [JsonConverter(typeof(MyJsonConverter<SummonerSpells>))]
//        public SummonerSpells summonerSpells;

//        //动态数据
//        public bool isDead;
//        public int level;
//        public float respawnTimer;
//        public Scores scores;

//        public void UpdateData(Player player)
//        {
//            this.isDead = player.isDead;
//            this.level = player.level;
//            this.respawnTimer = player.respawnTimer;

//            this.scores.assists = player.scores.assists;
//            this.scores.deaths = player.scores.deaths;
//            this.scores.kills = player.scores.kills; 
//            this.scores.wardScore = player.scores.wardScore;
//        }
//    }
//    internal class Runes
//    {
//        SingleRune keystone;
//        SingleRune primaryStone;
//        SingleRune secondaryRune;
//        public Runes(SingleRune k,SingleRune p,SingleRune s)
//        {
//            this.keystone = k;
//            this.primaryStone = p;
//            this.secondaryRune = s;
//        }
//    }
//    //public class tempConverter<T>:CustomCreationConverter<T>
//    //{
//    //    public override T Create(Type objectType)
//    //    {
            
//    //    }
//    //}
//    //Converter
//    internal class MyJsonConverter<T>:JsonConverter
//    {
//        public override bool CanConvert(Type objectType)
//        {
//            return true;
//        }
//        public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
//        {
//            if(objectType == typeof(Runes))
//            {
//                JObject jo = JObject.Parse((string)reader.Value);
//                SingleRune keystone = jo["keystone"].ToObject<SingleRune>();
//                SingleRune primaryRune = jo["primaryRuneTree"].ToObject<SingleRune>();
//                SingleRune secondaryRune = jo["keyStoneTree"].ToObject<SingleRune>();

//                Runes runes = new Runes(keystone,primaryRune,secondaryRune);
//                return runes;
//            }
//            else if(objectType == typeof(SummonerSpells))
//            {
//                JObject jo = JObject.Parse((string)reader.Value);
//                SingleSummonerSpell one = jo["summonerSpellOne"].ToObject<SingleSummonerSpell>();
//                SingleSummonerSpell two = jo["summonerSpellTwo"].ToObject<SingleSummonerSpell>();

//                SummonerSpells spells = new SummonerSpells(one, two);
//                return spells;
//            }
//            return serializer.Deserialize<T>(reader);
//        }
//        public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
//        {
//            serializer.Serialize(writer, value);
//        }
//    }
//    internal class SingleRune
//    {
//        public string displayName;
//        public SingleRune(string name)
//        {
//            this.displayName = name;
//        }
//    }
//    internal class Scores
//    {
//        public int assists;
//        public int creepScore;
//        public int deaths;
//        public int kills;
//        public float wardScore;
//    }
//    internal class SummonerSpells
//    {
//        SingleSummonerSpell summonerSpellOne;
//        SingleSummonerSpell summonerSpellTwo;
//        public SummonerSpells(SingleSummonerSpell o,SingleSummonerSpell t)
//        {
//            this.summonerSpellOne = o;
//            this.summonerSpellTwo = t;
//        }
//    }
//    internal class SingleSummonerSpell
//    {
//        string displayName;
//        public SingleSummonerSpell(string name)
//        {
//            this.displayName = name;
//        }
//    }

//}
