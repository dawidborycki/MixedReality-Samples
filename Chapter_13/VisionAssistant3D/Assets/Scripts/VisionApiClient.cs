#region Using

using CI.HttpClient;
using HoloToolkit.Unity;
using System;
using System.IO;
using System.Linq;
using UnityEngine;

#endregion

public class VisionApiClient : Singleton<VisionApiClient>
{
    #region Properties

    public string Key = "dc15f728291f45b296dccce4f705a2a5";
    public string Endpoint = "https://westcentralus.api.cognitive.microsoft.com/vision/v1.0";

    #endregion

    #region Fields

    private HttpClient httpClient;

    #endregion

    #region Constants

    private const string subscriptionHeaderKey = "Ocp-Apim-Subscription-Key";
    private const string describeQuery = "/describe?maxCandidates = 1";
    private const string mediaType = "application/octet-stream";

    #endregion

    #region Methods (Public)

    public void SendRequest(Texture2D imageTexture)
    {
        var imageData = ImageConversion.EncodeToPNG(imageTexture);

        var content = new StreamContent(
            new MemoryStream(imageData), mediaType);

        var uri = new Uri(string.Concat(Endpoint, describeQuery));

        httpClient.Post(uri, content, ParseResponse);
    }

    #endregion

    #region Methods (Private)

    protected override void Awake()
    {
        httpClient = new HttpClient();
        httpClient.CustomHeaders.Add(subscriptionHeaderKey, Key);

#if UNITY_EDITOR
    System.Net.ServicePointManager.ServerCertificateValidationCallback
        += (o, certificate, chain, errors) =>
    {
        return true;
    };
#endif
    }

    private void ParseResponse(HttpResponseMessage<string> response)
    {
        Debug.Log(response.StatusCode);
        Debug.Log(response.Data);

        var imageDescription = string.Empty;

        if (!response.IsSuccessStatusCode)
        {
            imageDescription = response.ReasonPhrase;
        }
        else
        {
            var analysisResult = JsonUtility.FromJson<AnalysisResult>(
                response.Data);

            imageDescription = analysisResult.description.
                captions.FirstOrDefault().text;
        }

        BroadcastMessage("OnImageDescriptionGenerated", imageDescription);
    }

    #endregion
}

[Serializable]
public class Caption
{
    public string text;
    public double confidence;
}

[Serializable]
public class Description
{
    public string[] tags;
    public Caption[] captions;
}

[Serializable]
public class Metadata
{
    public int height;
    public int width;
    public string format;
}

public class AnalysisResult
{
    public Description description;
    public string requestId;
    public Metadata metadata;
}

