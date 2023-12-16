using System.Management;

namespace HardwareMonitoringLibrary
{
    public class Battery
    {
        public int ChargePercentage { get; set; }
        public int Status { get; set; }
        public int TimeLeft { get; set; }
        public Battery() { }    
        public void GetBatteryInfo()
        {
            ObjectQuery query = new ObjectQuery("SELECT * FROM Win32_Battery");
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(query);

            foreach (ManagementObject info in searcher.Get())
            {
                ChargePercentage = Convert.ToInt32(info["Caption"]);
                Status = Convert.ToInt32(info["BatteryStatus"]);
                TimeLeft = Convert.ToInt32(info["EstimatedRunTime"]);
            }
        }
    }
}
