using UnityEngine;

public class NPCFlinchState : NPCBaseState
{
    [Header("State tranisitions")]
    [SerializeField] private NPCIdleState _idleState;

    [Header("State params")]
    [SerializeField] private AnimationClip _flinchAnim;
    [SerializeField] private float _interpolationTime = 0.05f;

    public override void CheckExitState(NPCStateManager npc)
    {
        Debug.Log(CompletedExitTime());
        if (!CompletedExitTime())
        {
            return;
        }

        npc.SwitchState(_idleState);
    }

    public override void EnterState(NPCStateManager npc)
    {
        SetDuration(_flinchAnim.length);
        npc.Dependencies.Animation.PlayAnimationInterpolated(_flinchAnim, _interpolationTime);
    }

    public override void ExitState(NPCStateManager npc)
    {
        npc.Dependencies.Attributes.ResetPosture();
    }

    public override void PhysicsUpdateState(NPCStateManager npc)
    {

    }

    public override void UpdateState(NPCStateManager npc)
    {

    }
}
