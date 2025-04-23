using UnityEngine;

public class PlayerIdleState : PlayerBaseState {
    public override void CheckExitState(PlayerStateManager player) {
        
        // Checks if the player is grounded
        if(player.characterController.isGrounded) {
            // Checks for player jump input
            if (player.input.isJumpPressed) player.SwitchState(player.jumpState);
            
            // Checks for player movement input
            if (player.input.movementDirection != Vector2.zero) player.SwitchState(player.runState);
        }
    }

    public override void EnterState(PlayerStateManager player) {
        player.movement.ToggleGroundSnaping(true);
        player.movement.ToggleHorizontalMovementInput(false);
    }

    public override void UpdateState(PlayerStateManager player) { }

    public override void PhysicsUpdateState(PlayerStateManager player) { }

    public override void ExitState(PlayerStateManager player) {
        player.movement.ToggleGroundSnaping(false);
    }
}
