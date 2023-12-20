using HardwareMonitoringLibrary;
using HardwareMonitoringUI.Converters;
using Microsoft.Maui.Devices;
using Battery = Microsoft.Maui.Devices.Battery;

namespace HardwareMonitoringUI.Pages;

public partial class BatteryPage : ContentPage
{
    public double ChargeLevel { get; set; }
    public int ChargePercentage { get; set; }
    public string Status { get; set; }
    public string TimeLeft { get; set; }
    public EnergySaverStatus PowerSaveMode { get; set; }

    public TimeForecast2 timeForecast { get; set; }
   
    public BatteryPage()
	{
		InitializeComponent();

        timeForecast = new TimeForecast2();

        UpdateBatteryInfo();
        BindingContext = this;


        Device.StartTimer(TimeSpan.FromSeconds(5), () =>
        {
            UpdateBatteryInfo();
            return true; //  true, чтобы таймер продолжил работу
        });
    }

    private async void UpdateBatteryInfo()
    {
        ChargeLevel = Battery.ChargeLevel;
        ChargePercentage = Convert.ToInt32(ChargeLevel * 100);
        BatteryChargePercentage.Text = ChargePercentage.ToString() + "%";
        Status = Battery.PowerSource.ToString();
        if (Status.Equals("Battery"))
        {
            BatteryStatus.Text = "Режим питания: питание от аккумулятора";
        }
        else
        {
            BatteryStatus.Text = "Режим питания: питание от сети";
        }

        if (Status.Equals("Battery"))
        {
            await timeForecast.RAM.UpdateMemoryInfoAsync();
            TimeLeft = timeForecast.UpdateTime(ChargePercentage);
            BatteryTime.Text = "Прогноз времени работы: " + TimeLeft.ToString();
        }
        else
        {
            BatteryTime.Text = String.Empty;
        }

        PowerSaveMode = Battery.EnergySaverStatus;
        BatteryEnergySaverStatus.Text = PowerSaveMode.ToString();
        if (PowerSaveMode.ToString().Equals("On"))
        {
            BatteryEnergySaverStatus.Text = "Режим экономии заряда включен";
        }
        else
        {
            BatteryEnergySaverStatus.Text = "Режим экономии заряда выключен";
        }

        if (!Status.Equals("Battery"))
        {
            image.Source = "ch.jpg";
        }
        else if(ChargePercentage >= 90)
        {
            image.Source = "p100.jpg";
        }
        else if(ChargePercentage <=89 && ChargePercentage > 70)
        {
            image.Source = "p85.jpg";
        }
        else if (ChargePercentage <= 70 && ChargePercentage > 50)
        {
            image.Source = "p70.jpg";
        }
        else if (ChargePercentage <= 50 && ChargePercentage > 35)
        {
            image.Source = "p50.jpg";
        }
        else if (ChargePercentage <= 35 && ChargePercentage > 15)
        {
            image.Source = "p35.jpg";
        }
        else
        {
            image.Source = "p20.jpg";
        }
    }

}