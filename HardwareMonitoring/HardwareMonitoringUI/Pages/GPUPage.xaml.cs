using HardwareMonitoringLibrary;

namespace HardwareMonitoringUI.Pages;

public partial class GPUPage : ContentPage
{
	public GPU GPU { get; set; }
	public GPUPage()
	{
		InitializeComponent();
		GPU = new GPU();
		BindingContext = GPU;
	}
}