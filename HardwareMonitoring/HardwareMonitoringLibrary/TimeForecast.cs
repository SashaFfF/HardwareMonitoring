using System.Diagnostics;
using System.Management;

namespace HardwareMonitoringLibrary
{
    public class TimeForecast
    {

        public string UpdateTime(double percent)
        {
            double seconds = GetOperatingTimeForecast(percent);
            return TimeConverter(seconds);
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

        //прогноз времени работы ноутбука в секундах 
        public double GetOperatingTimeForecast(double percent)
        {
            double NumberOfProcesses = GetCountOfProcesses();
            double voltage = GetVoltage();
            if (percent > 0 && voltage > 0)
            {
                return ((percent / 100.0) * (voltage / 10.8) * (1 - (NumberOfProcesses / 300.0)) * 3600);
            }
            return 0;
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
    }
}
