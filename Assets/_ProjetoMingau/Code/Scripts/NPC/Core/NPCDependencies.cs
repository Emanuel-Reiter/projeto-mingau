using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent), typeof(NPCTargetDetection))]
public class NPCDependencies : MonoBehaviour
{
    public NavMeshAgent NavMeshAgent { get; private set; }
    public NPCTargetDetection TargetDetection { get; private set; }
    public TimerSingleton GlobalTimer { get; private set; }
    public NPCAnimationManager Animation { get; private set; }

    public Vector3 SpawnPosition { get; private set; }

    private void Awake()
    {
        try
        {
            NavMeshAgent = GetComponent<NavMeshAgent>();
            TargetDetection = GetComponent<NPCTargetDetection>();
            GlobalTimer = GameObject.FindGameObjectWithTag("GlobalTimer").GetComponent<TimerSingleton>();
            Animation = GetComponent<NPCAnimationManager>();

            SpawnPosition = transform.position;
        }
        catch
        {
            Debug.LogError("Failed to load NPC dependencies");
        }
    }
}
