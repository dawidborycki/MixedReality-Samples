#region Using

using MediaControls.Audio.ViewModels;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

#endregion

namespace MediaControls.Audio
{
    public sealed partial class MainPage : Page
    {
        private AudioViewModel viewModel = new AudioViewModel();

        public MainPage()
        {
            InitializeComponent();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            await viewModel.InitializeSpeechRecognizer();
        }
    }
}