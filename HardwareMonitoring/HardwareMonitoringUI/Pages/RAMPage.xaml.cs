using HardwareMonitoringLibrary;
using Microcharts;
using SkiaSharp;
using System.Xml;

namespace HardwareMonitoringUI.Pages;

public partial class RAMPage : ContentPage
{
    private int time = 0;
    private RAM RAM;
    private ChartEntry[] entries;
    public RAMPage()
    {
        InitializeComponent();
        RAM = new RAM();
        entries = new[]
        {
            new ChartEntry((float)RAM.MemoryLoadPercentage)
            {
                Color = SKColor.Parse("#4b3efa"),
                ValueLabel = $"{RAM.MemoryLoadPercentage}%",
                Label=$"{time} сек."
            }
        };
        cpuChart.Chart = new LineChart() { Entries = entries, ShowYAxisText = true, ShowYAxisLines = true, MaxValue = 100 };
        BindingContext = RAM;

        // Запускаем таймер при инициализации страницы
        StartTimer();
    }

    private void StartTimer()
    {
        Device.StartTimer(TimeSpan.FromSeconds(5), () =>
        {
            // Вызываем асинхронный метод обновления информации
            UpdateMemoryInfoAsync();

            // Возвращаем true, чтобы таймер продолжил работу
            return true;
        });
    }

    private async void UpdateMemoryInfoAsync()
    {
        // Вызываем асинхронный метод обновления информации
        await RAM.UpdateMemoryInfoAsync();

        // Обновляем содержимое лейблов напрямую из UI-потока
        Device.BeginInvokeOnMainThread(() =>
        {
            // Обновляем содержимое лейблов
            FreeMemoryLabel.Text = $"{RAM.FreeMemory} ГБ";
            LoadedMemoryLabel.Text = $"{RAM.LoadedMemory} ГБ";
            MemoryLoadPercentageLabel.Text = $"{RAM.MemoryLoadPercentage}%";

            UpdateChart();
        });


    }

    private void UpdateChart()
    {
        float occupancyPercentage = (float)RAM.MemoryLoadPercentage;

        time += 5;
        // новая точку для графика
        var newEntry = new ChartEntry(occupancyPercentage)
        {
            Color = SKColor.Parse("#4b3efa"), 
            ValueLabel = $"{occupancyPercentage}%",
            Label= $"{time} сек."
        };

        if (entries.Length > 10)
        {
            entries = entries.Skip(1).ToArray();
        }

        entries = entries.Append(newEntry).ToArray();
        cpuChart.Chart = new LineChart()
        {
            Entries = entries,
            ShowYAxisText = true,
            ShowYAxisLines = true,
            MaxValue = 100
        };
    }
}