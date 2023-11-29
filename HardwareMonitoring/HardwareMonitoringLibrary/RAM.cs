using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;

namespace HardwareMonitoringLibrary
{
    public class RAM
    {
        private static ObjectQuery query = new ObjectQuery("SELECT * FROM Win32_OperatingSystem");
        private static ManagementObjectSearcher searcher = new ManagementObjectSearcher(query);

        private static ulong TotalMemory;
        private static ulong FreeMemory;
        private static ulong LoadedMemory;

        private static void UpdateMemoryInfo()
        {
            foreach (ManagementObject info in searcher.Get())
            {
                TotalMemory = Convert.ToUInt64(info["TotalVisibleMemorySize"]);
                FreeMemory = Convert.ToUInt64(info["FreePhysicalMemory"]);
                LoadedMemory = TotalMemory - FreeMemory;
            }
        }

        public static ulong GetTotalMemory()
        {
            UpdateMemoryInfo();
            return TotalMemory;
        }

        public static ulong GetFreeMemory()
        {
            UpdateMemoryInfo(); 
            return FreeMemory;
        }

        public static double GetMemoryLoadPercentage()
        {
            UpdateMemoryInfo();
            return Convert.ToDouble(LoadedMemory) / Convert.ToDouble(TotalMemory) * 100;  
        }

        public static ulong GetLoadedMemory()
        {
            UpdateMemoryInfo();
            return LoadedMemory;
        }
    }
}
