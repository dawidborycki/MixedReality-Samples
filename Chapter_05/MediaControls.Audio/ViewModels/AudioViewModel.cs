#region Using

using MixedReality.Common.Helpers;
using MixedReality.Common.ViewModels;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Windows.Media.SpeechRecognition;

#endregion

namespace MediaControls.Audio.ViewModels
{
    public class AudioViewModel : BaseViewModel
    {
        #region Properties

        public Uri Source
        {
            get => source;
            set => SetProperty(ref source, value);
        }        

        public bool IsSpeechRecognitionActive
        {
            get => isSpeechRecognitionActive;
            set => SetProperty(ref isSpeechRecognitionActive, value);
        }

        public Synthesizer Synthesizer { get; private set; } = new Synthesizer();

        public string TextToSpeak { get; set; } = string.Empty;

        public ObservableCollection<string> SpeechRecognitionResults = new ObservableCollection<string>();

        #endregion

        #region Fields

        private Uri source;

        private SpeechRecognizer speechRecognizer = new SpeechRecognizer();
        
        private bool isSpeechRecognitionActive;

        #endregion

        #region Constructor

        public AudioViewModel()
        {
            source = new Uri("ms-appx:///Media/audio.wav");           
        }

        #endregion

        #region Methods (Public)

        public async void Speak()
        {
            await Synthesizer.Speak(TextToSpeak);
        }

        public async Task InitializeSpeechRecognizer()
        {
            await speechRecognizer.CompileConstraintsAsync();

            speechRecognizer.StateChanged += SpeechRecognizer_StateChanged;

            speechRecognizer.ContinuousRecognitionSession.ResultGenerated +=
                ContinuousRecognitionSession_ResultGenerated;            

            speechRecognizer.ContinuousRecognitionSession.Completed +=
                ContinuousRecognitionSession_Completed;
        }

        public async void StartSpeechRecognition()
        {
            try
            {
                await speechRecognizer.ContinuousRecognitionSession.StartAsync();

                IsSpeechRecognitionActive = true;
            }
            catch (Exception ex)
            {
                SpeechRecognitionResults.Add($"Recognizer unavailable: {ex.Message}");

                IsSpeechRecognitionActive = false;
            }
        }
        
        #endregion

        #region Methods (Private)

        private async void ContinuousRecognitionSession_ResultGenerated(
            SpeechContinuousRecognitionSession sender,
            SpeechContinuousRecognitionResultGeneratedEventArgs args)
        {
            var result = $"{args.Result.Text} ({args.Result.Confidence})";
            
            await ThreadHelper.InvokeOnMainThread(() =>
            {
                SpeechRecognitionResults.Add($"Recognition result: {result}");
            });
        }

        private async void SpeechRecognizer_StateChanged(
            SpeechRecognizer sender, 
            SpeechRecognizerStateChangedEventArgs args)
        {
            await ThreadHelper.InvokeOnMainThread(() =>
            {
                SpeechRecognitionResults.Add($"Recognizer state: {args.State}");
            });
        }

        private async void ContinuousRecognitionSession_Completed(
            SpeechContinuousRecognitionSession sender, 
            SpeechContinuousRecognitionCompletedEventArgs args)
        {                        
            await ThreadHelper.InvokeOnMainThread(() =>
            {
                IsSpeechRecognitionActive = false;

                SpeechRecognitionResults.Add($"Recognition completed");
            });
        }

        #endregion
    }
}
