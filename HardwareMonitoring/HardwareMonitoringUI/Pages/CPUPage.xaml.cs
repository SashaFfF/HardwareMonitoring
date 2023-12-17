using HardwareMonitoringLibrary;
using System.Xml;
using SkiaSharp;
using Microcharts;
using HardwareMonitoringUI.Converter;

namespace HardwareMonitoringUI.Pages;

public partial class CPUPage : ContentPage
{

    private int time = 0;
    private CPU CPU;
    private bool isUpdating = false;
    private ChartEntry[] entries;

    public CPUPage()
    {
        InitializeComponent();
        CPU = new CPU();
        entries = new[]
        {
            new ChartEntry(CPU.OccupancyPercentage)
            {
                Color = SKColor.Parse("#FF1493"), // Задайте цвет
                Label = $"{time} сек.",
                ValueLabel = $"{CPU.OccupancyPercentage}%"
            }
        };
        cpuChart.Chart = new LineChart() { Entries = entries, ShowYAxisText = true, ShowYAxisLines = true, MaxValue = 100 };
        BindingContext = CPU;
        StartTimer();
    }

    private void StartTimer()
    {
        Device.StartTimer(TimeSpan.FromSeconds(5), () =>
        {
            //if (!isUpdating)
            //{
            //    // Устанавливаем флаг, чтобы избежать конфликта с другими таймерами
            //    isUpdating = true;
                UpdateDataAsync();
            //    isUpdating = false;
            //}

            //  true, чтобы таймер продолжил работу
            return true;
        });
    }

    private async void UpdateDataAsync()
    {
        await CPU.UpdateInfoAsync();

        Device.BeginInvokeOnMainThread(() =>
        {
            PercentageConverter percentageConverter = new PercentageConverter();
            if (CPU.OccupancyPercentage == 0) { CPU.OccupancyPercentage = percentageConverter.NullConvert(); }
            //TemperatureLabel.Text = $"{CPU1.Temperature} °C";
            OccupancyPercentageLabel.Text = $"{CPU.OccupancyPercentage}%";

            UpdateChart();
        });
    }

    private void UpdateChart()
    {
        // Получите текущее значение загрузки процессора
        float occupancyPercentage = CPU.OccupancyPercentage;

        time += 5;
        // Создайте новую точку для графика
        var newEntry = new ChartEntry(occupancyPercentage)
        {
            Color = SKColor.Parse("#FF1493"), // Задайте цвет
            Label = $"{time} сек.",
            ValueLabel = $"{occupancyPercentage}%"
        };

        if (entries.Length > 26)
        {
            entries = entries.Skip(1).ToArray();
        }

        // Добавьте новую точку в массив точек графика
        entries = entries.Append(newEntry).ToArray();

        // Создайте новый график с обновленными данными
        cpuChart.Chart = new LineChart()
        {
            Entries = entries,
            ShowYAxisText = true,
            ShowYAxisLines = true,
            MaxValue = 100
        };
    }

}