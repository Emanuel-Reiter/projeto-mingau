using UnityEngine;

public class PlayerIdleState : PlayerBaseState {
    public override void CheckExitState(PlayerStateManager player) {
        // Checks for player movement input
        if (player.input.movementDirection != Vector2.zero) player.SwitchState(player.runState);
    }

    public override void EnterState(PlayerStateManager player) { }

    public override void UpdateState(PlayerStateManager player) { }

    public override void PhysicsUpdateState(PlayerStateManager player) { }

    public override void ExitState(PlayerStateManager player) { }
}
