using UnityEngine;

public class PlayerLandHeavyState : PlayerBaseState
{
    [Header("Animation params")]
    [SerializeField] private AnimationClip _landHeavyAnim;
    [SerializeField] private float _transitionTime;

    [Header("State transitions")]
    [SerializeField] private PlayerJumpState _jumpState;
    [SerializeField] private PlayerFallState _fallState;
    [SerializeField] private PlayerRunState _runState;
    [SerializeField] private PlayerIdleState _idleState;

    public override void CheckExitState(PlayerStateManager player)
    {
        if (CompletedExitTime()) return;

        bool isGrounded = player.Locomotion.IsGrounded;
        if (!isGrounded)
        {
            player.SwitchState(_fallState);
            return;
        }

        bool isMoving = player.Dependencies.Input.MovementDirectionInput != Vector2.zero;
        if (isMoving) player.SwitchState(_runState);
        else player.SwitchState(_idleState);
    }

    public override void EnterState(PlayerStateManager player)
    {
        SetDuration(_landHeavyAnim.length);

        player.Dependencies.Jump.ResetJumpCount();
        player.Dependencies.Dash.ResetDashCount();

        player.Dependencies.AnimationManager.PlayInterpolated(_landHeavyAnim, _transitionTime);
    }

    public override void UpdateState(PlayerStateManager player)
    {
        player.Locomotion.Decelerate();
        player.Locomotion.RotateTowardsMovementDirection();

        player.Locomotion.CalculateOnEntitiesVelocity();
    }

    public override void ExitState(PlayerStateManager player)
    {
    
    }

    public override void PhysicsUpdateState(PlayerStateManager player)
    { 
    
    }
}
