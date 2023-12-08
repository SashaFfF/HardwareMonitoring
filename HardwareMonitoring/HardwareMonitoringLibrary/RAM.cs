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

        public double TotalMemory { get; set; }
        public double FreeMemory { get; set; }
        public double LoadedMemory { get; set; }
        public double MemoryLoadPercentage { get; set; }

        public RAM() 
        {
            GetTotalInfo();
        }

        public async 
        Task
UpdateMemoryInfoAsync()
        {
            await Task.Run(() =>
            {
                foreach (ManagementObject info in searcher.Get())
                {
                    FreeMemory = Math.Round((Convert.ToDouble(info["FreePhysicalMemory"]) / (1024.0 * 1024.0)), 1);
                    LoadedMemory = Math.Round(TotalMemory - FreeMemory, 1);
                    MemoryLoadPercentage = Math.Round(Convert.ToDouble(LoadedMemory) / Convert.ToDouble(TotalMemory) * 100, 1);
                }
            });
        }

        public void GetTotalInfo()
        {
            foreach (ManagementObject info in searcher.Get())
            {
                TotalMemory = Math.Round((Convert.ToDouble(info["TotalVisibleMemorySize"]) / (1024.0 * 1024.0)), 1);
                FreeMemory = Math.Round((Convert.ToDouble(info["FreePhysicalMemory"]) / (1024.0 * 1024.0)), 1);
                LoadedMemory = Math.Round(TotalMemory - FreeMemory, 1);
                MemoryLoadPercentage = Math.Round(Convert.ToDouble(LoadedMemory) / Convert.ToDouble(TotalMemory) * 100, 1);
            }
        }

        /*
        public double GetTotalMemory()
        {
            UpdateMemoryInfo();
            return TotalMemory;
        }

        public double GetFreeMemory()
        {
            UpdateMemoryInfo(); 
            return FreeMemory;
        }

        public double GetMemoryLoadPercentage()
        {
            UpdateMemoryInfo();
            return Convert.ToDouble(LoadedMemory) / Convert.ToDouble(TotalMemory) * 100;  
        }

        public double GetLoadedMemory()
        {
            UpdateMemoryInfo();
            return LoadedMemory;
        }
        */
    }
}
