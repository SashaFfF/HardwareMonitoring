using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;

namespace TestingHardwareMonitoring
{
    internal class BatteryInf
    {
        public void get()
        {
            ObjectQuery query = new ObjectQuery("SELECT * FROM Win32_Battery");
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(query);

            ManagementObjectCollection queryCollection = searcher.Get();

            foreach (ManagementObject m in queryCollection)
            {
                Console.WriteLine("Caption: " + m["Caption"]);
                Console.WriteLine("Battery Status: " + m["BatteryStatus"]);
                Console.WriteLine("Estimated Charge Remaining: " + m["EstimatedChargeRemaining"]);
                Console.WriteLine("Estimated Run Time: " + m["EstimatedRunTime"]);
                Console.WriteLine("Design Capacity: " + m["DesignCapacity"]);
                Console.WriteLine("Full Charge Capacity: " + m["FullChargeCapacity"]);
                Console.WriteLine("Status: " + m["Status"]);
                Console.WriteLine("Time On Battery: " + m["TimeOnBattery"]);
                Console.WriteLine("Time To Full Charge: " + m["TimeToFullCharge"]);
                Console.WriteLine("-----------------------------------");
            }
        }

        public void time()
        {
            ObjectQuery query = new ObjectQuery("SELECT * FROM Win32_Battery");
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(query);

            ManagementObjectCollection queryCollection = searcher.Get();

            foreach (ManagementObject m in queryCollection)
            {
                uint estimatedRunTime = (uint)m["EstimatedRunTime"];
                TimeSpan timeSpan = TimeSpan.FromSeconds(estimatedRunTime);

                Console.WriteLine($"Estimated Run Time: {timeSpan}");
                Console.WriteLine("-----------------------------------");
            }
        }
        

        public void get2()
        {
            ObjectQuery query = new ObjectQuery("SELECT * FROM Win32_PortableBattery");
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(query);

            ManagementObjectCollection queryCollection = searcher.Get();

            foreach (ManagementObject m in queryCollection)
            {
                Console.WriteLine("Time: " + m["TimeToFullCharge"]);
                Console.WriteLine("Time: " + m["FullChargeCapacity"]);
                Console.WriteLine("Time: " + m["TimeToFullCharge"]);
                Console.WriteLine("Time: " + m["TimeOnBattery"]);
            }
        }
    }
}
