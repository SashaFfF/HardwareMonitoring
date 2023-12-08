using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;
using LibreHardwareMonitor.Hardware;

namespace HardwareMonitoringLibrary
{
    public class CPU
    {
        private ObjectQuery query;
        private ManagementObjectSearcher searcher;
        private Computer computer;

        public string Name { get; set; }
        public int NumberOfCores { get; set; }
        public int NumberOfThreads { get; set; }
        public double Frequency { get; set; }
        public int OccupancyPercentage { get; set; }
        public double Temperature { get; set; }


        public CPU()
        {
            query = new ObjectQuery("SELECT * FROM Win32_Processor");
            searcher = new ManagementObjectSearcher(query);
            computer = new Computer
            {
                IsCpuEnabled = true
            };

            GetTotalInfo();
            Task.Run(async () =>
            {
                await GetOccupancyPercentageAsync();
                await GetTemperatureAsync();
            }).Wait();
        }

        public void GetTotalInfo() {
            foreach (ManagementObject info in searcher.Get())
            {
                Name = info["Name"].ToString();
                NumberOfCores = Convert.ToInt32(info["NumberOfCores"]);
                NumberOfThreads = Convert.ToInt32(info["ThreadCount"]);
                Frequency = Convert.ToDouble(info["MaxClockSpeed"]);
            }
        }


        //название 
        public void GetProcessorName()
        {
            foreach (ManagementObject info in searcher.Get())
            {
                Name = info["Name"].ToString();
            }
        }

        //ядра
        public void GetNumberOfCores()
        {
            foreach (ManagementObject info in searcher.Get())
            {
                 NumberOfCores = Convert.ToInt32(info["NumberOfCores"]);
            }
        }

        //потоки
        public void GetNumberOfThreads()
        {
            foreach (ManagementObject info in searcher.Get())
            {
                NumberOfThreads = Convert.ToInt32(info["ThreadCount"]);
            }
        }

        //частота
        public void GetProcessorFrequency()
        {
            foreach (ManagementObject info in searcher.Get())
            {
               Frequency = Convert.ToDouble(info["MaxClockSpeed"]);
            }
        }

        //загруженность
        public static int GetProcessorLoad()
        {
            ObjectQuery query = new ObjectQuery("SELECT * FROM Win32_PerfFormattedData_PerfOS_Processor WHERE Name='_Total'");
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(query);

            foreach (ManagementObject info in searcher.Get())
            {
                return Convert.ToInt32(info["PercentProcessorTime"]);
            }

            return -1;
        }
        public async Task UpdateInfoAsync()
        {
            // Вызываем асинхронные методы напрямую
            await GetTemperatureAsync();
            await GetOccupancyPercentageAsync();
        }

        public async Task GetTemperatureAsync()
        {
            await Task.Run(() =>
            {
                computer.Open();
                computer.Accept(new UpdateVisitor());

                foreach (IHardware hardware in computer.Hardware)
                {
                    foreach (ISensor sensor in hardware.Sensors)
                    {
                        if (sensor.SensorType == SensorType.Temperature)
                        {
                            Temperature = sensor.Value ?? 0;
                        }
                    }
                }

                computer.Close();
            });
        }

        public async Task GetOccupancyPercentageAsync()
        {
            await Task.Run(() =>
            {
                computer.Open();
                computer.Accept(new UpdateVisitor());

                foreach (IHardware hardware in computer.Hardware)
                {
                    foreach (ISensor sensor in hardware.Sensors)
                    {
                        if (sensor.SensorType == SensorType.Load)
                        {
                            OccupancyPercentage = Convert.ToInt32(sensor.Value ?? 0);
                        }
                    }
                }

                computer.Close();
            });
        }





        //public void UpdateInfo()
        //{
        //    GetTemperature();
        //    GetOccupancyPercentage();
        //}

        //public void GetTemperature()
        //{
        //    computer.Open();
        //    computer.Accept(new UpdateVisitor());

        //    foreach (IHardware hardware in computer.Hardware)
        //    {

        //        foreach (ISensor sensor in hardware.Sensors)
        //        {
        //            if (sensor.SensorType == SensorType.Temperature) 
        //            {
        //                Temperature = sensor.Value ?? 0;
        //            }
        //        }
        //    }

        //    computer.Close();
        //}
        //public void GetOccupancyPercentage()
        //{
        //    computer.Open();
        //    computer.Accept(new UpdateVisitor());

        //    foreach (IHardware hardware in computer.Hardware)
        //    {

        //        foreach (ISensor sensor in hardware.Sensors)
        //        {
        //            if (sensor.SensorType == SensorType.Load)
        //            {
        //                OccupancyPercentage =Convert.ToInt32(sensor.Value ?? 0);
        //            }
        //        }
        //    }

        //    computer.Close();
        //}

        //public static void MonitorGit()
        //{
        //    Computer computer = new Computer
        //    {
        //        IsCpuEnabled = true,
        //        IsGpuEnabled = true,
        //        IsMemoryEnabled = true,
        //        IsMotherboardEnabled = true,
        //        IsControllerEnabled = true,
        //        IsNetworkEnabled = true,
        //        IsStorageEnabled = true
        //    };

        //    computer.Open();
        //    computer.Accept(new UpdateVisitor());

        //    foreach (IHardware hardware in computer.Hardware)
        //    {
        //        Console.WriteLine("Hardware: {0}", hardware.Name);

        //        foreach (IHardware subhardware in hardware.SubHardware)
        //        {
        //            Console.WriteLine("\tSubhardware: {0}", subhardware.Name);

        //            foreach (ISensor sensor in subhardware.Sensors)
        //            {
        //                Console.WriteLine("\t\tSensor: {0}, value: {1}", sensor.Name, sensor.Value);
        //            }
        //        }

        //        foreach (ISensor sensor in hardware.Sensors)
        //        {
        //            Console.WriteLine("\tSensor: {0}, value: {1}", sensor.Name, sensor.Value);
        //        }
        //    }

        //    computer.Close();
        //}


    }
}
