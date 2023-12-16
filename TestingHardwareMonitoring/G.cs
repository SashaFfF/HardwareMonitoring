using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenHardwareMonitor.Hardware;
using System.Threading.Tasks;
using HardwareMonitoringLibrary;
using System.Management;
using NvAPIWrapper;
using NvAPIWrapper;
using NvAPIWrapper.Display;
using NvAPIWrapper.GPU;

namespace TestingHardwareMonitoring
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

    public class G
    {
        public static void get()
        {
            Console.WriteLine("Информация о GPU:");

            ObjectQuery query = new ObjectQuery("SELECT * FROM Win32_VideoController");
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(query);

            foreach (ManagementObject videoController in searcher.Get())
            {
                Console.WriteLine($"Имя: {videoController["Caption"]}");
                Console.WriteLine($"Загрузка GPU: {videoController["LoadPercentage"]}%");
                Console.WriteLine($"Используемая графическая память: {videoController["AdapterRAM"]} байт");
                Console.WriteLine();
            }
        }
        public static void g()
        {
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_VideoController");

            foreach (ManagementObject queryObj in searcher.Get())
            {
                Console.WriteLine("DeviceID: {0}", queryObj["DeviceID"]);
                Console.WriteLine("Name: {0}", queryObj["Name"]);

                // Общий объем видеопамяти
                ulong totalVideoMemory = Convert.ToUInt64(queryObj["AdapterRAM"]);
                Console.WriteLine("Total Video Memory: {0} bytes", totalVideoMemory);
                Console.WriteLine("Total Video Memory: {0} MB", totalVideoMemory / (1024 * 1024));

                // Выделенный объем видеопамяти
                string dedicatedVideoMemoryString = queryObj["AdapterCompatibility"] as string;
                ulong dedicatedVideoMemory;

                if (dedicatedVideoMemoryString != null && ulong.TryParse(dedicatedVideoMemoryString, out dedicatedVideoMemory))
                {
                    Console.WriteLine("Dedicated Video Memory: {0} bytes", dedicatedVideoMemory);
                    Console.WriteLine("Dedicated Video Memory: {0} MB", dedicatedVideoMemory / (1024 * 1024));
                }
                else
                {
                    Console.WriteLine("Unable to retrieve dedicated video memory information.");
                }
            }
        }

        public static void GetN()
        {
            // Инициализация NVAPI
            NVIDIA.Initialize();

            // Получение списка всех подключенных физических GPU
            PhysicalGPU[] physicalGPUs = PhysicalGPU.GetPhysicalGPUs();

            if (physicalGPUs.Length > 0)
            {
                Console.WriteLine($"Найдено {physicalGPUs.Length} физических GPU(ов):");

                // Вывод информации о каждом физическом GPU
                foreach (var gpu in physicalGPUs)
                {
                    Console.WriteLine($"GPU: {gpu.FullName}");
                    Console.WriteLine($"VBIOS: {gpu.Bios}");
                    Console.WriteLine($"Графическая плата: {gpu.Board}");
                    //Console.WriteLine($"Информация о шине: {gpu.ECCMemoryInformation}");
                    //Console.WriteLine($"Количество ядер CUDA: {gpu.CUDACores}");
                    //Console.WriteLine($"Текущая ширина PCIE: {gpu.CurrentPCIEDownStreamWidth}");
                    //Console.WriteLine($"Тип GPU: {gpu.GPUType}");
                    //Console.WriteLine($"IRQ: {gpu.IRQ}");
                    //Console.WriteLine($"Quadro: {gpu.IsQuadro}");
                    //Console.WriteLine($"Полная информация о памяти: {gpu.MemoryInfo}");
                    //Console.WriteLine($"PCI идентификаторы: {gpu.PCIIdentifiers}");
                    //Console.WriteLine($"Физический размер буфера кадра: {gpu.PhysicalFrameBufferSize} KB");
                    //Console.WriteLine($"Виртуальный размер буфера кадра: {gpu.VirtualFrameBufferSize} KB");
                    //Console.WriteLine($"Количество шейдерных подпрограмм: {gpu.ShaderSubPipeLines}");

                    Console.WriteLine(); // Пустая строка для разделения информации о каждом GPU
                }
            }
            else
            {
                Console.WriteLine("Не найдено подключенных физических GPU.");
            }

            // Освобождение ресурсов NVAPI
            NVIDIA.Unload();

        }
  
    }
}

