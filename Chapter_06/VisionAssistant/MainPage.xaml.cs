#region Using

using MixedReality.Common.Helpers;
using VisionAssistant.ViewModels;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

#endregion

namespace VisionAssistant
{
    public sealed partial class MainPage : Page
    {
        private VisionAssistantViewModel viewModel = new VisionAssistantViewModel();

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
