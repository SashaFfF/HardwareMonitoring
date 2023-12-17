using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Management;

namespace TestingHardwareMonitoring
{
    public class Prognosis
    {
        public int GetCountOfProcesses()
        {
            System.Diagnostics.Process[] processes= Process.GetProcesses();
            return processes.Length;
        }

        public double GetVoltage()
        {
            var query = new ObjectQuery("SELECT * FROM Win32_Battery");
            var searcher = new ManagementObjectSearcher(query);

            foreach (ManagementObject mo in searcher.Get())
            {
                 return Convert.ToDouble(mo["DesignVoltage"])/100.0;
            }
            return 0;
        }


//        if charge_percent >= 0 and voltage >= 0:
//            # Добавим условие, чтобы учесть все значения заряда от 0 до 100
//            adjusted_processes = num_processes
//# Добавим формулу с учетом всех факторов
//            battery_life_seconds = (charge_percent / 100) * (voltage / 10.8) * (1 - (num_processes / 300)) * 3600
//            return battery_life_seconds
//        else:
//            return "Низкая эффективность батареи в текущих условиях"
        public double GetOperatingTimeForecast(double percent)
        {
            double NumberOfProcesses = GetCountOfProcesses();
            double voltage = GetVoltage();
            if(percent>0 && voltage> 0)
            {
                return ((percent/100.0) * (voltage / 10.8) * (1-(NumberOfProcesses/300.0)) * 3600);
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
