#region Using

using System;
using System.Linq;
using UnityEngine;
using UnityEngine.XR.WSA.WebCam;

#endregion

public class VisionScript : MonoBehaviour
{
    #region Properties

    public TextMesh InfoPanel;
    public Renderer ImageRenderer;

    #endregion

    #region Fields

    private PhotoCapture photoCapture;
    private CameraParameters cameraParameters;
    private bool isPhotoCaptureReady;

    #endregion

    #region Methods

    #region Start and OnDestroy

    private void Start()
    {
#if !WINDOWS_UWP
        // Reset the info panel
        InfoPanel.text = string.Empty;

        // Initialize photo capture
        PhotoCapture.CreateAsync(false, OnPhotoCaptureCreated);
#endif        
    }

    #endregion

    #region PhotoCapture

    private void OnPhotoCaptureCreated(PhotoCapture captureObject)
    {
        photoCapture = captureObject;

        // Pick the best resolution available
        var resolution = PhotoCapture.SupportedResolutions.OrderByDescending(
            r => r.width * r.height).First();

        cameraParameters = new CameraParameters(WebCamMode.VideoMode)
        {
            hologramOpacity = 0.0f,
            cameraResolutionHeight = resolution.height,
            cameraResolutionWidth = resolution.width,
            pixelFormat = CapturePixelFormat.BGRA32
        };

        photoCapture.StartPhotoModeAsync(cameraParameters, OnPhotoModeStarted);
    }

    private void OnPhotoModeStarted(PhotoCapture.PhotoCaptureResult result)
    {
        isPhotoCaptureReady = result.success;

        if (!isPhotoCaptureReady)
        {
            Debug.Log("Photo capture initialization failed");
        }
    }

    private void TakePhoto()
    {
        if (isPhotoCaptureReady)
        {
            photoCapture.TakePhotoAsync(OnPhotoCaptured);
        }
        else
        {
            Debug.Log("Photo capture is unavailable");
        }
    }

    private void OnPhotoCaptured(PhotoCapture.PhotoCaptureResult result,
        PhotoCaptureFrame photoCaptureFrame)
    {
        if (result.success)
        {
            // Display image
            var texture2D = new Texture2D(
                cameraParameters.cameraResolutionWidth,
                cameraParameters.cameraResolutionHeight);

            photoCaptureFrame.UploadImageDataToTexture(texture2D);

            ImageRenderer.material.SetTexture("_MainTex", texture2D);

            // Send image for analysis
            VisionApiClient.Instance.SendRequest(texture2D);
        }
        else
        {
            Debug.Log("Photo capture failed");
        }
    }

    #endregion

    #region Events

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            TakePhoto();
        }
    }

    #endregion

    #region Message handlers

    private void OnImageDescriptionGenerated(
        string imageDescription)
    {
        InfoPanel.text = imageDescription;
    }

    #endregion

    #endregion
}
