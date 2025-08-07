using UnityEngine;

public class PlayerRunState : PlayerBaseState {
    public override void CheckExitState(PlayerStateManager player) {

        // Checks for dash input
        if (player.input.isDashPressed && player.dash.HaveReaminingDashes()) player.SwitchState(player.dashState);

        // Checks of the player is currently grounded
        if (player.verticalMovement.GetSlopeRelativeIsGrounded()) {
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
        // Reset player jumps and dashes
        player.movementAttributes.ResetAmountOfJumps();
        player.dash.ResetAmountOfDashes();

        player.verticalMovement.ToggleGroundSnaping(true);

        player.animationManager.PlayAnimationInterpolated(player.animationManager.run_01_anim, player.animationManager.interpolationTime_01);
    }

    public override void UpdateState(PlayerStateManager player) {
        player.horizontalMovement.CalculateHorizontalMovement();
        player.verticalMovement.CalculateVerticalMovement();
    }

    public override void PhysicsUpdateState(PlayerStateManager player) { }

    public override void ExitState(PlayerStateManager player) { }
}
