using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;

namespace TestingHardwareMonitoring
{
    public enum CacheLevel : ushort
    {
        Level1 = 3,
        Level2 = 4,
        Level3 = 5,
    }
    public class Cache
    {
            public static List<uint> GetCacheSizes(CacheLevel level)
            {
                ManagementClass mc = new ManagementClass("Win32_CacheMemory");
                ManagementObjectCollection moc = mc.GetInstances();
                List<uint> cacheSizes = new List<uint>(moc.Count);

                cacheSizes.AddRange(moc
                  .Cast<ManagementObject>()
                  .Where(p => (ushort)(p.Properties["Level"].Value) == (ushort)level)
                  .Select(p => (uint)(p.Properties["MaxCacheSize"].Value)));

                return cacheSizes;
            }

        public static void print()
        {
            List<uint> s = GetCacheSizes(CacheLevel.Level1);
            foreach (uint size in s)
            {
                Console.WriteLine(size);
            }
        }

        static  uint cachsize1;
        static uint cachsize2;
        static uint cachsize3;
        public static void CPUSpeed()
        {
            using (ManagementObject Mo = new ManagementObject("Win32_Processor.DeviceID='CPU0'"))
            {
                //cachsize1 = (uint)(Mo["L1CacheSize"]);
                cachsize2 = (uint)(Mo["L2CacheSize"]);
                cachsize3 = (uint)(Mo["L3CacheSize"]);
            }
            //Console.WriteLine(cachsize1);
            Console.WriteLine(cachsize2);
            Console.WriteLine(cachsize3);
        }
    }
}
