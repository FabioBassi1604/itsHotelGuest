using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ITSHotelGuest.Models
{
    public class SetAlarmToggleRequest
    {
        [JsonProperty("toggleA")]
        public bool ToggleA { get; set; }
    }
}
