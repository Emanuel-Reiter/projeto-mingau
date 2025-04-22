using UnityEngine;

public class PlayerJumpState : PlayerBaseState {
    public override void CheckExitState(PlayerStateManager player) {
        if (player.characterController.isGrounded) {
            if (player.input.movementDirection != Vector2.zero) player.SwitchState(player.runState);
            else player.SwitchState(player.idleState);
        }
    }

    public override void EnterState(PlayerStateManager player) {
        player.movement.ApplyJump();
    }

    public override void UpdateState(PlayerStateManager player) { }

    public override void PhysicsUpdateState(PlayerStateManager player) { }

    public override void ExitState(PlayerStateManager player) { }
}
