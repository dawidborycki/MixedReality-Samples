using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.WSA.Input;

public class HitTheCan : MonoBehaviour
{
    public GameObject Ball;

    // Use this for initialization
    void Start()
    {
        AdjustSettings();
    }

    private void Awake()
    {
        InteractionManager.InteractionSourcePressed +=
            InteractionManager_InteractionSourcePressed;
    }

    private void OnDestroy()
    {
        InteractionManager.InteractionSourcePressed -=
            InteractionManager_InteractionSourcePressed;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var ray = GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);

            ThrowTheBall(ray, 10);
        }
    }    

    void AddForceToHitTarget(Ray ray, float forceScaleFactor)
    {
        RaycastHit hitTarget;

        if (Physics.Raycast(ray, out hitTarget))
        {
            var rigidbody = hitTarget.rigidbody;

            if (rigidbody != null)
            {
                rigidbody.AddForce(transform.forward * forceScaleFactor);
            }
        }
    }

    //void FixedUpdate()
    //{
    //    if (Input.GetMouseButton(0))
    //    {
    //        var ray = GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);

    //        AddForceToHitTarget(ray, 10);
    //    }
    //}

    private void ThrowTheBall(Ray ray, float speed)
    {
        if (Ball != null)
        {
            var ball = Instantiate(Ball, transform);

            var rigidbody = ball.GetComponent<Rigidbody>();

            if (rigidbody != null)
            {
                rigidbody.velocity = ray.direction * speed;
            }

            Debug.Log("Throw the ball. Origin: "
                + ray.origin + ", Direction: " + ray.direction);

            Debug.DrawRay(ray.origin, ray.direction, Color.green, 3.0f);
        }
    }

    private void InteractionManager_InteractionSourcePressed(
        InteractionSourcePressedEventArgs obj)
    {
        if (obj.state.anyPressed)
        {
            // Configure ray
            var ray = new Ray()
            {
                origin = GetRayOrigin(obj),

                direction = GetRayDirection(obj)
            };

            // ... and then throw the ball
            ThrowTheBall(ray, 10);
        }
    }

    private Vector3 GetRayOrigin(InteractionSourcePressedEventArgs obj)
    {
        // Ray origin is inferred from the interaction source pose if available.
        // Otherwise the ray origin is set to the camera position
        Vector3 interactionSourcePose;        

        if (obj.state.sourcePose.TryGetPosition(out interactionSourcePose,
            InteractionSourceNode.Pointer))
        {
            return interactionSourcePose;
        }
        else
        {
            return transform.position;
        }
    }

    private Vector3 GetRayDirection(InteractionSourcePressedEventArgs obj)
    {
        if (obj.state.touchpadPressed)
        {
            // Ray direction is set according to the touchpad position
            return Position2DToRayDirection(obj.state.touchpadPosition);
        }
        else if (obj.state.thumbstickPressed)
        {
            // Ray direction is set according to the thumbstick position
            return Position2DToRayDirection(obj.state.thumbstickPosition);
        }
        else
        {
            // Ray direction is set according to the head pose
            //return obj.state.headPose.forward;
            return obj.state.headPose.position;
        }
    }

    private Vector3 Position2DToRayDirection(Vector2 position2D)
    {
        var result = Vector3.forward;

        if (position2D != null)
        {
            result.x = position2D.x;
            result.y = position2D.y;
        }

        return result;
    }

    private void AdjustSettings()
    {
        Debug.Log("XRDevice.model = " + XRDevice.model);

        if (!XRDevice.model.Contains("HoloLens"))
        {
            XRDevice.fovZoomFactor = 2.5f;

            GameObject.Find("Platform").transform.position +=
                new Vector3(0.0f, 1.25f, 0.0f);
        }
    }
}
