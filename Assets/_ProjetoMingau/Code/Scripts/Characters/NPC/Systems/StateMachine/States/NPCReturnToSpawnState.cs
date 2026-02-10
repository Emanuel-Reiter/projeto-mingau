using UnityEngine;

public class NPCReturnToSpawnState : NPCBaseState
{
    [Header("State tranisitions")]
    [SerializeField] private NPCIdleState _idleState;
    [SerializeField] private NPCFlinchState _flinchState;
    [SerializeField] private float _spawnDistanceTreshold = 2.0f;
    
    [Header("State params")]
    [SerializeField] private AnimationClip _movementAnim;
    [SerializeField] private float _interpolationTime = 0.05f;

    public override void CheckExitState(NPCStateManager npc)
    {
        if (!npc.Deps.Attributes.IsAlive)
        {
            if (_dieState == null) return;

            npc.SwitchState(_dieState);
            return;
        }

        if (npc.Deps.Attributes.IsPostureBroken)
        {
            npc.SwitchState(_flinchState);
            return;
        }

        float distanceToSpawn = Vector3.Distance(npc.transform.position, npc.Deps.SpawnPosition);
        bool hasReachedSpawn = distanceToSpawn < _spawnDistanceTreshold;

        if(hasReachedSpawn)
        {
            npc.SwitchState(_idleState);
            return;
        }
    }

    public override void EnterState(NPCStateManager npc)
    {
        npc.Deps.NavMeshAgent.SetDestination(npc.Deps.SpawnPosition);

        npc.Deps.Animation.PlayAnimationInterpolated(_movementAnim, _interpolationTime);
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

