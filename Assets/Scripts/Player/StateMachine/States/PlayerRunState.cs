using UnityEngine;

public class PlayerRunState : PlayerBaseState {
    public override void CheckExitState(PlayerStateManager player) {
        // Checks for the absence of the movment input
        if (player.input.movementDirection == Vector2.zero) player.SwitchState(player.idleState);
    }

    public override void EnterState(PlayerStateManager player) { }

    public override void UpdateState(PlayerStateManager player) {
        player.movement.Move();
        player.movement.ChangeDirection();
    }

    public override void PhysicsUpdateState(PlayerStateManager player) { }

    public override void ExitState(PlayerStateManager player) { }
}
