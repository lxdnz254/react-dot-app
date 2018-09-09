using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
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