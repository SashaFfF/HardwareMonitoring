using HardwareMonitoringLibrary;
using HardwareMonitoringLibrary.Drive;
using BatteryLibrary;
using Battery = BatteryLibrary.Battery;


//Console.WriteLine(Processor.GetProcessorName());
//Console.WriteLine(Processor.GetNumberOfCores());
//Console.WriteLine(Processor.GetNumberOfThreads());

CPU cpu = new CPU();
Console.WriteLine($"CPU temp: {cpu.Temperature}");
//for(int i = 0;i < 10; i++)
//{

//    Console.WriteLine($"Load: {Processor.Monitor()}");
//}


////пданные о RAM получаем в Кб, вопрос в том, как переводить, использовать 1000 или 1024
//Console.WriteLine($"Total memory: {RAM.GetTotalMemory() / (1024.0 * 1024.0)} gb");
//Console.WriteLine($"Free memory: {RAM.GetFreeMemory() / (1024.0 * 1024.0)} gb" );
//Console.WriteLine($"Loaded memory: {(RAM.GetTotalMemory() - RAM.GetFreeMemory()) /  (1024.0 * 1024.0)} gb");
//Console.WriteLine($"Loaded %: {RAM.GetMemoryLoadPercentage()} %");

//Console.WriteLine($"Total memory: {RAM.GetTotalMemory() / (1000.0 * 1000.0)} gb");
//Console.WriteLine($"Free memory: {RAM.GetFreeMemory() / (1000.0 * 1000.0)} gb");
//Console.WriteLine($"Loaded memory: {(RAM.GetTotalMemory() - RAM.GetFreeMemory()) / (1000.0 * 1000.0)} gb");
//Console.WriteLine($"Loaded %: {RAM.GetMemoryLoadPercentage()} %");

//Drive.GetHardDriveInfo();

//Drive drive= new Drive();
//Console.WriteLine(drive.DriveModel);
//Console.WriteLine(drive.DriveMediaType);
//Console.WriteLine(drive.DriveSizeGb);

//foreach(Disk d in drive.Disks)
//{
//    Console.WriteLine(d.Name);
//    Console.WriteLine(d.FileSystem);
//    Console.WriteLine(d.TotalSizeGb);
//    Console.WriteLine(d.FreeSpaceGb);
//    Console.WriteLine(d.FreePercentage);
//    Console.WriteLine(d.UsedSpaceGb);
//    Console.WriteLine(d.OccupancyPercentage);
//}

//Battery battery = new Battery();

// Обновление информации о батарее
//battery.UpdateBatteryInfo();

// Вывод данных в консоль
//Console.WriteLine($"Battery Percent: {battery.BatteryPercent}%");
//Console.WriteLine($"Is Charging: {battery.IsCharging}");
//Console.WriteLine($"Time Remaining: {battery.TimeRemaining}");
//Console.WriteLine($"Time To Full Charge: {battery.TimeToFullCharge}");


GPU.GetInfo();

//Processor.MonitorGit();