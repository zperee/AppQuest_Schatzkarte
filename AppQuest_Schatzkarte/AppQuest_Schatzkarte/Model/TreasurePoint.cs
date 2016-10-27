using Newtonsoft.Json;

namespace AppQuest_Schatzkarte.Model
{
    public class TreasurePoint
    {
        [JsonProperty("lat")]
        public double Latitude { get; set; }

        [JsonProperty("lon")]
        public double Longitude { get; set; }
    }
}