using UnityEngine;

public class PlayerJumpState : PlayerBaseState
{
    public override void CheckExitState(PlayerStateManager player)
    {
        bool negativeVerticalVelocity = player.locomotionParams.verticalVelocity < 0.0f;

        bool isJumpPressed = player.input.isJumpPressed;
        bool haveRemainingJumps = player.locomotionParams.HaveRemainingJumps();
        bool jumpIsNotInCooldown = !player.locomotionParams.isJumpOnCooldown;
        bool canJump = isJumpPressed && haveRemainingJumps && jumpIsNotInCooldown;

        if (negativeVerticalVelocity)
        {
            player.SwitchState(player.fallState);
        }
        else
        {
            if (canJump)
            {
                player.SwitchState(player.jumpState);
            }
        }
    }

    public override void EnterState(PlayerStateManager player)
    {
        player.verticalMovement.ToggleGroundSnaping(false);

        player.jump.Jump();

        // Selects the jump animation and behaviuor based on if the player is grounded or not
        if (player.characterController.isGrounded)
        {
            player.animationManager.PlayAnimation(player.animationManager.jump_01_anim);
        }
        else
        {
            player.animationManager.PlayAnimationInterpolated(
                player.animationManager.jump_02_anim,
                player.animationManager.interpolationTime_00);
        }
    }

    public override void UpdateState(PlayerStateManager player)
    {
        player.horizontalMovement.CalculateHorizontalMovement();
        player.verticalMovement.CalculateVerticalMovement();
    }

    public override void PhysicsUpdateState(PlayerStateManager player) { }

    public override void ExitState(PlayerStateManager player)
    {
        player.locomotionParams.SetIsJumping(false);
        //player.rigManager.SetTailRigWeight(1.0f);
    }
}
