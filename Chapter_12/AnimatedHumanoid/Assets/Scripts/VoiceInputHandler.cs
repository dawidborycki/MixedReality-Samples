#region Using

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Windows.Speech;

#endregion

public class VoiceInputHandler : MonoBehaviour
{
    #region Fields

    private KeywordRecognizer keywordRecognizer;
    private Dictionary<string, Action> voiceCommands;

    #endregion

    #region Methods

    private void Start()
    {
        InitializeVoiceCommandsDictionary();

        ConfigureAndStartRecognizer();
    }

    private void InitializeVoiceCommandsDictionary()
    {
        voiceCommands = new Dictionary<string, Action>();

        voiceCommands.Add("Walk", () => WalkCommandHandler());
        voiceCommands.Add("Stop", () => StopCommandHanlder());
    }

    private void StopCommandHanlder()
    {
        if (EthanScript.Instance.IsWalking)
        {
            GazeHandler.Instance.
                SendMessageToFocusedObject("UpdateEthanState");
        }
    }

    private void WalkCommandHandler()
    {
        if (!EthanScript.Instance.IsWalking)
        {
            GazeHandler.Instance.
                SendMessageToFocusedObject("UpdateEthanState");
        }
    }

    private void ConfigureAndStartRecognizer()
    {
        keywordRecognizer = new KeywordRecognizer(
            voiceCommands.Keys.ToArray(), ConfidenceLevel.Low);

        keywordRecognizer.OnPhraseRecognized +=
            KeywordRecognizer_OnPhraseRecognized;

        keywordRecognizer.Start();
    }

    private void KeywordRecognizer_OnPhraseRecognized(
        PhraseRecognizedEventArgs args)
    {
        Action voiceCommandHandler;

        if (voiceCommands.TryGetValue(args.text,
            out voiceCommandHandler))
        {
            voiceCommandHandler.Invoke();
        }
    }

    #endregion
}