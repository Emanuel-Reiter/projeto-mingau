using UnityEngine;

public class PlayerLandState : PlayerBaseState
{
    // TODO: BAD EXIT IMPLEMENTATION NEEDS REFACTORING
    float exitTimer = 0.1f;

    public override void CheckExitState(PlayerStateManager player) {
        if (player.input.isJumpPressed && player.movementAttributes.HaveRemainingJumps()) {
            player.SwitchState(player.jumpState);
        }

        if (exitTimer <= 0.0f) {
            if(player.verticalMovement.GetSlopeRelativeIsGrounded()) {
                if (player.input.movementDirection != Vector2.zero) player.SwitchState(player.runState);
                else player.SwitchState(player.idleState);
            }
            else {
                player.SwitchState(player.fallState);
            }
        }
    }

    public override void EnterState(PlayerStateManager player) {
        player.movementAttributes.ResetAmountOfJumps();
        player.dash.ResetAmountOfDashes();

        player.animationManager.PlayAnimationInterpolated
            (player.animationManager.land_01_anim, 
            player.animationManager.interpolationTime_00);

        exitTimer = player.animationManager.land_01_anim.length;
    }

    public override void ExitState(PlayerStateManager player) { }

    public override void PhysicsUpdateState(PlayerStateManager player) { }

    public override void UpdateState(PlayerStateManager player) {
        player.movement.Decelerate();

        exitTimer -= Time.deltaTime;
    }
}
