using Newtonsoft.Json;

namespace Chaos.Models
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<List<Root>>(myJsonResponse);
    public class UserPref
    {
        [JsonProperty("1")]
        public bool _1 { get; set; }

        [JsonProperty("2")]
        public bool _2 { get; set; }

        [JsonProperty("3")]
        public bool _3 { get; set; }

        [JsonProperty("4")]
        public bool _4 { get; set; }

        [JsonProperty("5")]
        public bool _5 { get; set; }

        [JsonProperty("6")]
        public bool _6 { get; set; }

        [JsonProperty("7")]
        public bool _7 { get; set; }
    }


}
