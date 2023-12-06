using LibreHardwareMonitor.Hardware;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;

namespace HardwareMonitoringLibrary
{
    public class CPUAsync
    {
            private static ObjectQuery query= new ObjectQuery("SELECT * FROM Win32_Processor");
            private static ManagementObjectSearcher searcher  = new ManagementObjectSearcher(query);
        private static Computer computer = new Computer { IsCpuEnabled = true };

        public string Name { get; set; }
            public int NumberOfCores { get; set; }
            public int NumberOfThreads { get; set; }
            public double Frequency { get; set; }
            public double OccupancyPercentage { get; set; }
            public float Temperature { get; set; }

            public CPUAsync()
            {
                
                
                

                InitializeAsync().Wait(); // Блокирующий вызов, рассмотрите использование async по всей цепочке вызовов
            }

            private async Task InitializeAsync()
            {
                await Task.WhenAll(
                    GetTotalInfoAsync(),
                    GetOccupancyPercentageAsync(),
                    GetTemperatureAsync()
                );
            }

            public async Task GetTotalInfoAsync()
            {
                await Task.Run(() =>
                {
                    foreach (ManagementObject info in searcher.Get())
                    {
                        Name = info["Name"].ToString();
                        NumberOfCores = Convert.ToInt32(info["NumberOfCores"]);
                        NumberOfThreads = Convert.ToInt32(info["ThreadCount"]);
                        Frequency = Convert.ToDouble(info["MaxClockSpeed"]);
                    }
                });
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
                                OccupancyPercentage = sensor.Value ?? 0;
                            }
                        }
                    }

                    computer.Close();
                });
            }
        }
    }
