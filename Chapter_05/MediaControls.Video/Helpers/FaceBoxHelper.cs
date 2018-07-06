#region Using

using MixedReality.Common.ArgumentValidation;
using Windows.Media.FaceAnalysis;
using Windows.UI;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

#endregion

namespace MediaControls.Video.Helpers
{
    public class FaceBoxHelper
    {
        #region Methods (Public)

        public static Rectangle ToRectangle(DetectedFace detectedFace)
        {
            Check.IsNull(detectedFace, "detectedFace");
            var rectangle = new Rectangle()
            {
                Width = detectedFace.FaceBox.Width,
                Height = detectedFace.FaceBox.Height,
                Stroke = new SolidColorBrush(Colors.Orange),
                StrokeThickness = 4
            };

            Translate(rectangle, detectedFace);

            return rectangle;
        }

        #endregion

        #region Methods (Private)

        private static void Translate(Rectangle rectangle, DetectedFace detectedFace)
        {
            var translateTransform = new TranslateTransform()
            {
                X = detectedFace.FaceBox.X,
                Y = detectedFace.FaceBox.Y
            };

            rectangle.RenderTransform = translateTransform;
        }

        #endregion
    }
}
