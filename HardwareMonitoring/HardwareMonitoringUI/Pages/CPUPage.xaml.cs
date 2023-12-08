using HardwareMonitoringLibrary;
using System.Xml;

namespace HardwareMonitoringUI.Pages;

public partial class CPUPage : ContentPage
{
    private CPU CPU;
    private bool isUpdating = false;

    public CPUPage()
    {
        InitializeComponent();
        CPU = new CPU();
        BindingContext = CPU;

        // Запускаем таймер при инициализации страницы
        StartTimer();
    }

    private void StartTimer()
    {
        Device.StartTimer(TimeSpan.FromSeconds(5), () =>
        {
            // Проверяем, не выполняется ли уже обновление
            if (!isUpdating)
            {
                // Устанавливаем флаг, чтобы избежать конфликта с другими таймерами
                isUpdating = true;

                // Вызываем асинхронный метод обновления информации
                UpdateDataAsync();

                // Сбрасываем флаг после обновления
                isUpdating = false;
            }

            // Возвращаем true, чтобы таймер продолжил работу
            return true;
        });
    }

    private async void UpdateDataAsync()
    {
        // Асинхронно обновляем данные
        await CPU.UpdateInfoAsync();

        // Обновляем содержимое лейблов напрямую из UI-потока
        Device.BeginInvokeOnMainThread(() =>
        {
            // Обновляем содержимое лейблов
            TemperatureLabel.Text = $"{CPU.Temperature} °C";
            OccupancyPercentageLabel.Text = $"{CPU.OccupancyPercentage}%";
        });
    }

}