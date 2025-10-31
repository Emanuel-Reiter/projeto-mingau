using UnityEngine;

public class NPCIdleState : NPCBaseState
{
    [Header("State tranisitions")]
    [SerializeField] private NPCTargetChaseState _targetChaseState;

    public override void CheckExitState(NPCStateManager npc)
    {
        if (!IsComplete) return;

        npc.SwitchState(_targetChaseState);
    }

    public override void EnterState(NPCStateManager npc)
    {
        
    }

    public override void UpdateState(NPCStateManager npc)
    {
        bool haveTarget = npc.Dependencies.TargetDetection.HaveTarget();
        IsComplete = haveTarget && CheckCompletedExitTime();
    }

    public override void PhysicsUpdateState(NPCStateManager npc)
    {

    }

    public override void ExitState(NPCStateManager npc)
    {

    }
}
