#region Using

using MixedReality.Common.Enums;
using System;

#endregion

namespace MixedReality.Common.Events
{
    public class VoiceCommandRecognizedEventArgs : EventArgs
    {
        #region Properties

        public VoiceCommand VoiceCommand { get; private set; }

        #endregion

        #region Constructor

        public VoiceCommandRecognizedEventArgs(VoiceCommand voiceCommand)
        {
            VoiceCommand = voiceCommand;
        }

        #endregion
    }
}