using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HardwareMonitoringUI.Converters
{
    public class BatteryTime
    {
        public static string RemainingChargeTime(int percent)
        {
            if(percent > 50)
            {
                return "2 ч. 06 мин.";

            }
            else
            {
                return "1 ч. 30 мин.";
            }
        }
    }
}
