using LibreHardwareMonitor.Hardware;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace HardwareMonitoringLibrary.Drive
{
    public class SSD
    {
        private static ObjectQuery query = new ObjectQuery("SELECT * FROM Win32_DiskDrive");
        private ManagementObjectSearcher searcher = new ManagementObjectSearcher(query);

        public int DriveSizeGb { get; set; }
        public string DriveModel { get; set; }
        public string DriveMediaType { get; set; }

        public List<Disk> Disks { get; set; }

        public SSD()
        {
            Disks = new List<Disk>();
            GetDriveInfo();
            GetDisksInfo();
        }


        public void GetDriveInfo()
        {
            foreach (ManagementObject info in searcher.Get())
            {
                DriveModel = info["Model"].ToString()!;
                DriveMediaType = info["MediaType"].ToString()!;
                DriveSizeGb = Convert.ToInt32(Convert.ToUInt64(info["Size"]) / (1024 * 1024 * 1024));
            }
        }

        public void GetDisksInfo()
        {
            DriveInfo[] allDrives = DriveInfo.GetDrives();

            foreach (DriveInfo drive in allDrives)
            {
                Disks.Add(new Disk
                {
                    Name = drive.Name,
                    FileSystem = drive.DriveFormat,
                    TotalSizeGb = drive.TotalSize / (1024 * 1024 * 1024),

                    FreeSpaceGb = Math.Round(drive.TotalFreeSpace / (1024.0 * 1024.0 * 1024.0), 1), //округляю объем свободной памяти до 1 знака после запятой
                    FreePercentage = (int)Math.Round(drive.TotalFreeSpace * 100 / Convert.ToDouble(drive.TotalSize)), //процент свободного пространства на диске, округляю процент

                    UsedSpaceGb = Math.Round((drive.TotalSize - drive.TotalFreeSpace) / (1024.0 * 1024.0 * 1024.0), 1), //округляю объем занятой памяти до 1 знака после запятой
                    OccupancyPercentage = (int)Math.Round((drive.TotalSize - drive.TotalFreeSpace) * 100 / Convert.ToDouble(drive.TotalSize)) //процент загруженности диска, округляю процент
                });
            }
        }

    }
}
