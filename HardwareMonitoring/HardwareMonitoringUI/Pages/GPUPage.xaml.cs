using HardwareMonitoringLibrary;
using Microcharts;
using SkiaSharp;

namespace HardwareMonitoringUI.Pages;

public partial class GPUPage : ContentPage
{
    private int time = 0;
	public GPU GPU { get; set; }
    private ChartEntry[] entries;
    public GPUPage()
	{
		InitializeComponent();
		GPU = new GPU();

        entries = new[]
        {
            new ChartEntry((float)GPU.Load)
            {
                Color = SKColor.Parse("#118708"), // Задайте цвет
                Label = $"{time} сек.",
                ValueLabel = $"{GPU.Load}%"
            }
        };
        gpuChart.Chart = new LineChart() { Entries = entries, ShowYAxisText = true, ShowYAxisLines = true, MaxValue = 100 };

        BindingContext = GPU;
        Task.Run(async () =>
        {
            while (true)
            {
                try
                {
                    var gpuUsage = GPULoad.GetGPUUsage();
                    UpdateGPUInfo(gpuUsage);
                    UpdateChart(gpuUsage);
                }
                catch { }

                await Task.Delay(1000);
            }
        });
    }


    private void UpdateGPUInfo(float gpuUsage)
    {
        Device.BeginInvokeOnMainThread(() => LoadLabel.Text = $"{Math.Round(gpuUsage)}%");
    }


    private void UpdateChart(float gpuUsage)
    {
        time += 1;
        var newEntry = new ChartEntry(gpuUsage)
        {
            Color = SKColor.Parse("#118708"), 
            Label = $"{time} сек.",
            ValueLabel = $"{Math.Round(gpuUsage)}%"
        };

        if (entries.Length > 15)
        {
            entries = entries.Skip(1).ToArray();
        }

        entries = entries.Append(newEntry).ToArray();
        gpuChart.Chart = new LineChart()
        {
            Entries = entries,
            ShowYAxisText = true,
            ShowYAxisLines = true,
            MaxValue = 100,
            IsAnimated = false
        };
    }
}