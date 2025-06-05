using UnityEngine;

public class PlayerFallState : PlayerBaseState {
    public override void CheckExitState(PlayerStateManager player) {
        if (player.characterController.isGrounded) {
            // Checks if the player is grounded, then switch to the fitting grounded state
            if (player.input.movementDirection != Vector2.zero) player.SwitchState(player.runState);
            else player.SwitchState(player.idleState);
        }
        else {
            // Checks for player air jump input
            if (player.input.isJumpPressed && player.attributes.haveAirJumpsLeft()) {
                player.SwitchState(player.jumpState);
            }
        }
    }

    public override void EnterState(PlayerStateManager player) {
        player.movement.ToggleHorizontalMovementInput(true);

        player.animationManager.PlayAnimationInterpolated(player.animationManager.fallAnimation, player.animationManager.fastInterpolationTime);
    }

    public override void UpdateState(PlayerStateManager player) { }

    public override void PhysicsUpdateState(PlayerStateManager player) { }

    public override void ExitState(PlayerStateManager player) { }
}