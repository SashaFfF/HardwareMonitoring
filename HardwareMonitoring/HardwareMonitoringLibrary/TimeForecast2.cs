using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;

namespace HardwareMonitoringLibrary
{
    public class TimeForecast2
    {
        public CPU CPU { get; set; }
        public RAM RAM { get; set; }
        public TimeForecast2()
        {
            CPU = new CPU();
            RAM = new RAM();
        }

        //количество запущенных процессов
        public int GetCountOfProcesses()
        {
            System.Diagnostics.Process[] processes = Process.GetProcesses();
            return processes.Length;
        }

        //вольтаж батареи
        public double GetVoltage()
        {
            var query = new ObjectQuery("SELECT * FROM Win32_Battery");
            var searcher = new ManagementObjectSearcher(query);

            foreach (ManagementObject mo in searcher.Get())
            {
                return Convert.ToDouble(mo["DesignVoltage"]) / 100.0;
            }
            return 0;
        }

        public Dictionary<int, Type> GetProcessesTypeDictionary()
        {
            Dictionary<int, Type> processesTypeDictionary = new Dictionary<int, Type>();
            Process[] processes = Process.GetProcesses();
            foreach (Process process in processes)
            {
                var processName = process.ProcessName.ToLower();
                if (processName.Contains("chrome") || processName.Contains("firefox") || processName.Contains("opera") || processName.Contains("msedge") || processName.Contains("iexplore") || processName.Contains("browser"))
                {
                    processesTypeDictionary[process.Id] = new Type("browser", 0.4);
                }
                else if (processName.Contains("winword") || processName.Contains("excel") || processName.Contains("powerpnt") || processName.Contains("outlook"))
                {
                    processesTypeDictionary[process.Id] = new Type("office", 0.1);
                }
                else if (processName.Contains("vlc") || processName.Contains("itunes") || processName.Contains("wmplayer"))
                {
                    processesTypeDictionary[process.Id] = new Type("multimedia", 0.3);
                }
                else if (processName.Contains("photoshop") || processName.Contains("illustrator") || processName.Contains("gimp"))
                {
                    processesTypeDictionary[process.Id] = new Type("graphics", 0.3);
                }
                else if (processName.Contains("explorer") || processName.Contains("taskmgr") || processName.Contains("cmd") || processName.Contains("powershell"))
                {
                    processesTypeDictionary[process.Id] = new Type("system", 0.1);

                }
                else if (processName.Contains("skype") || processName.Contains("discord") || processName.Contains("ktalk"))
                {
                    processesTypeDictionary[process.Id] = new Type("communication", 0.2);
                }
                else if (processName.Contains("svchost") || processName.Contains("lsass") || processName.Contains("csrss"))
                {
                    processesTypeDictionary[process.Id] = new Type("system_service", 0.1);
                }
                else
                {
                    processesTypeDictionary[process.Id] = new Type("other", 0.1);
                }
            }
            return processesTypeDictionary;
        }

        public double GetOperatingTimeForecast(double percent)
        {
            double procesEnergyConsumptionCoefficient;
            string processTypeName = GetMaxCoefficientProcessType();
            switch (processTypeName)
            {
                case "browser":
                    procesEnergyConsumptionCoefficient = 0.4;
                    break;
                case "office":
                    procesEnergyConsumptionCoefficient = 0.1;
                    break;
                case "multimedia":
                    procesEnergyConsumptionCoefficient = 0.3;
                    break;
                case "graphics":
                    procesEnergyConsumptionCoefficient = 0.3;
                    break;
                case "system":
                    procesEnergyConsumptionCoefficient = 0.1;
                    break;
                case "communication":
                    procesEnergyConsumptionCoefficient = 0.2;
                    break;
                case "system_service":
                    procesEnergyConsumptionCoefficient = 0.1;
                    break;
                default:
                    procesEnergyConsumptionCoefficient = 0.1;
                    break;
            }

            double voltage = GetVoltage();
            double numberOfProcesses = GetCountOfProcesses();

            double chargeCoefficient = percent / 100.0;
            double voltageCoefficient = voltage / 10.8;
            double numberOfProcessesCoefficient = 1.2 * numberOfProcesses / 300;
            double ramCoefficient = 1 - (RAM.MemoryLoadPercentage / 100.0);
            double cpuCoefficient = 1 - (CPU.OccupancyPercentage/ 100.0);

            var time = chargeCoefficient * voltageCoefficient *
                        (1-procesEnergyConsumptionCoefficient) * ramCoefficient * cpuCoefficient * 3600;

            return time;
        }

        public string TimeConverter(double seconds)
        {
            int hours = (int)(seconds / 3600);
            int minutes = (int)((seconds % 3600) / 60);
            if (hours > 0)
            {
                return $"{hours} ч. {minutes} мин.";
            }
            else
            {
                return $"{minutes} мин.";
            }
        }
        public string UpdateTime(double percent)
        {
            double seconds = GetOperatingTimeForecast(percent);
            return TimeConverter(seconds);
        }

        public string GetMaxCoefficientProcessType()
        {
            var processesTypeDictionary = GetProcessesTypeDictionary();
            //максимальное значение Coefficient
            double maxCoefficient = processesTypeDictionary.Max(kv => kv.Value.Coefficient);
            //первый элемент с максимальным Coefficient
            var maxCoefficientType = processesTypeDictionary.FirstOrDefault(kv => kv.Value.Coefficient == maxCoefficient).Value;
            return maxCoefficientType.ProcessType;
        }

    }

    public class Type
    {
        public string ProcessType { get; set; }
        public double Coefficient { get; set; }
        public Type(string type, double coefficient)
        {
            ProcessType = type;
            Coefficient = coefficient;
        }

    }
}
