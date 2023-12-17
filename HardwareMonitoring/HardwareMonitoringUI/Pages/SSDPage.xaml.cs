using HardwareMonitoringLibrary;
using HardwareMonitoringLibrary.Drive;
using Microcharts;
using SkiaSharp;

namespace HardwareMonitoringUI.Pages;

public partial class SSDPage : ContentPage
{
	public SSD Drive { get; set; }
    private ChartEntry[] entries;
    public SSDPage()
	{
		InitializeComponent();
        Drive = new SSD();
        makeChart();
        BindingContext = this;
    }

    public void makeChart()
    {
        int diskCount = Drive.Disks.Count;

        if(diskCount == 2)
        {
            float free = (float)Drive.Disks[0].FreePercentage;
            float load = (float)Drive.Disks[0].OccupancyPercentage;
            entries = new[]
            {
                new ChartEntry(free)
                {
                    Color = SKColor.Parse("#b6b4d1"),
                    Label = "Свободно",
                    ValueLabel = $"{free}%"
                },
                new ChartEntry(load)
                {
                    Color = SKColor.Parse("#54517d"),
                    Label = "Занято",
                    ValueLabel = $"{load}%"
                }
            };
            CChart.Chart = new DonutChart() { Entries = entries, MaxValue = 100, IsAnimated = false };
            //, ShowYAxisText = true, ShowYAxisLines = true, MaxValue = 100 

            free = (float)Drive.Disks[1].FreePercentage;
            load = (float)Drive.Disks[1].OccupancyPercentage;
            entries = new[]
            {
                new ChartEntry(free)
                {
                    Color = SKColor.Parse("#b6b4d1"),
                    Label = "Свободно",
                    ValueLabel = $"{free}%"
                },
                new ChartEntry(load)
                {
                    Color = SKColor.Parse("#54517d"),
                    Label = "Занято",
                    ValueLabel = $"{load}%"
                }
            };
            DChart.Chart = new DonutChart() { Entries = entries, MaxValue = 100, IsAnimated = false };
        }


    }
}
