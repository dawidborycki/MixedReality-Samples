#region Using

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Windows.Media.SpeechSynthesis;
using Windows.UI.Core;
using Windows.UI.Xaml.Controls;

#endregion

namespace MixedReality.Common.Helpers
{
    public class Synthesizer
    {
        #region Properties

        public IReadOnlyList<VoiceInformation> Voices { get; } = SpeechSynthesizer.AllVoices;
        public VoiceInformation SelectedVoice { get; set; } = SpeechSynthesizer.DefaultVoice;        

        #endregion

        #region Fields

        private SpeechSynthesizer speechSynthesizer = new SpeechSynthesizer();
        private MediaElement mediaElement = new MediaElement()
        {
            AutoPlay = true
        };

        private ManualResetEventSlim manualResetEventSlim = new ManualResetEventSlim(true);

        private const int msTimeOut = 5000;

        #endregion
        
        #region Methods (Public)

        public async Task Speak(string text)
        {
            if (!string.IsNullOrEmpty(text))
            {
                // Configure voice
                speechSynthesizer.Voice = SelectedVoice ?? SpeechSynthesizer.DefaultVoice;

                // Synthesize text
                var speechStream = await speechSynthesizer.SynthesizeTextToStreamAsync(text);

                // Set source
                mediaElement.SetSource(speechStream, speechStream.ContentType);                
            }
        }

        #endregion
    }
}