using HardwareMonitoringLibrary;
using System.Xml;

namespace HardwareMonitoringUI.Pages;

public partial class CPUPage : ContentPage
{
    private CPU CPU;
    public CPUPage()
    {
        InitializeComponent();
        CPU = new CPU();
        BindingContext = CPU;
    }

}