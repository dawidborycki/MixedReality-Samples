#region Using

using MixedReality.Common.ArgumentValidation;
using System.Numerics;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Composition;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Hosting;

#endregion

namespace HelloWorld
{
    public sealed partial class MainPage : Page
    {
        #region Constructor

        public MainPage()
        {
            InitializeComponent();

            TextBlockMixedReality.LayoutUpdated += TextBlockMixedReality_LayoutUpdated;            
        }

        #endregion

        #region Methods (Private)

        private void TextBlockMixedReality_LayoutUpdated(
            object sender, object e)
        {            
            // Get compositor and create sprite visual
            var compositor = ElementCompositionPreview.
                GetElementVisual(MainGrid).Compositor;
            var visual = compositor.CreateSpriteVisual();

            // Adjust visual size to text block size
            visual.Size = TextBlockMixedReality.DesiredSize.ToVector2();

            // Create drop shadow
            visual.Shadow = CreateDropShadowForTextBlock(
                compositor, TextBlockMixedReality);

            // Add visual to the composition preview
            ElementCompositionPreview.SetElementChildVisual(
                MainGrid, visual);
        }

        private DropShadow CreateDropShadowForTextBlock(
            Compositor compositor, TextBlock textBlock)
        {
            // Check arguments
            Check.IsNull(compositor, "Compositor");
            Check.IsNull(textBlock, "TextBlock");

            var shadow = compositor.CreateDropShadow();

            // Configure shadow
            shadow.Color = Colors.DarkOrange;
            shadow.Offset = GetShadowOffset(textBlock);
            shadow.Mask = TextBlockMixedReality.GetAlphaMask();

            return shadow;
        }

        private Vector3 GetShadowOffset(TextBlock textBlock)
        {
            // Z offset
            const float zOffset = -100.0f;

            // Calculate TextBlock origin
            var transform = textBlock.TransformToVisual(
                Window.Current.Content);
            var origin = transform.TransformPoint(new Point());

            // Return the resulting offset
            return new Vector3((float)origin.X, 
                (float)origin.Y, zOffset);
        }

        #endregion
    }
}
