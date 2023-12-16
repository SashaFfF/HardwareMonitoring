namespace HardwareMonitoringUI
{
    public partial class MainPage : ContentPage
    {
        int count = 0;
        CPUTemperature Temperature;

        public MainPage()
        {
            
            InitializeComponent();
            Temperature = new CPUTemperature();
            BindingContext = Temperature;
        }

    }
}