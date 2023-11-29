using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HardwareMonitoringLibrary.Drive
{
    public class Disk
    {
        public string Name { get; set; }
        public string FileSystem { get; set; }
        public long TotalSizeGb { get; set; }
        public double FreeSpaceGb { get; set; }
        public double UsedSpaceGb { get; set; }
        public int OccupancyPercentage { get; set; }
        public int FreePercentage { get; set; }
    }
}
