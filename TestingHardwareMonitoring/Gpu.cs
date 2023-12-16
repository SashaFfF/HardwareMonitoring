using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace TestingHardwareMonitoring
{
    public class Gpu
    {
        public static void GetiInfo()
        {
            while (true)
            {
                try
                {
                    var gpuCounters = GetGPUCounters();
                    var gpuUsage = GetGPUUsage(gpuCounters);
                    Console.WriteLine(Math.Round(gpuUsage));
                    continue;
                }
                catch { }

                Thread.Sleep(1000);
            }
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

            Thread.Sleep(1000);

            var result = gpuCounters.Sum(x => x.NextValue());

            return result;
        }
    }
}
