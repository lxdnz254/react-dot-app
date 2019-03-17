using System;
using Newtonsoft.Json;

namespace react_dot_app.Models
{
    public class Genre
    {
        [JsonProperty(PropertyName="name")]
        public string Name {get; set; }
    }
}