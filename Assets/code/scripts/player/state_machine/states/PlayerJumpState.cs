using UnityEngine;

public class PlayerJumpState : PlayerBaseState {
    public override void CheckExitState(PlayerStateManager player) {
        // Checks if the player is falling
        if (player.movementAttributes.verticalVelocity < 0.0f) {
            player.SwitchState(player.fallState);
        }
        else {
            // Checks for player air jump input, amount of air jumps left and if the jump is in cooldown
            if (player.input.isJumpPressed && player.movementAttributes.HaveRemainingJumps() && !player.movementAttributes.isJumpOnCooldown) {
                player.SwitchState(player.jumpState);
            }
        }
    }

    public override void EnterState(PlayerStateManager player) {
        player.verticalMovement.ToggleGroundSnaping(false);

        player.jump.Jump();

        // Selects the jump animation and behaviuor based on if the player is grounded or not
        if (player.characterController.isGrounded) {
            player.animationManager.PlayAnimation(player.animationManager.jump_01_anim);
        }
        else {
            player.animationManager.PlayAnimationInterpolated(
                player.animationManager.jump_02_anim, 
                player.animationManager.interpolationTime_00);

            //player.rigManager.SetTailRigWeight(0.0f);
        }
    }

    public override void UpdateState(PlayerStateManager player) {
        player.horizontalMovement.CalculateHorizontalMovement();
        player.verticalMovement.CalculateVerticalMovement();
    }

    public override void PhysicsUpdateState(PlayerStateManager player) { }

    public override void ExitState(PlayerStateManager player) {
        player.movementAttributes.SetIsJumping(false);
        //player.rigManager.SetTailRigWeight(1.0f);
    }
}
