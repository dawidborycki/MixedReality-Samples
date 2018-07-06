#region Using

using HoloToolkit.Unity.InputModule;
using UnityEngine;
using UnityEngine.XR.WSA.Input;

#endregion

public class GestureHandler : MonoBehaviour
{
    #region Fields

    private GestureRecognizer manipulationRecognizer;
    private GestureRecognizer navigationRecognizer;

    private Vector3 previousManipulation;
    private float previousScale;

    #endregion

    #region Methods

    #region Start and OnDestroy

    private void Start()
    {
        ConfigureAndStartManipulationRecognizer();

        ConfigureAndStartNavigationRecognizer();
    }

    private void OnDestroy()
    {
        ReleaseManipulationRecognizer();

        ReleaseNavigationRecognizer();
    }

    #endregion

    #region Manipulation

    private void ConfigureAndStartManipulationRecognizer()
    {
        // Instantiate recognizer
        manipulationRecognizer = new GestureRecognizer();

        // Configure recognizable gestures
        manipulationRecognizer.SetRecognizableGestures(
            GestureSettings.ManipulationTranslate);

        // Wire the event handlers
        manipulationRecognizer.ManipulationStarted +=
            GestureRecognizer_ManipulationStarted;

        manipulationRecognizer.ManipulationUpdated +=
            GestureRecognizer_ManipulationUpdated;

        // Start recognizer
        manipulationRecognizer.StartCapturingGestures();
    }

    private void GestureRecognizer_ManipulationStarted(
        ManipulationStartedEventArgs obj)
    {
        previousManipulation = Vector3.zero;
    }

    private void GestureRecognizer_ManipulationUpdated(
        ManipulationUpdatedEventArgs obj)
    {
        if (GazeManager.Instance.HitObject != null)
        {
            UpdatePosition(GazeManager.Instance.HitObject,
                obj.cumulativeDelta);
        }
    }

    private void UpdatePosition(GameObject hitObject,
        Vector3 cumulativeDelta)
    {
        var delta = cumulativeDelta - previousManipulation;

        previousManipulation = cumulativeDelta;

        GazeManager.Instance.HitObject.transform.position += delta;
    }

    #endregion

    #region Navigation

    private void ConfigureAndStartNavigationRecognizer()
    {
        navigationRecognizer = new GestureRecognizer();

        navigationRecognizer.SetRecognizableGestures(
            GestureSettings.NavigationZ);

        navigationRecognizer.NavigationStarted +=
            NavigationRecognizer_NavigationStarted;

        navigationRecognizer.NavigationUpdated +=
            NavigationRecognizer_NavigationUpdated;

        navigationRecognizer.StartCapturingGestures();
    }

    private void NavigationRecognizer_NavigationStarted(
        NavigationStartedEventArgs obj)
    {
        previousScale = 0.0f;
    }

    private void NavigationRecognizer_NavigationUpdated(
        NavigationUpdatedEventArgs obj)
    {
        if (GazeManager.Instance.HitObject != null)
        {
            UpdateScale(GazeManager.Instance.HitObject,
                obj.normalizedOffset);
        }
    }

    private void UpdateScale(GameObject hitObject,
        Vector3 normalizedOffset)
    {
        var scale = normalizedOffset.z - previousScale;

        previousScale = normalizedOffset.z;

        hitObject.transform.localScale += Vector3.one * scale;
    }

    #endregion

    #region Cleaning

    private void ReleaseManipulationRecognizer()
    {
        manipulationRecognizer.StopCapturingGestures();

        manipulationRecognizer.ManipulationStarted -=
            GestureRecognizer_ManipulationStarted;

        manipulationRecognizer.ManipulationUpdated -=
            GestureRecognizer_ManipulationUpdated;
    }

    private void ReleaseNavigationRecognizer()
    {
        navigationRecognizer.StopCapturingGestures();

        navigationRecognizer.NavigationStarted -=
            NavigationRecognizer_NavigationStarted;

        navigationRecognizer.NavigationUpdated -=
            NavigationRecognizer_NavigationUpdated;
    }

    #endregion

    #endregion
}
