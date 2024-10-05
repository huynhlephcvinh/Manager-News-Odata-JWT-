﻿using Newtonsoft.Json;

namespace FE_NewsManagementSystem_HuynhLePhucVinh.Models
{
    public class BadRequestModel
    {
        [JsonProperty("value")]
        public string Value { get; set; }
        [JsonProperty("@odata.context")]
        public string Context { get; set; }
    }
    
}
