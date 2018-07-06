#region Using

using UnityEngine;
using UnityEngine.AI;

#endregion

public class EthanScript : Singleton<EthanScript>
{
    #region Fields

    private Animator animator;
    private AudioSource audioSource;
    private NavMeshAgent navMeshAgent;

    private bool isWalking = false;
    private bool isPathSet = false;

    #endregion

    #region Properties

    public bool IsWalking
    {
        get { return isWalking; }
    }

    #endregion

    #region Methods (Private)

    #region Start and Update

    private void Start()
    {
        TryGetComponent(ref animator, "Animator is unavailable");
        TryGetComponent(ref audioSource, "AudioSource is unavailable");
        TryGetComponent(ref navMeshAgent, "NavMeshAgent is unavailable");
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            UpdateEthanState();            
        }

        CheckIfDestinationWasReached();
    }

    #endregion

    #region Helpers

    private void TryGetComponent<T>(ref T field, string errorMessage)
    {
        field = GetComponent<T>();

        if (field == null)
        {
            Debug.LogError(errorMessage);
        }
    }

    private void UpdateEthanState()
    {
        UpdateAnimatorParameter();

        UpdateAudioClip();

        SetPath(new Vector3(5, 0, -7));
    }

    #endregion

    #region Animator

    private void UpdateAnimatorParameter()
    {
        if (animator != null)
        {
            isWalking = !isWalking;

            animator.SetBool("IsWalking", isWalking);
        }        
    }    

    #endregion

    #region Audio clip

    private void UpdateAudioClip()
    {
        if (audioSource != null)
        {
            if (isWalking)
            {
                audioSource.Play();
            }
            else
            {
                audioSource.Stop();
            }
        }
    }

    #endregion

    #region Navigation

    private void SetPath(Vector3 destination)
    {
        if (navMeshAgent != null && !isPathSet)
        {
            var navMeshPath = new NavMeshPath();

            if (NavMesh.CalculatePath(transform.position, destination, NavMesh.AllAreas, navMeshPath))
            {
                navMeshAgent.SetPath(navMeshPath);

                isPathSet = true;
            }
            else
            {
                Debug.Log("The path cannot be determined");
            }
        }
        else
        {
            navMeshAgent.isStopped = !isWalking;
        }
    }

    private void CheckIfDestinationWasReached()
    {
        if (navMeshAgent != null)
        {
            if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance && isWalking)
            {
                UpdateAnimatorParameter();
                UpdateAudioClip();
            }
        }
    }

    private void DebugPath(NavMeshPath navMeshPath)
    {
        for (var i = 0; i < navMeshPath.corners.Length - 1; i++)
        {
            Debug.DrawLine(navMeshPath.corners[i],
                navMeshPath.corners[i + 1], Color.red);
        }
    }

    #endregion

    #region Gaze

    private void GazeEntered()
    {
        //UpdateEthanState();
    }

    #endregion    

    #endregion
}