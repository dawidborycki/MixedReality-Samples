using UnityEngine;
using UnityEngine.AI;

public class EthanScript : MonoBehaviour
{
    private Animator animator;
    private AudioSource audioSource;
    private NavMeshAgent navMeshAgent;

    private bool isWalking = false;
    private bool isPathSet = false;

    private void Start()
    {
        TryGetComponent(ref animator, "Animator is unavailable");
        TryGetComponent(ref audioSource, "AudioSource is unavailable");
        TryGetComponent(ref navMeshAgent, "NavMeshAgent is unavailable");
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            UpdateAnimatorParameter();

            UpdateAudioClip();

            SetPath(new Vector3(5, 0, -7));
        }

        CheckIfDestinationWasReached();
    }

    private void TryGetComponent<T>(ref T field, string errorMessage)
    {
        field = GetComponent<T>();

        if (field == null)
        {
            Debug.LogError(errorMessage);
        }
    }

    private void UpdateAnimatorParameter()
    {
        if (animator != null)
        {
            isWalking = !isWalking;

            animator.SetBool("IsWalking", isWalking);
        }
    }

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
}