using UnityEngine;

public class PlayerFallState : PlayerBaseState {
    public override void CheckExitState(PlayerStateManager player) {
        // Checks for dash input
        if (player.input.isDashPressed && player.dash.HaveReaminingDashes()) player.SwitchState(player.dashState);

        if (player.verticalMovement.GetSlopeRelativeIsGrounded()) {
            // Checks if the player is grounded, then switch to the fitting grounded state
            if (player.input.movementDirection != Vector2.zero) player.SwitchState(player.runState);
            else player.SwitchState(player.idleState);
        }
        else {
            // Checks for player air jump input
            if (player.input.isJumpPressed && player.movementAttributes.HaveRemainingJumps()) {
                player.SwitchState(player.jumpState);
            }
        }
    }

    public override void EnterState(PlayerStateManager player) {
        player.verticalMovement.ToggleGroundSnaping(true);

        player.animationManager.PlayAnimationInterpolated(player.animationManager.fall_01_anim, player.animationManager.fastInterpolationTime);
    }

    public override void UpdateState(PlayerStateManager player) {
        player.horizontalMovement.CalculateHorizontalMovement();
        player.verticalMovement.CalculateVerticalMovement();
    }

    public override void PhysicsUpdateState(PlayerStateManager player) { }

    public override void ExitState(PlayerStateManager player) { }
}