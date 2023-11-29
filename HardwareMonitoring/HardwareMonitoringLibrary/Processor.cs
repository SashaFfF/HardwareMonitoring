using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;
using LibreHardwareMonitor.Hardware;

namespace HardwareMonitoringLibrary
{
    public class Processor
    {
        private static ObjectQuery query = new ObjectQuery("SELECT * FROM Win32_Processor");
        private static ManagementObjectSearcher searcher = new ManagementObjectSearcher(query);
        private static Computer computer = new Computer
        {
            IsCpuEnabled = true
        };

        //название 
        public static string GetProcessorName()
        {
            foreach (ManagementObject info in searcher.Get())
            {
                return info["Name"].ToString();
            }
            return "Нет данных";
        }

        //ядра
        public static int GetNumberOfCores()
        {
            foreach (ManagementObject info in searcher.Get())
            {
                return Convert.ToInt32(info["NumberOfCores"]);
            }
            return -1;
        }

        //потоки
        public static int GetNumberOfThreads()
        {
            foreach (ManagementObject info in searcher.Get())
            {
                return Convert.ToInt32(info["ThreadCount"]);
            }
            return -1;
        }

        //частота
        public static double GetProcessorFrequency()
        {
            foreach (ManagementObject info in searcher.Get())
            {
                return Convert.ToDouble(info["MaxClockSpeed"]);
            }
            return -1;
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

        //public static int GetProcessorLoad()
        //{
        //    foreach (ManagementObject info in searcher.Get())
        //    {
        //        return Convert.ToInt32(info["LoadPercentage"]);
        //    }

        //    return -1;
        //}

        public static float  MonitorTemp()
        {
            computer.Open();
            computer.Accept(new UpdateVisitor());

            foreach (IHardware hardware in computer.Hardware)
            {

                foreach (ISensor sensor in hardware.Sensors)
                {
                    if (sensor.SensorType == SensorType.Temperature) 
                    {
                        return sensor.Value ?? 0;
                    }
                }
            }

            computer.Close();
            return -1;
        }
        public static float Monitor()
        {
            computer.Open();
            computer.Accept(new UpdateVisitor());

            foreach (IHardware hardware in computer.Hardware)
            {

                foreach (ISensor sensor in hardware.Sensors)
                {
                    if (sensor.SensorType == SensorType.Load)
                    {
                        return  sensor.Value ?? 0;
                    }
                }
            }

            computer.Close();
            return -1;
        }

        public static void MonitorGit()
        {
            Computer computer = new Computer
            {
                IsCpuEnabled = true,
                IsGpuEnabled = true,
                IsMemoryEnabled = true,
                IsMotherboardEnabled = true,
                IsControllerEnabled = true,
                IsNetworkEnabled = true,
                IsStorageEnabled = true
            };

            computer.Open();
            computer.Accept(new UpdateVisitor());

            foreach (IHardware hardware in computer.Hardware)
            {
                Console.WriteLine("Hardware: {0}", hardware.Name);

                foreach (IHardware subhardware in hardware.SubHardware)
                {
                    Console.WriteLine("\tSubhardware: {0}", subhardware.Name);

                    foreach (ISensor sensor in subhardware.Sensors)
                    {
                        Console.WriteLine("\t\tSensor: {0}, value: {1}", sensor.Name, sensor.Value);
                    }
                }

                foreach (ISensor sensor in hardware.Sensors)
                {
                    Console.WriteLine("\tSensor: {0}, value: {1}", sensor.Name, sensor.Value);
                }
            }

            computer.Close();
        }


    }
}
