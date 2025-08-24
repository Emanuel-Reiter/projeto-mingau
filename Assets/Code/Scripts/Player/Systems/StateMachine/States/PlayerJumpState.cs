using UnityEngine;

public class PlayerJumpState : PlayerBaseState
{
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
    }

    public override void EnterState(PlayerStateManager player)
    {
        player.Physics.ToggleGroundSnaping(false);
        player.Dependencies.Jump.ConsumeAirJump();

        bool isGrounded = player.Physics.IsGrounded;
        if (isGrounded)
            player.Dependencies.AnimationManager.PlayAnimationInterpolated(
                player.Dependencies.AnimationManager.Jump[0],
                player.Dependencies.AnimationManager.ShortInterpolationTime);
        else
            player.Dependencies.AnimationManager.PlayAnimationInterpolated(
                player.Dependencies.AnimationManager.Jump[1],
                player.Dependencies.AnimationManager.ShortInterpolationTime);

        player.Dependencies.Jump.PerformJump();
    }

    public override void UpdateState(PlayerStateManager player)
    {
        player.Locomotion.CalculateHorizontalMovement();
        player.Locomotion.CalculateVerticalMovement();
        player.Locomotion.CalculateRotation();
    }

    public override void PhysicsUpdateState(PlayerStateManager player) { }

    public override void ExitState(PlayerStateManager player)
    {
        player.Dependencies.Jump.SetIsJumping(false);
    }
}
