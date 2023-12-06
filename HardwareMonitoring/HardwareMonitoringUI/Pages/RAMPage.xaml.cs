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
		BindingContext= RAM;
	}
}