using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ITSHotelGuest.Models
{
    public class SetConditRequest
    {
        [JsonProperty("condit")]
        public bool Condit { get; set; }
    }
}
