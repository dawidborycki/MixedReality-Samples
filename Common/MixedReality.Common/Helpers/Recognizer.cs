#region Using

using MixedReality.Common.Enums;
using MixedReality.Common.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Media.SpeechRecognition;

#endregion

namespace MixedReality.Common.Helpers
{
    public class Recognizer
    {
        #region Fields

        private SpeechRecognizer speechRecognizer = new SpeechRecognizer();

        private Dictionary<VoiceCommand, List<string>> voiceCommands;

        private List<string> availableCommands;

        #endregion

        #region Events

        public event TypedEventHandler<object, VoiceCommandRecognizedEventArgs> VoiceCommandRecognized;

        #endregion

        #region Constructor

        public Recognizer()
        {
            // Prepare voice commands dictionary
            InitializeVoiceCommandsDictionary();

            // Prepare recognition constraints
            availableCommands = voiceCommands.SelectMany(vc => vc.Value).ToList();

            var speechRecognitionConstraints = new SpeechRecognitionListConstraint(
                availableCommands);

            speechRecognizer.Constraints.Add(speechRecognitionConstraints);

            // Attach event handlers
            speechRecognizer.StateChanged += SpeechRecognizer_StateChanged;

            speechRecognizer.ContinuousRecognitionSession.ResultGenerated +=
                ContinuousRecognitionSession_ResultGenerated;
        }

        #endregion        

        #region Methods (Private)

        private void InitializeVoiceCommandsDictionary()
        {
            voiceCommands = new Dictionary<VoiceCommand, List<string>>
            {
                {
                    VoiceCommand.WhatISee,
                    new List<string>()
                    {
                        "Tell me what I see",
                        "What I see",
                        "Describe what I see"
                    }
                },
                {
                    VoiceCommand.LookUp,
                    new List<string>()
                    {
                        "Look this up",
                        "Tell me more",
                        "Search"
                    }
                }
            };
        }

        private void ContinuousRecognitionSession_ResultGenerated(
            SpeechContinuousRecognitionSession sender,
            SpeechContinuousRecognitionResultGeneratedEventArgs args)
        {            
            if (VoiceCommandRecognized != null)
            {
                if (availableCommands.Contains(args.Result.Text))
                {
                    var voiceCommand = voiceCommands.First(
                        c => c.Value.Contains(args.Result.Text)).Key;

                    VoiceCommandRecognized.Invoke(this,
                        new VoiceCommandRecognizedEventArgs(voiceCommand));
                }
            }
        }

        #endregion

        #region Methods (Public)

        public async Task BeginVoiceCommandRecognition()
        {
            await speechRecognizer.CompileConstraintsAsync();

            await speechRecognizer.ContinuousRecognitionSession.StartAsync(
                SpeechContinuousRecognitionMode.PauseOnRecognition);
        }

        private void SpeechRecognizer_StateChanged(SpeechRecognizer sender,
            SpeechRecognizerStateChangedEventArgs args)
        {
            System.Diagnostics.Debug.WriteLine(args.State);
        }

        #endregion
    }
}