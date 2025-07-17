using UnityEngine;

public class PlayerJumpState : PlayerBaseState {
    public override void CheckExitState(PlayerStateManager player) {
        // Checks if the player is falling
        if (player.attributes.verticalVelocity < 0.0f) {
            player.SwitchState(player.fallState);
        }
        else {
            // Checks for player air jump input, amount of air jumps left and if the jump is in cooldown
            if (player.input.isJumpPressed && player.attributes.HaveRemainingJumps() && !player.attributes.isJumpOnCooldown) {
                player.SwitchState(player.jumpState);
            }
        }
    }

    public override void EnterState(PlayerStateManager player) {
        player.verticalMovement.ToggleGroundSnaping(false);

        player.jump.ApplyJump();

        // Selects the jump animation and behaviuor based on if the player is grounded or not
        if (player.characterController.isGrounded) {
            player.animationManager.PlayAnimation(player.animationManager.jumpAnimation01);
        }
        else {
            player.animationManager.PlayAnimationInterpolated(
                player.animationManager.jumpAnimation02, 
                player.animationManager.instantaneousInterpolationTime);

            //player.rigManager.SetTailRigWeight(0.0f);
        }
    }

    public override void UpdateState(PlayerStateManager player) {
        player.horizontalMovement.CalculateHorizontalMovement();
        player.verticalMovement.CalculateVerticalMovement();
    }

    public override void PhysicsUpdateState(PlayerStateManager player) { }

    public override void ExitState(PlayerStateManager player) {
        player.attributes.SetIsJumping(false);
        //player.rigManager.SetTailRigWeight(1.0f);
    }
}
