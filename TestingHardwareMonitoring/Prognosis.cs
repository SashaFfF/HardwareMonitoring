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


        public void GetInfoaboutPr()
        {
            // Получаем все запущенные процессы
            Process[] processes = Process.GetProcesses();

            // Выводим информацию о каждом процессе
            foreach (Process proc in processes)
            {
                try
                {
                    // Получаем информацию о потреблении энергии
                    long energy = proc.WorkingSet64; // Это значение может быть использовано как оценка для потребления энергии

                    // Выводим информацию о процессе и его энергопотреблении
                    Console.WriteLine("Процесс: " + proc.ProcessName + ", ID: " + proc.Id + ", Энергопотребление: " + energy);
                }
                catch (Exception ex)
                {
                    // Обработка ошибок, если это необходимо
                    Console.WriteLine("Ошибка при получении информации о процессе: " + ex.Message);
                }
            }
        }

        //2 
        public double GetOperatingTimeForecastVersion2(double percent, double CPULoad, double RAMLoad)
        {
            double voltage = GetVoltage();
            if (percent > 0 && voltage > 0)
            {
                double adjustedPercent = percent / 100.0;

                double totalEnergyConsumption = 0;

                Process[] processes = Process.GetProcesses();

                foreach (var process in processes)
                {
                    totalEnergyConsumption += process.WorkingSet64;
                }

                double timeInSeconds = adjustedPercent * (voltage / 10.8) * totalEnergyConsumption *AdjustForWorkload(CPULoad, RAMLoad) * 3600;
                return timeInSeconds;
            }
            return 0;
        }

        //
        private double AdjustForWorkload(double cpuLoad, double memoryLoad)
        {
            return 1 - ((cpuLoad + memoryLoad) / 200);
        }

        //////////////////////////////////////////////////
        //public double GetNewTimeForecast(double percent)
        //{
        //    // Получаем напряжение батареи
        //    double voltage = GetVoltage();

        //    // Получаем информацию о запущенных процессах
        //    List<ProcessInfo> processesInfo = GetProcessesInfo();

        //    // Проверяем, что процент заряда и напряжение положительны
        //    if (percent > 0 && voltage > 0)
        //    {
        //        // Инициализируем переменные для общего взвешенного энергопотребления и общего веса
        //        double totalWeightedEnergyConsumption = 0;
        //        double totalWeight = 0;

        //        // Проходим по каждому процессу
        //        foreach (ProcessInfo processInfo in processesInfo)
        //        {
        //            // Рассчитываем вес процесса на основе его характеристик (в данном случае - энергопотребление)
        //            double weight = CalculateProcessWeight(processInfo);

        //            // Обновляем суммарный вес и взвешенное энергопотребление
        //            totalWeight += weight;
        //            totalWeightedEnergyConsumption += processInfo.EnergyConsumption;
        //        }

        //        // Получаем количество запущенных процессов
        //        double NumberOfProcesses = processesInfo.Count;

        //        // Учитываем уровень энергопотребления процессов, загруженность CPU и использование памяти
        //        double energyConsumptionFactor = 1 - (NumberOfProcesses / 300.0); // Базовый фактор уровня энергопотребления

        //        // Учитываем загрузку CPU и использование памяти
        //        double cpuLoad = GetCPULoad();  // Загрузка CPU в процентах (от 0 до 100)
        //        double memoryUsage = GetMemoryUsage();  // Использование оперативной памяти в процентах (от 0 до 100)
        //        energyConsumptionFactor *= (1 - cpuLoad / 100.0) * (1 - memoryUsage / 100.0);

        //        // Рассчитываем средневзвешенное энергопотребление процессов
        //        double weightedEnergyConsumption = totalWeightedEnergyConsumption / totalWeight;

        //        // Рассчитываем оставшееся время работы от батареи
        //        return ((percent / 100.0) * (voltage / 10.8) * energyConsumptionFactor * weightedEnergyConsumption * 3600);
        //    }

        //    // Если процент заряда или напряжение не положительны, возвращаем 0
        //    return 0;
        //}

        //// Пример функции, которая определяет вес процесса на основе его характеристик
        //private double CalculateProcessWeight(ProcessInfo processInfo)
        //{
        //    // Здесь можно использовать различные характеристики процесса для определения его веса
        //    // В данном примере вес равен энергопотреблению процесса
        //    return processInfo.EnergyConsumption;
        //}

    }
}
