#region Using

using HoloToolkit.Unity.InputModule;
using HoloToolkit.Unity.SpatialMapping;
using UnityEngine;
using UnityEngine.XR.WSA.Input;

#endregion

public class ProjectileScript : MonoBehaviour
{
    #region Properties

    public GameObject Ball;

    #endregion

    #region Fields

    private GestureRecognizer doubleTapRecognizer;

    #endregion

    #region Methods

    #region Start and OnDestroy

    private void Start()
    {
        ConfigureAndStartGestureRecognizer();        
    }

    private void OnDestroy()
    {
        ReleaseGestureRecognizer();
    }

    #endregion

    #region Gesture recognizer 

    private void ConfigureAndStartGestureRecognizer()
    {
        doubleTapRecognizer = new GestureRecognizer();

        doubleTapRecognizer.SetRecognizableGestures(GestureSettings.DoubleTap);
        doubleTapRecognizer.Tapped += DoubleTapRecognizer_Tapped;
        doubleTapRecognizer.StartCapturingGestures();
    }

    private void DoubleTapRecognizer_Tapped(TappedEventArgs obj)
    {
        ThrowTheBall(GazeManager.Instance.Ray, 15);
    }

    private void ReleaseGestureRecognizer()
    {
        doubleTapRecognizer.StopCapturingGestures();
        doubleTapRecognizer.Tapped -= DoubleTapRecognizer_Tapped;
    }

    #endregion

    #region Ball

    private void ThrowTheBall(Ray ray, float speed)
    {
        if (Ball != null)
        {
            var ball = Instantiate(Ball,
                transform.position, Quaternion.identity);

            var rigidbody = ball.GetComponent<Rigidbody>();

            if (rigidbody != null)
            {
                rigidbody.velocity = ray.direction * speed;                
            }
        }
    }

    #endregion

    #endregion
}
