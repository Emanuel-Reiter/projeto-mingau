using UnityEngine;

public class PlayerDashState : PlayerBaseState
{
    bool _useGravity = false;
    bool _queueAttack = false;

    public override void CheckExitState(PlayerStateManager player)
    {
        bool isDashing = player.Dependencies.Dash.IsDashing;
        if (isDashing) return;

        if (_queueAttack)
        {
            player.SwitchState(player.AttackLightState);
            return;
        }

            bool isGrounded = player.Locomotion.IsGrounded;
        if (!isGrounded)
        {
            player.SwitchState(player.FallState);
            return;
        }

        bool isMoving = player.Dependencies.Input.MovementDirectionInput != Vector2.zero;
        if (isMoving) player.SwitchState(player.RunState);
        else player.SwitchState(player.IdleState);

    }

    public override void EnterState(PlayerStateManager player)
    {
        player.Dependencies.Dash.ConsumeDash();

        player.Dependencies.AnimationManager.PlayInterpolated(
            player.Dependencies.AnimationManager.Dash,
            player.Dependencies.AnimationManager.InstantTransitionTime);

        player.Dependencies.Dash.PerformDash();

        _useGravity = player.Locomotion.IsGrounded;
        _queueAttack = false;
    }

    public override void ExitState(PlayerStateManager player) { }

    public override void PhysicsUpdateState(PlayerStateManager player) { }

    public override void UpdateState(PlayerStateManager player)
    {
        player.Locomotion.Decelerate();
        player.Locomotion.RotateTowardsMovementDirection();

        if (_useGravity) player.Locomotion.CalculateVerticalVelocity();

        bool attackInput = player.Dependencies.Input.IsAttackLightPressed;
        if (attackInput)
        {
            _queueAttack = true;
        }
    }
}