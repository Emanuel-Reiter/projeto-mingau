using UnityEngine;

public class PlayerRunState : PlayerBaseState {
    public override void CheckExitState(PlayerStateManager player) {

        // Checks of the player is currently grounded
        if (player.characterController.isGrounded) {
            // Checks for player jump input
            if (player.input.isJumpPressed) player.SwitchState(player.jumpState);

            // Checks for the absence of the movment input
            if (player.input.movementDirection == Vector2.zero) player.SwitchState(player.idleState);
        }
        else {
            // If not grounded switch to fall state
            player.SwitchState(player.fallState);
        }
    }

    public override void EnterState(PlayerStateManager player) {
        // Reset player air jumps
        player.attributes.ResetAirJumps();

        player.movement.ToggleGroundSnaping(true);
        player.movement.ToggleHorizontalMovementInput(true);

        player.animationManager.PlayAnimationInterpolated(player.animationManager.runAnimation, player.animationManager.fastInterpolationTime);
    }

    public override void UpdateState(PlayerStateManager player) { }

    public override void PhysicsUpdateState(PlayerStateManager player) { }

    public override void ExitState(PlayerStateManager player) {
        player.movement.ToggleGroundSnaping(false);
    }
}
