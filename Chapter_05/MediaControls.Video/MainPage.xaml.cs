using MediaControls.Video.ViewModels;
using Windows.UI.Xaml.Controls;

namespace MediaControls.Video
{
    public sealed partial class MainPage : Page
    {
        private VideoViewModel viewModel = new VideoViewModel();

        public MainPage()
        {
            InitializeComponent();            
        }        
    }
}
