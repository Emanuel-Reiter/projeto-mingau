using UnityEngine;

public class PlayerIdleState : PlayerBaseState {
    public override void CheckExitState(PlayerStateManager player) {
        
        // Checks if the player is currently grounded
        if(player.verticalMovement.GetSlopeRelativeIsGrounded()) {
            // Checks for player jump input
            if (player.input.isJumpPressed) player.SwitchState(player.jumpState);
            
            // Checks for player movement input
            if (player.input.movementDirection != Vector2.zero) player.SwitchState(player.runState);
        }
        else {
            // If not grounded switch to fall state
            player.SwitchState(player.fallState);
        }
    }

    public override void EnterState(PlayerStateManager player) {
        // Reset player air jumps
        player.attributes.ResetAmountOfJumps();

        player.verticalMovement.ToggleGroundSnaping(true);

        player.animationManager.PlayAnimationInterpolated(player.animationManager.idleAnimation, player.animationManager.fastInterpolationTime);
    }

    public override void UpdateState(PlayerStateManager player) {
        player.horizontalMovement.CalculateHorizontalMovement();
        player.verticalMovement.CalculateVerticalMovement();
    }

    public override void PhysicsUpdateState(PlayerStateManager player) { }

    public override void ExitState(PlayerStateManager player) { }
}
