using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ITSHotelGuest.Models
{
    public class RoomData
    {
        public string RoomNumber { get; set; }

        [Display(Name = "Temperature")]
        public decimal Temperature { get; set; }
        //[Display(Name = "Alarm time Hour")]
        //public int AlarmTimeH { get; set; }
        //[Display(Name = "Alarm time Minutes")]
        //public int AlarmTimeM { get; set; }
        [Display(Name = "Alarm time")]
        public DateTime AlarmTime { get; set; }
        [Display(Name = "Alarm set")]
        public bool AlarmSet { get; set; }
        [Display(Name = "Light")]
        public bool Light { get; set; }
        [Display(Name = "PassCode")]
        public string PassCode { get; set; }
        [Display(Name = "Conditioner set")]
        public bool ConditionerSet { get; set; }
        [Display(Name = "Conditioner Temperature")]
        public decimal ConditionerTemperature { get; set; }
    }
}
