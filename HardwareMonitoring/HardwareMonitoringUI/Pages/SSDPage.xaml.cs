using HardwareMonitoringLibrary.Drive;

namespace HardwareMonitoringUI.Pages;

public partial class SSDPage : ContentPage
{
	public Drive Drive { get; set; }
	public SSDPage()
	{
		InitializeComponent();
        Drive = new Drive();
        BindingContext = this;
    }
}
