#region Using

using UnityEngine;
using UnityEngine.AI;

#endregion

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Animator))]
public class Patrolling : MonoBehaviour {

    #region Fields

    private NavMeshAgent navMeshAgent;
    private Animator animator;
    private MeshRenderer referencePlaneRenderer;
    private LineRenderer targetIndicator;

    private bool isPatrolling;
    private int currentDestinationIndex = 0;

    #endregion

    #region Methods

    #region Helpers

    private void ObtainReferencesToRequiredComponents()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        referencePlaneRenderer = GameObject.
            Find("ReferencePlane").GetComponent<MeshRenderer>();
        targetIndicator = GameObject.
            Find("TargetIndicator").GetComponent<LineRenderer>();
    }

    private void ConfigureAgent()
    {
        navMeshAgent.speed = 0.05f;
        navMeshAgent.angularSpeed = 300.0f;
        navMeshAgent.stoppingDistance = 0.01f;        
        navMeshAgent.autoBraking = false;
    }

    private void UpdatePatrollingStatus(bool isPatrolling)
    {
        this.isPatrolling = isPatrolling;

        animator.SetBool("IsWalking", isPatrolling);

        navMeshAgent.isStopped = !isPatrolling;

        UpdateAgentDestination();
    }

    private void UpdateAgentDestination()
    {
        if (referencePlaneRenderer != null)
        {
            var destinations = new Vector3[]
            {
                referencePlaneRenderer.bounds.min,
                referencePlaneRenderer.bounds.max
            };

            navMeshAgent.destination = destinations[currentDestinationIndex];

            currentDestinationIndex = (currentDestinationIndex + 1) % destinations.Length;
        }
    }

    private void IndicateDestination()
    {
        if (targetIndicator != null)
        {
            targetIndicator.SetPositions(new Vector3[]
            {
                navMeshAgent.transform.position,
                navMeshAgent.destination
            });
        }
    }

    #endregion

    private void Start()
    {
        ObtainReferencesToRequiredComponents();

        ConfigureAgent();
    }

    private void Update()
    {
        if (isPatrolling)
        {
            if (!navMeshAgent.pathPending 
                && navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
            {
                UpdateAgentDestination();
            }

            IndicateDestination();
        }        
    }    

    #endregion
}
