using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace HardwareMonitoringLibrary
{
    public class GPU
    {
        private ObjectQuery query;
        private ManagementObjectSearcher searcher;

        public string Name { get; set; }
        public int CurrentRefreshRate { get; set; }
        public double Load { get; set; }
        //public double TotalVideoMemory { get; set; }
        //public double VideoMemory { get; set; }



        public GPU() 
        {
            query = new ObjectQuery("SELECT * FROM Win32_VideoController");
            searcher = new ManagementObjectSearcher(query);
            GetTotalInfo();
            Task.Run(async () =>
            {
                await GetiInfoAboutLoad();
            }).Wait();
        }

        public void GetTotalInfo()
        {
            foreach (ManagementObject mo in searcher.Get())
            {
                Name = mo["Name"].ToString();
                CurrentRefreshRate = Convert.ToInt32(mo["CurrentRefreshRate"]);
               // TotalVideoMemory = Convert.ToUInt64(mo["AdapterRAM"]);
                //VideoMemory = Convert.ToUInt64(mo["AdapterRAM"]);
            }
        }

        public async Task GetLoadAsync()
        {
            await Task.Run(() =>
            {
                ManagementObjectSearcher searcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_PerfFormattedData_GPUPerformanceCounters_GPUEngine");
                foreach (ManagementObject queryObj in searcher.Get())
                {
                    Load = Convert.ToInt32(queryObj["UtilizationPercentage"]);
                }
            });
        }

        // 2 вариант получения загрузки gpu с помощью PerformanceCounterCategory из System.Diagnostics

        public async Task GetiInfoAboutLoad()
        {
            await Task.Run(() =>
            {
                var gpuCounters = GetGPUCounters();
                var gpuUsage = GetGPUUsage(gpuCounters);
                Load = Math.Round(gpuUsage, MidpointRounding.AwayFromZero);
            });
        }

        public static List<PerformanceCounter> GetGPUCounters()
        {
            var category = new PerformanceCounterCategory("GPU Engine");
            var counterNames = category.GetInstanceNames();

            var gpuCounters = counterNames
                                .Where(counterName => counterName.EndsWith("engtype_3D"))
                                .SelectMany(counterName => category.GetCounters(counterName))
                                .Where(counter => counter.CounterName.Equals("Utilization Percentage"))
                                .ToList();

            return gpuCounters;
        }

        public static float GetGPUUsage(List<PerformanceCounter> gpuCounters)
        {
            gpuCounters.ForEach(x => x.NextValue());

            //Thread.Sleep(1000);

            var result = gpuCounters.Sum(x => x.NextValue());

            return result;
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
