using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ITSHotelGuest.Models
{
    public class SetDoorRequest
    {
        [JsonProperty("passCode")]
        public string PassCode { get; set; }
    }
}
