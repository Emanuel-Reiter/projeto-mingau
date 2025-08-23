using UnityEngine;

public class PlayerRunState : PlayerBaseState
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
        if (!isMoving) player.SwitchState(player.IdleState);
    }

    public override void EnterState(PlayerStateManager player)
    {
        player.Dependencies.Jump.ResetJumpCount();
        player.Dependencies.Dash.ResetDashCount();

        player.Physics.ToggleGroundSnaping(true);

        player.Dependencies.AnimationManager.PlayAnimationInterpolated(
            player.Dependencies.AnimationManager.Run,
            player.Dependencies.AnimationManager.interpolationTime_02);
    }

    public override void UpdateState(PlayerStateManager player)
    {
        player.Locomotion.CalculateHorizontalMovement();
        player.Locomotion.CalculateVerticalMovement();
        player.Locomotion.CalculateRotation();
    }

    public override void PhysicsUpdateState(PlayerStateManager player) { }

    public override void ExitState(PlayerStateManager player) { }
}
