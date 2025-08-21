using UnityEngine;

public class PlayerJumpState : PlayerBaseState
{
    public override void CheckExitState(PlayerStateManager player)
    {
        bool isFalling = player.Locomotion.VerticalVelocity < 0.0f;
        if (isFalling) player.SwitchState(player.FallState);

        bool hasRemainingJumps = player.Dependencies.Jump.CurrentJumpCount > 0;
        bool isJumpOnCooldown = player.Dependencies.Jump.IsJumpOnCooldown;

        bool isJumpPressed = player.Dependencies.Input.IsJumpPressed;
        bool canJump = hasRemainingJumps && !isJumpOnCooldown;
        if (isJumpPressed && canJump) player.SwitchState(player.JumpState);
    }

    public override void EnterState(PlayerStateManager player)
    {
        player.Dependencies.Jump.ConsumeJump();
        
        bool isGrounded = player.Physics.IsGrounded;
        if (isGrounded)
            player.Dependencies.AnimationManager.PlayAnimationInterpolated(
                player.Dependencies.AnimationManager.jump_01_anim,
                player.Dependencies.AnimationManager.interpolationTime_00);
        else
            player.Dependencies.AnimationManager.PlayAnimationInterpolated(
                player.Dependencies.AnimationManager.jump_02_anim,
                player.Dependencies.AnimationManager.interpolationTime_00);

        player.Dependencies.Jump.PerformJump();
    }

    public override void UpdateState(PlayerStateManager player)
    {
        player.Locomotion.CalculateHorizontalMovement();
        player.Locomotion.CalculateVerticalMovement();
    }

    public override void PhysicsUpdateState(PlayerStateManager player) { }

    public override void ExitState(PlayerStateManager player)
    {
        player.Dependencies.Jump.SetIsJumping(false);
    }
}
