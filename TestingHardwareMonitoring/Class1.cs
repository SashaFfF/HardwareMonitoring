using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;

namespace TestingHardwareMonitoring
{
    internal class Class1
    {
        public static void GetGPU()
        {
            try
            {
                ManagementObjectSearcher searcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_VideoController");

                foreach (ManagementObject queryObj in searcher.Get())
                {
                    Console.WriteLine("DeviceID: {0}", queryObj["DeviceID"]);
                    Console.WriteLine("Name: {0}", queryObj["Name"]);
                    Console.WriteLine("Adapter RAM: {0} bytes", queryObj["AdapterRAM"]);
                    Console.WriteLine("Driver Version: {0}", queryObj["DriverVersion"]);
                    Console.WriteLine("Video Processor: {0}", queryObj["VideoProcessor"]);
                    Console.WriteLine();
                }
            }
            catch (ManagementException e)
            {
                Console.WriteLine("An error occurred while querying for WMI data: " + e.Message);
            }
        }

    }
}
