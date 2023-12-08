using HardwareMonitoringLibrary;
using System.Xml;

namespace HardwareMonitoringUI.Pages;

public partial class RAMPage : ContentPage
{
    private RAM RAM;
    public RAMPage()
    {
        InitializeComponent();
        RAM = new RAM();
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
        });
    }
}