using UnityEngine;

public class PlayerJumpState : PlayerBaseState
{

    Vector3 _startPos;
    Vector3 _endPos;

    [Header("Animation params")]
    [SerializeField] private AnimationClip[] _jumpAnim;
    [SerializeField] private float _transitionTime;

    [Header("State transitions")]
    [SerializeField] private PlayerFallState _fallState;
    [SerializeField] private PlayerJumpState _jumpState;
    [SerializeField] private PlayerDashState _dashState;

    public override void CheckExitState(PlayerStateManager player)
    {
        bool isFalling = player.Locomotion.VerticalVelocity < 0.0f;
        if (isFalling)
        {
            player.SwitchState(_fallState);
            return;
        }

        bool canJump = player.Dependencies.Jump.CanJump();
        bool jumpInput = player.Dependencies.Input.IsJumpPressed;
        if (jumpInput && canJump)
        {
            player.SwitchState(_jumpState);
            return;
        }

        bool canDash = player.Dependencies.Dash.CanDash();
        bool dashInput = player.Dependencies.Input.IsDashPressed;
        if (canDash && dashInput)
        {
            player.SwitchState(_dashState);
            return;
        }
    }

    public override void EnterState(PlayerStateManager player)
    {
        player.Dependencies.Jump.ConsumeAirJump();

        bool isGrounded = player.Locomotion.IsGrounded;
        if (isGrounded)
            player.Dependencies.AnimationManager.PlayInterpolated(_jumpAnim[0], _transitionTime);
        else
            player.Dependencies.AnimationManager.PlayInterpolated(_jumpAnim[1], _transitionTime);

        player.Dependencies.Jump.PerformJump();

        _startPos = player.transform.position;
    }

    public override void UpdateState(PlayerStateManager player)
    {
        player.Locomotion.CalculateHorizontalVelocity();
        player.Locomotion.CalculateVerticalVelocity();
        player.Locomotion.RotateTowardsMovementDirection();
    }

    public override void PhysicsUpdateState(PlayerStateManager player) 
    { 
    
    }

    public override void ExitState(PlayerStateManager player)
    {
        player.Dependencies.Jump.SetIsJumping(false);

        _endPos = player.transform.position;

        float distance = Vector3.Distance(_startPos, _endPos);
        // Debug.Log($"Jump distance: {distance}");
    }
}
