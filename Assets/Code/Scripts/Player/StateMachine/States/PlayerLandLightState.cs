using UnityEngine;

public class PlayerLandLightState : PlayerBaseState
{
    float _exitTime = 0.1f;

    public override void CheckExitState(PlayerStateManager player)
    {
        _exitTime -= Time.deltaTime;

        bool canJump = player.Dependencies.Jump.CurrentJumpCount > 0;
        bool jumpInput = player.Dependencies.Input.IsJumpPressed;
        if (canJump && jumpInput) player.SwitchState(player.JumpState);

        if (_exitTime > 0.0f) return;

        bool isGrounded = player.Physics.IsGrounded;
        if (!isGrounded) player.SwitchState(player.FallState);

        bool isMoving = player.Dependencies.Input.MovementDirectionInput != Vector2.zero;
        if (isMoving) player.SwitchState(player.RunState);
        else player.SwitchState(player.IdleState);
    }

    public override void EnterState(PlayerStateManager player)
    {
        _exitTime = player.Dependencies.AnimationManager.LandLight.length;

        player.Dependencies.Jump.ResetJumpCount();
        player.Dependencies.Dash.ResetDashCount();

        player.Dependencies.AnimationManager.PlayAnimationInterpolated
            (player.Dependencies.AnimationManager.LandLight,
            player.Dependencies.AnimationManager.ShortInterpolationTime);
    }

    public override void UpdateState(PlayerStateManager player)
    {
        player.Locomotion.Decelerate(player.Dependencies.LocomotionParams.GetGroundedRelativeAcceleration());
        player.Locomotion.CalculateRotation();
    }

    public override void ExitState(PlayerStateManager player) { }

    public override void PhysicsUpdateState(PlayerStateManager player) { }
}
