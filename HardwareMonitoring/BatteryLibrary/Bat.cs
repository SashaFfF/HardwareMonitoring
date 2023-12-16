//using System;
//using Microsoft.Maui.Essentials;
//using System.Windows.Forms;

//namespace BatteryLibrary
//{
//    public class Bat
//    {
//        public float BatteryPercent { get; private set; }
//        public bool IsCharging { get; private set; }
//        public string TimeRemaining { get; private set; }
//        public string TimeToFullCharge { get; private set; }

//        public Bat()
//        {
//            UpdateBatteryInfo();
//        }

//        public void UpdateBatteryInfo()
//        {
//            var batteryInfo = Battery.EnergySaverStatus;

//            BatteryPercent = batteryInfo.RemainingChargePercent;
//            IsCharging = batteryInfo.PowerState == PowerState.Charging;

//            if (IsCharging)
//            {
//                TimeToFullCharge = GetTimeToFullCharge(batteryInfo);
//            }
//            else
//            {
//                TimeRemaining = GetTimeRemaining(batteryInfo);
//                TimeToFullCharge = null;
//            }
//        }

//        private string GetTimeRemaining(BatteryInfo batteryInfo)
//        {
//            TimeSpan timeSpan = TimeSpan.FromSeconds(batteryInfo.RemainingDischargeTime.TotalSeconds);
//            return timeSpan.ToString(@"hh\:mm\:ss");
//        }

//        private string GetTimeToFullCharge(BatteryInfo batteryInfo)
//        {
//            if (batteryInfo.PowerState == PowerState.Charging)
//            {
//                TimeSpan timeSpan = TimeSpan.FromSeconds(batteryInfo.RemainingChargeTime.TotalSeconds);
//                return timeSpan.ToString(@"hh\:mm\:ss");
//            }

//            return null;
//        }
//    }
//}
