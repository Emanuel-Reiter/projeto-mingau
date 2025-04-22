using UnityEngine;

public class PlayerRunState : PlayerBaseState {
    public override void CheckExitState(PlayerStateManager player) {
        if (player.characterController.isGrounded) {
            // Checks for player jump input
            if (player.input.isJumpPressed) player.SwitchState(player.jumpState);

            // Checks for the absence of the movment input
            if (player.input.movementDirection == Vector2.zero) player.SwitchState(player.idleState);
        }
    }

    public override void EnterState(PlayerStateManager player) {
        player.movement.ToggleGroundedMovement(true);
    }

    public override void UpdateState(PlayerStateManager player) { }

    public override void PhysicsUpdateState(PlayerStateManager player) { }

    public override void ExitState(PlayerStateManager player) {
        player.movement.ToggleGroundedMovement(false);
    }
}
