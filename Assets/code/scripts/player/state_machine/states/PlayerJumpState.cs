using UnityEngine;

public class PlayerJumpState : PlayerBaseState {
    public override void CheckExitState(PlayerStateManager player) {
        // Checks if the player is falling
        if (player.movement.verticalVelocity < 0.0f) {
            player.SwitchState(player.fallState);
        }
        else {
            // Checks for player air jump input, amount of air jumps left and if the jump is in cooldown
            if (player.input.isJumpPressed && player.attributes.haveAirJumpsLeft() && !player.attributes.isJumpOnCooldown) {
                player.SwitchState(player.jumpState);
            }
        }
    }

    public override void EnterState(PlayerStateManager player) {
        player.movement.ApplyJump();
        player.movement.ToggleHorizontalMovementInput(true);

        // Selects the jump animation based on if the player is grounded or not
        if (player.characterController.isGrounded) 
            player.animationManager.PlayAnimationInterpolated(
                player.animationManager.jumpAnimation01, 
                player.animationManager.instantaneousInterpolationTime
                );
        else 
            player.animationManager.PlayAnimationInterpolated(
                player.animationManager.jumpAnimation02, 
                player.animationManager.instantaneousInterpolationTime
                );

    }

    public override void UpdateState(PlayerStateManager player) { }

    public override void PhysicsUpdateState(PlayerStateManager player) { }

    public override void ExitState(PlayerStateManager player) {
        player.movement.SetIsJumping(false);
    }
}
