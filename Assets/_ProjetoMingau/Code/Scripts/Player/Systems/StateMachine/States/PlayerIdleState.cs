using UnityEngine;

public class PlayerIdleState : PlayerBaseState
{ 
    public override void CheckExitState(PlayerStateManager player)
    {
        bool isGrounded = player.Locomotion.IsGrounded;
        if (!isGrounded)
        {
            player.SwitchState(player.FallState);
            return;
        }

        bool canJump = player.Dependencies.Jump.CanJump();
        bool jumpInput = player.Dependencies.Input.IsJumpPressed;
        if (canJump && jumpInput)
        {
            player.SwitchState(player.JumpState);
            return;
        }

        bool canDash = player.Dependencies.Dash.CanDash();
        bool dashInput = player.Dependencies.Input.IsDashPressed;
        if (canDash && dashInput)
        {
            player.SwitchState(player.DashState);
            return;
        }

        bool isMoving = player.Dependencies.Input.MovementDirectionInput != Vector2.zero;
        if (isMoving) player.SwitchState(player.RunState);
    }

    public override void EnterState(PlayerStateManager player)
    {
        bool wasAirborne = player.WasPreviousState<PlayerFallState>();
        if (wasAirborne) player.Dependencies.Land.TriggerLandAnimation();

        player.Dependencies.Jump.ResetJumpCount();
        player.Dependencies.Dash.ResetDashCount();

        player.Dependencies.AnimationManager.PlayAnimationInterpolated(
            player.Dependencies.AnimationManager.Idle,
            player.Dependencies.AnimationManager.interpolationTime_02);
    }

    public override void UpdateState(PlayerStateManager player)
    {
        player.Locomotion.Decelerate();
        player.Locomotion.CalculateSlopeVelocity();
        player.Locomotion.RotateTowardsMovementDirection();
    }

    public override void PhysicsUpdateState(PlayerStateManager player) { }

    public override void ExitState(PlayerStateManager player) { }
}
