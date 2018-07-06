using Windows.System.Profile;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace UWP.Basics
{
    public sealed partial class MainPage : Page
    {
        private PersonViewModel ViewModel = new PersonViewModel();

        public MainPage()
        {
            InitializeComponent();

            if (AnalyticsInfo.VersionInfo.DeviceFamily == "Windows.Holographic")
            {
                Button_Click(this, null);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.FirstName = "Nick";
            ViewModel.LastName = "Wilde";
            ViewModel.Age = 27;
        }        
    }
}
