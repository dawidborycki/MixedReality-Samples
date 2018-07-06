using UnityEngine;

public class GazeHandler : Singleton<GazeHandler>
{
    #region Properties

    [Tooltip("Gaze cursor")]
    public GameObject Cursor;

    [Tooltip("An indicator, which becomes active, when user is looking at the collider")]
    public GameObject CursorDot;

    [Tooltip("Indicates the time (in seconds) after which a message will be sent to the gazed object")]
    public int GazeTriggerTime = 5;    
    
    #endregion

    #region Fields

    private float gazeTime;
    private float maxCursorDistance = 5f;

    private GameObject focusedObject;
    
    #endregion

    #region Methods (Public)

    public void SendMessageToFocusedObject(string methodName)
    {
        if (focusedObject != null)
        {
            focusedObject.SendMessage(methodName, 
                SendMessageOptions.DontRequireReceiver);    
        }

        ResetGazeTimer();
    }

    #endregion

    #region Methods (Private)

    private void Start()
    {
        UpdateCursorDot(false);
        ResetGazeTimer();
    }

    private void Update()
    {
        var gazeRay = CreateGazeRayAndUpdateCursorPosition();

        RaycastHit hitInfo;
        if (Physics.Raycast(gazeRay, out hitInfo))
        {
            UpdateCursorDot(true);

            CorrectCursorPosition(hitInfo);

            UpdateFocusedObject(hitInfo.collider.gameObject);

            if (CheckGazeTime())
            {
                SendMessageToFocusedObject("GazeEntered");
            }
        }
        else
        {
            UpdateCursorDot(false);

            SendMessageToFocusedObject("GazeExited");

            UpdateFocusedObject(null);
        }
    }

    private Ray CreateGazeRayAndUpdateCursorPosition()
    {
        // Get gaze origin and direction
        var gazeOrigin = transform.position;
        var gazeDirection = transform.forward;

        // Create the gaze ray
        var gazeRay = new Ray(gazeOrigin, gazeDirection);

        // Position the cursor along the gaze ray
        Cursor.transform.position = gazeRay.GetPoint(maxCursorDistance);        

        // Return the gaze ray to be used for ray casting
        return gazeRay;
    }

    private void UpdateCursorDot(bool isActive)
    {        
        CursorDot.SetActive(isActive);     
    }

    private void CorrectCursorPosition(RaycastHit hitInfo)
    {        
        if (hitInfo.point.z <= Cursor.transform.position.z)
        {
            Cursor.transform.position = hitInfo.point;
        }        
    }    

    private void ResetGazeTimer()
    {
        gazeTime = 0.0f;        
    }

    private bool CheckGazeTime()
    {
        gazeTime += Time.deltaTime;

        return gazeTime >= GazeTriggerTime;
    }

    private void UpdateFocusedObject(GameObject gameObject)
    {
    if(gameObject != focusedObject)
    {
        ResetGazeTimer();
            
        focusedObject = gameObject;
    }                
    }

    #endregion
}
