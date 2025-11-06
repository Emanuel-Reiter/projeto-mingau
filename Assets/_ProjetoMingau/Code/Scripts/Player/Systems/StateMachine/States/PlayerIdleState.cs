using UnityEngine;

public class PlayerIdleState : PlayerBaseState
{
    [Header("Animation params")]
    [SerializeField] private AnimationClip _idleAnim;
    [SerializeField] private float _transitionTime;

    [Header("State transitions")]
    [SerializeField] private PlayerFallState _fallState;
    [SerializeField] private PlayerJumpState _jumpState;
    [SerializeField] private PlayerDashState _dashState;
    [SerializeField] private PlayerAttackLightState _attackLightState;
    [SerializeField] private PlayerRunState _runState;

    public override void CheckExitState(PlayerStateManager player)
    {
        bool isGrounded = player.Locomotion.IsGrounded;
        if (!isGrounded)
        {
            player.SwitchState(_fallState);
            return;
        }

        bool canJump = player.Dependencies.Jump.CanJump();
        bool jumpInput = player.Dependencies.Input.IsJumpPressed;
        if (canJump && jumpInput)
        {
            player.SwitchState(_jumpState);
            return;
        }

        bool canDash = player.Dependencies.Dash.CanDash();
        bool dashInput = player.Dependencies.Input.IsDashPressed;
        if (canDash && dashInput)
        {
            player.SwitchState(_dashState);
            return;
        }

        if (player.Dependencies.Input.IsAttackLightPressed)
        {
            player.SwitchState(_attackLightState);
        }

        bool isMoving = player.Dependencies.Input.MovementDirectionInput != Vector2.zero;
        if (isMoving) player.SwitchState(_runState);
    }

    public override void EnterState(PlayerStateManager player)
    {
        bool wasAirborne = player.WasPreviousState<PlayerFallState>();
        if (wasAirborne) player.Dependencies.Land.TriggerLandAnimation();

        player.Dependencies.Jump.ResetJumpCount();
        player.Dependencies.Dash.ResetDashCount();

        player.Dependencies.AnimationManager.PlayInterpolated(_idleAnim, _transitionTime);
    }

    public override void UpdateState(PlayerStateManager player)
    {
        player.Locomotion.Decelerate();
        player.Locomotion.CalculateSlopeVelocity();
        player.Locomotion.RotateTowardsMovementDirection();
    }

    public override void PhysicsUpdateState(PlayerStateManager player) 
    {
    
    }

    public override void ExitState(PlayerStateManager player) 
    {
    
    }
}
