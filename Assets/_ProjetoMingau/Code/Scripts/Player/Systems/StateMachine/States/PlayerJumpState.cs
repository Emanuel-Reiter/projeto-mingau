using UnityEngine;

public class PlayerJumpState : PlayerBaseState
{

    Vector3 startPos;
    Vector3 endPos;

    public override void CheckExitState(PlayerStateManager player)
    {
        bool isFalling = player.Locomotion.VerticalVelocity < 0.0f;
        if (isFalling)
        {
            player.SwitchState(player.FallState);
            return;
        }

        bool canJump = player.Dependencies.Jump.CanJump();
        bool jumpInput = player.Dependencies.Input.IsJumpPressed;
        if (jumpInput && canJump)
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
    }

    public override void EnterState(PlayerStateManager player)
    {
        player.Dependencies.Jump.ConsumeAirJump();

        bool isGrounded = player.Locomotion.IsGrounded;
        if (isGrounded)
            player.Dependencies.AnimationManager.PlayAnimationInterpolated(
                player.Dependencies.AnimationManager.Jump[0],
                player.Dependencies.AnimationManager.ShortInterpolationTime);
        else
            player.Dependencies.AnimationManager.PlayAnimationInterpolated(
                player.Dependencies.AnimationManager.Jump[1],
                player.Dependencies.AnimationManager.ShortInterpolationTime);

        player.Dependencies.Jump.PerformJump();

        startPos = player.transform.position;
    }

    public override void UpdateState(PlayerStateManager player)
    {
        player.Locomotion.CalculateHorizontalVelocity();
        player.Locomotion.CalculateVerticalVelocity();
        player.Locomotion.RotateTowardsMovementDirection();
    }

    public override void PhysicsUpdateState(PlayerStateManager player) { }

    public override void ExitState(PlayerStateManager player)
    {
        player.Dependencies.Jump.SetIsJumping(false);

        endPos = player.transform.position;

        float distance = Vector3.Distance(startPos, endPos);
        // Debug.Log($"Jump distance: {distance}");
    }
}
