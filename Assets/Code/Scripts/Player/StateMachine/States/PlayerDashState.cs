using Unity.VisualScripting;
using UnityEngine;

public class PlayerDashState : PlayerBaseState {
    public override void CheckExitState(PlayerStateManager player) {

        if(!player.dash.isDashing) {
            // Checks if the player is currently grounded
            if (player.verticalMovement.GetSlopeRelativeIsGrounded()) {
                // Checks for player movement input
                if (player.input.movementDirection != Vector2.zero) player.SwitchState(player.runState);
                else player.SwitchState(player.idleState);
            }
            else {
                // If not grounded switch to fall state
                player.SwitchState(player.fallState);
            }
        }
    }

    public override void EnterState(PlayerStateManager player) {
        player.dash.DecreaseAmountOfDashes();

        player.animationManager.PlayAnimationInterpolated(
            player.animationManager.dash_01_anim,
            player.animationManager.interpolationTime_00);

        player.dash.Dash();
    }

    public override void ExitState(PlayerStateManager player) { }

    public override void PhysicsUpdateState(PlayerStateManager player) { }

    public override void UpdateState(PlayerStateManager player) {
        //player.verticalMovement.CalculateVerticalMovement();
        player.movement.Decelerate();
    }
}
