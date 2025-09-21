using UnityEngine;

public class PlayerFallState : PlayerBaseState
{
    private float _airTime;
    private float _heavyLandThresholdInSeconds = 0.5f;

    public override void CheckExitState(PlayerStateManager player)
    {
        _airTime += Time.deltaTime;

        bool isGrounded = player.Physics.IsGrounded;
        if (isGrounded)
        {
            bool heavyLand = _airTime > _heavyLandThresholdInSeconds;

            if (heavyLand) player.SwitchState(player.LandHeavyState);
            else
            {
                bool isMoving = player.Dependencies.Input.MovementDirectionInput != Vector2.zero;
                if (isMoving) player.SwitchState(player.RunState);
                else player.SwitchState(player.IdleState);
            }

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
        if(player.Physics.StandingOnUnstableGround)
        {
            player.Physics.CalculateUnstableGroundMovement();
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