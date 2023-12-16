using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace HardwareMonitoringLibrary
{
    public class GPULoad
    {
        public static float GetGPUUsage()
        {
            try
            {
                var gpuCounters = GetGPUCounters();
                gpuCounters.ForEach(x => x.NextValue());
                Thread.Sleep(1000);
                var result = gpuCounters.Sum(x => x.NextValue());
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting GPU usage: {ex.Message}");
                return 0;
            }
        }

        private static List<PerformanceCounter> GetGPUCounters()
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

    }
}
