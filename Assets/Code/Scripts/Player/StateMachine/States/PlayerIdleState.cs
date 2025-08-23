using UnityEngine;

public class PlayerIdleState : PlayerBaseState
{
    public override void CheckExitState(PlayerStateManager player)
    {
        bool isGrounded = player.Physics.IsGrounded;
        if (!isGrounded) player.SwitchState(player.FallState);

        bool canJump = player.Dependencies.Jump.CurrentJumpCount > 0;
        bool jumpInput = player.Dependencies.Input.IsJumpPressed;
        if (canJump && jumpInput) player.SwitchState(player.JumpState);

        bool canDash = player.Dependencies.Dash.CurrentDashCount > 0;
        bool dashInput = player.Dependencies.Input.IsDashPressed;
        if (canDash && dashInput) player.SwitchState(player.DashState);

        bool isMoving = player.Dependencies.Input.MovementDirectionInput != Vector2.zero;
        if (isMoving) player.SwitchState(player.RunState);
    }

    public override void EnterState(PlayerStateManager player)
    {
        player.Dependencies.Jump.ResetJumpCount();
        player.Dependencies.Dash.ResetDashCount();

        player.Physics.ToggleGroundSnaping(true);

        player.Dependencies.AnimationManager.PlayAnimationInterpolated(
            player.Dependencies.AnimationManager.Idle,
            player.Dependencies.AnimationManager.interpolationTime_02);
    }

    public override void UpdateState(PlayerStateManager player)
    {
        player.Locomotion.Decelerate(player.Dependencies.LocomotionParams.GetGroundedRelativeAcceleration());
        player.Locomotion.CalculateRotation();
    }

    public override void PhysicsUpdateState(PlayerStateManager player) { }

    public override void ExitState(PlayerStateManager player) { }
}
