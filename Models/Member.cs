using System;
using Newtonsoft.Json;

namespace react_dot_app.Models
{
    public class Member
    {
        [JsonProperty(PropertyName="name")]
        public string Name{get; set;}
        [JsonProperty(PropertyName="instrument")]
        public string Instrument{get; set;}
    }
}