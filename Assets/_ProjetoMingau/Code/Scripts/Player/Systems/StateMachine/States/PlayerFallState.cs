using UnityEngine;

public class PlayerFallState : PlayerBaseState
{
    private float _airTime;
    private float _heavyLandThresholdInSeconds = 0.5f;

    [Header("Animation params")]
    [SerializeField] private AnimationClip _fallAnim;
    [SerializeField] private float _transitionTime;

    [Header("State transitions")]
    [SerializeField] private PlayerLandHeavyState _heavyLandHeavyState;
    [SerializeField] private PlayerRunState _runState;
    [SerializeField] private PlayerIdleState _idleState;
    [SerializeField] private PlayerDashState _dashState;
    [SerializeField] private PlayerJumpState _jumpState;

    public override void CheckExitState(PlayerStateManager player)
    {
        _airTime += Time.deltaTime;

        bool isGrounded = player.Locomotion.IsGrounded;
        if (isGrounded)
        {
            bool heavyLand = _airTime > _heavyLandThresholdInSeconds;

            if (heavyLand) player.SwitchState(_heavyLandHeavyState);
            else
            {
                bool isMoving = player.Dependencies.Input.MovementDirectionInput != Vector2.zero;
                if (isMoving) player.SwitchState(_runState);
                else player.SwitchState(_idleState);
            }

            return;
        }

        bool canDash = player.Dependencies.Dash.CanDash();
        bool dashInput = player.Dependencies.Input.IsDashPressed;
        if (canDash && dashInput)
        {
            player.SwitchState(_dashState);
            return;
        }

        bool canJump = player.Dependencies.Jump.CanJump();
        bool jumpInput = player.Dependencies.Input.IsJumpPressed;
        if (canJump && jumpInput)
        {
            player.SwitchState(_jumpState);
            return;
        }
    }

    public override void EnterState(PlayerStateManager player)
    {
        _airTime = 0.0f;

        player.Dependencies.AnimationManager.PlayInterpolated(_fallAnim, _transitionTime);
    }

    public override void UpdateState(PlayerStateManager player)
    {
        player.Locomotion.CalculateHorizontalVelocity();
        player.Locomotion.CalculateVerticalVelocity();
        player.Locomotion.RotateTowardsMovementDirection();
        player.Locomotion.CalculateOnEntitiesVelocity();
    }

    public override void PhysicsUpdateState(PlayerStateManager player) 
    {

    }

    public override void ExitState(PlayerStateManager player) 
    {
    
    }
}