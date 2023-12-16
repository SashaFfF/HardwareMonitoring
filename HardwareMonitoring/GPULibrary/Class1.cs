using OpenHardwareMonitor.Hardware;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPULibrary
{

    public class UpdateVisitor : IVisitor
    {
        public void VisitComputer(IComputer computer)
        {
            computer.Traverse(this);
        }
        public void VisitHardware(IHardware hardware)
        {
            hardware.Update();
            foreach (IHardware subHardware in hardware.SubHardware) subHardware.Accept(this);
        }
        public void VisitSensor(ISensor sensor) { }
        public void VisitParameter(IParameter parameter) { }
    }
    public class Class1
    {
        public static void get()
        {
            Computer computer = new Computer();
            computer.GPUEnabled = true;  // Включаем мониторинг GPU
            computer.Open();

            while (true)
            {
                computer.Accept(new UpdateVisitor());

                foreach (var hardware in computer.Hardware)
                {
                    if (hardware.HardwareType == HardwareType.GpuNvidia || hardware.HardwareType == HardwareType.GpuAti)
                    {
                        Console.WriteLine($"GPU Name: {hardware.Name}");

                        foreach (var sensor in hardware.Sensors)
                        {
                            if (sensor.SensorType == SensorType.Load)
                            {
                                Console.WriteLine($"GPU Load: {sensor.Value}%");
                            }
                            else if (sensor.SensorType == SensorType.Data && sensor.Name == "GPU Memory Total")
                            {
                                Console.WriteLine($"GPU Memory Total: {sensor.Value} KB");
                            }
                        }
                    }
                }

                System.Threading.Thread.Sleep(1000);
            }
        }
    }
}
