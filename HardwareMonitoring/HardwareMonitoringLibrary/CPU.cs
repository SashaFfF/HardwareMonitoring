using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
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
        public int Temperature { get; set; }


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

                await GetTemperatureAsync();
                await GetOccupancyPercentageAsync();
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
            //await GetTemperatureAsync();
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
                            Temperature = Convert.ToInt32(sensor.Value ?? 0);
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

    }
}
