using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace react_dot_app.Models
{
    public class Venue
    {
        [JsonProperty(PropertyName="name")]
        public string Name {get; set;}
        [JsonProperty(PropertyName="capacity")]
        public int Capacity {get; set;}
        [JsonProperty(PropertyName="location")]
        public string Location {get; set;}
    }
}