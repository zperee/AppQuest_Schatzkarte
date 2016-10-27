using System.Collections.Generic;
using Newtonsoft.Json;

namespace AppQuest_Schatzkarte.Model
{
    public class SubmitObject
    {
        [JsonProperty("task")]
        public string Task => "Schatzkarte";

        [JsonProperty("points")]
        public List<TreasurePoint> Points { get; set; }
    }
}