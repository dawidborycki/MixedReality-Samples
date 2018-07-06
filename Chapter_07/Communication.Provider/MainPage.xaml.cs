using Communication.Provider.ViewModels;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Communication.Provider
{
    public sealed partial class MainPage : Page
    {
        private ProviderViewModel viewModel = new ProviderViewModel();

        public MainPage()
        {
            InitializeComponent();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            await viewModel.Initialize();
        }
    }
}
