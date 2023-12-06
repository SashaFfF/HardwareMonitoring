using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BatteryLibrary
{
    public class Battery
    {
        public float BatteryPercent { get; private set; }
        public bool IsCharging { get; private set; }
        public string TimeRemaining { get; private set; }
        public string TimeToFullCharge { get; private set; }

        public void UpdateBatteryInfo()
        {
            BatteryPercent = SystemInformation.PowerStatus.BatteryLifePercent * 100;
            IsCharging = SystemInformation.PowerStatus.PowerLineStatus == PowerLineStatus.Online;

            if (IsCharging)
            {
                TimeToFullCharge = GetTimeToFullCharge();
            }
            else
            {
                TimeRemaining = GetTimeRemaining();
                TimeToFullCharge = null;
            }
        }

        private string GetTimeRemaining()
        {
            int secondsRemaining = SystemInformation.PowerStatus.BatteryLifeRemaining;
            TimeSpan timeSpan = TimeSpan.FromSeconds(secondsRemaining);
            return timeSpan.ToString(@"hh\:mm\:ss");
        }

        private string GetTimeToFullCharge()
        {
            try
            {
                ManagementObjectSearcher searcher = new ManagementObjectSearcher("root\\cimv2", "SELECT * FROM Win32_Battery");

                foreach (ManagementObject queryObj in searcher.Get())
                {
                    if (queryObj["BatteryStatus"].ToString() == "2") // Charging
                    {
                        int secondsToFullCharge = Convert.ToInt32(queryObj["EstimatedChargeRemaining"]);
                        TimeSpan timeSpan = TimeSpan.FromSeconds(secondsToFullCharge);
                        return timeSpan.ToString(@"hh\:mm\:ss");
                    }
                }
            }
            catch (ManagementException)
            {
                // Обработка ошибок, если не удалось получить информацию
            }

            return null;
        }
    }
}
