using UnityEngine;

public class PlayerFallState : PlayerBaseState {

    private float airTime;

    public override void CheckExitState(PlayerStateManager player) {
        // Checks for dash input
        if (player.input.isDashPressed && player.dash.HaveReaminingDashes()) player.SwitchState(player.dashState);

        if (player.verticalMovement.GetSlopeRelativeIsGrounded()) {
            // Checks if the player is grounded, then switch to the fitting grounded state

            if (airTime > 0.1f) {
                player.SwitchState(player.landState);
            }
            else {
                if (player.input.movementDirection != Vector2.zero) player.SwitchState(player.runState);
                else player.SwitchState(player.idleState);
            }

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

        player.animationManager.PlayAnimationInterpolated(player.animationManager.fall_01_anim, player.animationManager.interpolationTime_02);

        airTime = 0.0f;
    }

    public override void UpdateState(PlayerStateManager player) {
        player.horizontalMovement.CalculateHorizontalMovement();
        player.verticalMovement.CalculateVerticalMovement();

        airTime += Time.deltaTime;
    }

    public override void PhysicsUpdateState(PlayerStateManager player) { }

    public override void ExitState(PlayerStateManager player) { }
}