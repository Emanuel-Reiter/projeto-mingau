using UnityEngine;

public class PlayerDashState : PlayerBaseState
{
    bool _useGravity = false;
    bool _queueAttack = false;

    [Header("Animation params")]
    [SerializeField] private AnimationClip _dashAnim;
    [SerializeField] private float _transitionTime;

    [Header("State transitions")]
    [SerializeField] private PlayerAttackLightState _attackLightState;
    [SerializeField] private PlayerFallState _fallState;
    [SerializeField] private PlayerRunState _runState;
    [SerializeField] private PlayerIdleState _idleState;

    public override void CheckExitState(PlayerStateManager player)
    {
        if (!CompletedExitTime()) return;

        if (_queueAttack)
        {
            player.SwitchState(_attackLightState);
            return;
        }

        bool isGrounded = player.Locomotion.IsGrounded;
        if (!isGrounded)
        {
            player.SwitchState(_fallState);
            return;
        }

        bool isMoving = player.Dependencies.Input.MovementDirectionInput != Vector2.zero;
        if (isMoving) player.SwitchState(_runState);
        else player.SwitchState(_idleState);

    }

    public override void EnterState(PlayerStateManager player)
    {
        // TODO: Refactor the dash duration and deceleration logic
        float dashDuration = 0.0f;
        if (player.Locomotion.IsGrounded) dashDuration = _dashAnim.length;
        else dashDuration = _dashAnim.length / 2.0f;
        SetDuration(dashDuration);

        player.Dependencies.Dash.ConsumeDash();

        player.Dependencies.AnimationManager.PlayInterpolated(_dashAnim, _transitionTime);

        player.Dependencies.Dash.PerformDash();

        _useGravity = player.Locomotion.IsGrounded;
        _queueAttack = false;
    }

    public override void UpdateState(PlayerStateManager player)
    {
        player.Locomotion.Decelerate();
        player.Locomotion.RotateTowardsMovementDirection();
        if (_useGravity) player.Locomotion.CalculateVerticalVelocity();

        bool attackInput = player.Dependencies.Input.IsAttackLightPressed;
        if (attackInput) _queueAttack = true;
    }

    public override void PhysicsUpdateState(PlayerStateManager player)
    {
    
    }

    public override void ExitState(PlayerStateManager player)
    {

    }

}