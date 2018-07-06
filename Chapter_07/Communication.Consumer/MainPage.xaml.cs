#region Using

using Communication.Consumer.ViewModels;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

#endregion

namespace Communication.Consumer
{
    public sealed partial class MainPage : Page
    {
        #region Fields

        private ConsumerViewModel viewModel = new ConsumerViewModel();

        #endregion

        #region Constructor

        public MainPage()
        {
            InitializeComponent();
        }

        #endregion

        #region Methods

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            Window.Current.SizeChanged += Current_SizeChanged;

            viewModel.Update(Window.Current.Bounds);

            await viewModel.Initialize();
        }

        private void Current_SizeChanged(object sender, WindowSizeChangedEventArgs e)
        {
            viewModel.Update(Window.Current.Bounds);
        }

        #endregion
    }
}
