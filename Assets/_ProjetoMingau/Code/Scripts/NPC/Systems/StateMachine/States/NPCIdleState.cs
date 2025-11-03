using UnityEngine;

public class NPCIdleState : NPCBaseState
{
    [Header("State tranisitions")]
    [SerializeField] private NPCTargetChaseState _targetChaseState;

    [Header("State params")]
    [SerializeField] private AnimationClip _idleAnim;
    [SerializeField] private float _interpolationTime = 0.1f;

    public override void CheckExitState(NPCStateManager npc)
    {
        if (!IsComplete) return;

        npc.SwitchState(_targetChaseState);
    }

    public override void EnterState(NPCStateManager npc)
    {
        npc.Dependencies.Animation.PlayAnimationInterpolated(_idleAnim, _interpolationTime);
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
