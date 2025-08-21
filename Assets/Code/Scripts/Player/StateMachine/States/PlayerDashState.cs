using UnityEngine;

public class PlayerDashState : PlayerBaseState
{
    public override void CheckExitState(PlayerStateManager player)
    {
        bool isDashing = player.Dependencies.Dash.IsDashing;
        if (isDashing) return;

        bool isGrounded = player.Physics.IsGrounded;
        if (!isGrounded) player.SwitchState(player.FallState);

        bool hasInput = player.Dependencies.Input.MovementDirectionInput != Vector2.zero;
        if (hasInput) player.SwitchState(player.RunState);
        else player.SwitchState(player.IdleState);
    }

    public override void EnterState(PlayerStateManager player)
    {
        player.Dependencies.Dash.ConsumeDash();

        player.Dependencies.AnimationManager.PlayAnimationInterpolated(
            player.Dependencies.AnimationManager.dash_01_anim,
            player.Dependencies.AnimationManager.interpolationTime_00);

        player.Dependencies.Dash.PerformDash();
    }

    public override void ExitState(PlayerStateManager player) { }

    public override void PhysicsUpdateState(PlayerStateManager player) { }

    public override void UpdateState(PlayerStateManager player)
    {
        player.Locomotion.Decelerate(player.Dependencies.LocomotionParams.DashAccelerationRate);
    }
}