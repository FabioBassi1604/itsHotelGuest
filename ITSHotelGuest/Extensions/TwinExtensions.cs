using Microsoft.Azure.Devices.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ITSHotelGuest.Extensions
{
    public static class TwinExtensions
    {
        public static string DesiredProperty(this Twin that, string name)
        {
            if (that == null) return null;
            if (!that.Properties.Desired.Contains(name)) return null;
            return that.Properties.Desired[name];
        }

        public static bool IsDesiredpropertyEmpty(this Twin that, string name)
        {
            return DesiredProperty(that, name) == null ? false : true;
        }
    }
}
