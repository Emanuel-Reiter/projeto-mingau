using UnityEngine;

public class PlayerFallState : PlayerBaseState
{
    private float _airTime;

    private float _lightLandThresholdInSeconds = 0.1f;
    private float _heavyLandThresholdInSeconds = 0.4f;

    public override void CheckExitState(PlayerStateManager player)
    {
        _airTime += Time.deltaTime;

        bool isGrounded = player.Physics.IsGrounded;
        if (isGrounded)
        {
            bool lightLand = _airTime > _lightLandThresholdInSeconds && _airTime < _heavyLandThresholdInSeconds;
            bool heavyLand = _airTime > _heavyLandThresholdInSeconds;
            bool noLand = !lightLand && !heavyLand;

            if (lightLand) player.SwitchState(player.LandLightState);
            if (heavyLand) player.SwitchState(player.LandHeavyState);
            if (noLand) player.SwitchState(player.IdleState);

            return;
        }

        bool canDash = player.Dependencies.Dash.CanDash();
        bool dashInput = player.Dependencies.Input.IsDashPressed;
        if (canDash && dashInput)
        {
            player.SwitchState(player.DashState);
            return;
        }

        bool canJump = player.Dependencies.Jump.CanJump();
        bool jumpInput = player.Dependencies.Input.IsJumpPressed;
        if(canJump && jumpInput)
        {
            player.SwitchState(player.JumpState);
            return;
        }
    }

    public override void EnterState(PlayerStateManager player)
    {
        _airTime = 0.0f;

        player.Physics.ToggleGroundSnaping(true);

        player.Dependencies.AnimationManager.PlayAnimationInterpolated(
            player.Dependencies.AnimationManager.Fall,
            player.Dependencies.AnimationManager.interpolationTime_02);
    }

    public override void UpdateState(PlayerStateManager player)
    {
        if(player.Physics.StandingOnEntity)
        {
            player.Physics.CalculateEntityMovement();
            return;
        }

        if (player.Physics.GetOnSteepSlope())
        {
            player.Physics.CalculateSlopeMovement();
            return;
        }

        player.Locomotion.CalculateHorizontalMovement();
        player.Locomotion.CalculateVerticalMovement();
        player.Locomotion.CalculateRotation();
    }

    public override void PhysicsUpdateState(PlayerStateManager player) { }

    public override void ExitState(PlayerStateManager player) { }
}