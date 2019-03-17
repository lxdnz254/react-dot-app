using System;
using Newtonsoft.Json;

namespace react_dot_app.Models
{
    public class Band
        {
            [JsonProperty(PropertyName= "name")]
            public string Name {get; set;}
            [JsonProperty(PropertyName= "rating")]
            public int Rating {get; set;}
            [JsonProperty(PropertyName= "cost")]
            public int Cost {get; set;}
        }
}