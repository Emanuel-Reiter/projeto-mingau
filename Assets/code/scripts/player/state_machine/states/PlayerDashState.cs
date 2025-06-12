using Unity.VisualScripting;
using UnityEngine;

public class PlayerDashState : PlayerBaseState {
    public override void CheckExitState(PlayerStateManager player) { }

    public override void EnterState(PlayerStateManager player) {
        player.animationManager.PlayAnimationInterpolated(
            player.animationManager.runAnimation,
            player.animationManager.instantaneousInterpolationTime);
    }

    public override void ExitState(PlayerStateManager player) { }

    public override void PhysicsUpdateState(PlayerStateManager player) { }

    public override void UpdateState(PlayerStateManager player) { }
}
