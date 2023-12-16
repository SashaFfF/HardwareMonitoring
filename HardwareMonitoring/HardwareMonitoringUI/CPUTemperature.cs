using HardwareMonitoringLibrary;
using LibreHardwareMonitor.Hardware;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HardwareMonitoringUI
{
    public class Visitor : IVisitor
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
    public class CPUTemperature
    {
        public double Temperature { get; set; }
        Computer comp = new Computer
        {
            IsCpuEnabled = true
        };

        public CPUTemperature()
        {
            Task.Run(async () =>
            {
                await GetTempAsync();
            }).Wait();

            
        }

        public async Task GetTempAsync()
        {
            await Task.Run(() =>
            {
                comp.Open();
                comp.Accept(new UpdateVisitor());
                foreach (IHardware hardware in comp.Hardware)
                {
                    foreach (ISensor sensor in hardware.Sensors)
                    {
                        if (sensor.SensorType == SensorType.Temperature)
                        {
                            Temperature = Convert.ToInt32(sensor.Value ?? 0);
                        }
                    }
                }

                comp.Close();
            });
        }

    }
}
