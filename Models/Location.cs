using Newtonsoft.Json;

namespace web_fitness.Models
{
    public class Location
    {
        [JsonProperty("lat")]
        public string lat { get; set; }

        [JsonProperty("lon")]
        public string lon { get; set; }

    }
}
