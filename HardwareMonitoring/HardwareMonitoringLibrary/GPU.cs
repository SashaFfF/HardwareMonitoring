using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;

namespace HardwareMonitoringLibrary
{
    public class GPU
    {
        private ObjectQuery query;
        private ManagementObjectSearcher searcher;

        public string Name { get; set; }
        public int CurrentRefreshRate { get; set; }

    
        public GPU() 
        {
            query = new ObjectQuery("SELECT * FROM Win32_VideoController");
            searcher = new ManagementObjectSearcher(query);
            GetTotalInfo();
        }

        public void GetTotalInfo()
        {
            foreach (ManagementObject mo in searcher.Get())
            {
                Name = mo["Name"].ToString();
                CurrentRefreshRate = Convert.ToInt32(mo["CurrentRefreshRate"]);
            }
        }

        public static void GetInfo()
        {
            ObjectQuery query = new ObjectQuery("SELECT * FROM Win32_VideoController");
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(query);

            foreach (ManagementObject mo in searcher.Get())
            {
                Console.WriteLine($"Name: {mo["Name"]}");
                Console.WriteLine($"Adapter RAM: {mo["AdapterRAM"]} bytes");
                Console.WriteLine($"Driver Version: {mo["DriverVersion"]}");
                Console.WriteLine($"Video Processor: {mo["VideoProcessor"]}");
                Console.WriteLine($"Availability: {mo["Availability"]}");
                Console.WriteLine($"Caption: {mo["Caption"]}");
                Console.WriteLine($"Device ID: {mo["DeviceID"]}");
                Console.WriteLine($"Current Refresh Rate: {mo["CurrentRefreshRate"]} Hz");
                Console.WriteLine($"Current Bits Per Pixel: {mo["CurrentBitsPerPixel"]}");
                Console.WriteLine($"Current Horizontal Resolution: {mo["CurrentHorizontalResolution"]} pixels");
                Console.WriteLine($"Current Vertical Resolution: {mo["CurrentVerticalResolution"]} pixels");
                Console.WriteLine($"Max Refresh Rate: {mo["MaxRefreshRate"]} Hz");
                Console.WriteLine($"Video Mode Description: {mo["VideoModeDescription"]}");
                Console.WriteLine($"Video Architecture: {mo["VideoArchitecture"]}");
                Console.WriteLine($"Video Memory Type: {mo["VideoMemoryType"]}");
                Console.WriteLine($"Video Memory Type: {mo["VideoMemoryType"]}");
                Console.WriteLine($"Video Mode: {mo["VideoMode"]}");
                Console.WriteLine($"Video Memory Type: {mo["VideoMemoryType"]}");
                Console.WriteLine($"Video Mode: {mo["VideoMode"]}");
                Console.WriteLine($"Video Memory Type: {mo["VideoMemoryType"]}");
                Console.WriteLine($"Video Mode: {mo["VideoMode"]}");
                Console.WriteLine();
            }
        }
    }
}
