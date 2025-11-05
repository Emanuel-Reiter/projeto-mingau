using UnityEngine;

public class PlayerFallState : PlayerBaseState
{
    private float _airTime;
    private float _heavyLandThresholdInSeconds = 0.5f;

    public override void CheckExitState(PlayerStateManager player)
    {
        _airTime += Time.deltaTime;

        bool isGrounded = player.Locomotion.IsGrounded;
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

        player.Dependencies.AnimationManager.PlayInterpolated(
            player.Dependencies.AnimationManager.Fall,
            player.Dependencies.AnimationManager.FastTransitionTime);
    }

    public override void UpdateState(PlayerStateManager player)
    {
        player.Locomotion.CalculateHorizontalVelocity();
        player.Locomotion.CalculateVerticalVelocity();
        player.Locomotion.RotateTowardsMovementDirection();
    }

    public override void PhysicsUpdateState(PlayerStateManager player) { }

    public override void ExitState(PlayerStateManager player) { }
}