using UnityEngine;

public class NPCReturnToSpawnState : NPCBaseState
{
    [Header("State tranisitions")]
    [SerializeField] private NPCIdleState _idleState;

    [SerializeField] private float _spawnDistanceTreshold = 1.5f;

    public override void CheckExitState(NPCStateManager npc)
    {
        bool hasReachedSpawn = Vector3.Distance(transform.position, npc.Dependencies.SpawnPosition) < _spawnDistanceTreshold;
        Debug.Log(Vector3.Distance(transform.position, npc.Dependencies.SpawnPosition));
        if(hasReachedSpawn)
        {
            npc.SwitchState(_idleState);
        }
    }

    public override void EnterState(NPCStateManager npc)
    {
        npc.Dependencies.NavMeshAgent.SetDestination(npc.Dependencies.SpawnPosition);
    }

    public override void UpdateState(NPCStateManager npc)
    {

    }

    public override void PhysicsUpdateState(NPCStateManager npc)
    {

    }

    public override void ExitState(NPCStateManager npc)
    {

    }
}

