using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent), typeof(NPCTargetDetection))]
public class NPCDependencies : MonoBehaviour
{
    public NavMeshAgent NavMeshAgent { get; private set; }
    public NPCTargetDetection TargetDetection { get; private set; }
    public GlobalTimer GlobalTimer { get; private set; }
    public NPCAnimationManager Animation { get; private set; }
    public AttributesManager Attributes { get; private set; }

    public Vector3 SpawnPosition { get; private set; }

    private void Awake()
    {
        NavMeshAgent = GetComponent<NavMeshAgent>();
        TargetDetection = GetComponent<NPCTargetDetection>();
        Animation = GetComponent<NPCAnimationManager>();
        Attributes = GetComponent<AttributesManager>();

        SpawnPosition = transform.position;
    }
}
