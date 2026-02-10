using UnityEngine;

public class NPCAttackState : NPCBaseState
{
    [Header("State tranisitions")]
    [SerializeField] private NPCIdleState _idleState;
    [SerializeField] private NPCFlinchState _flinchState;

    [Header("State params")]
    [SerializeField] private AnimationClip _attackAnim;
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

        if (!CompletedExitTime())
        {
            return;
        }

        npc.SwitchState(_idleState);
    }

    public override void EnterState(NPCStateManager npc)
    {
        SetDuration(_attackAnim.length);
        npc.Deps.Animation.PlayAnimationInterpolated(_attackAnim, _interpolationTime);
    }

    public override void ExitState(NPCStateManager npc)
    {
        
    }

    public override void PhysicsUpdateState(NPCStateManager npc)
    {

    }

    public override void UpdateState(NPCStateManager npc)
    {

    }
}
