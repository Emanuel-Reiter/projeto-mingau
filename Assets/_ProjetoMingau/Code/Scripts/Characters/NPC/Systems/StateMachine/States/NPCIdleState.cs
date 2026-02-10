using UnityEngine;

public class NPCIdleState : NPCBaseState
{
    [Header("State tranisitions")]
    [SerializeField] private NPCTargetChaseState _targetChaseState;
    [SerializeField] private NPCFlinchState _flinchState;

    [Header("State params")]
    [SerializeField] private AnimationClip _idleAnim;
    [SerializeField] private float _interpolationTime = 0.1f;

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

        if (!IsComplete) return;

        npc.SwitchState(_targetChaseState);
    }

    public override void EnterState(NPCStateManager npc)
    {
        npc.Deps.Animation.PlayAnimationInterpolated(_idleAnim, _interpolationTime);
    }

    public override void UpdateState(NPCStateManager npc)
    {
        bool haveTarget = npc.Deps.TargetDetection.HaveTarget();
        IsComplete = haveTarget && CompletedExitTime();
    }

    public override void PhysicsUpdateState(NPCStateManager npc)
    {

    }

    public override void ExitState(NPCStateManager npc)
    {

    }
}
