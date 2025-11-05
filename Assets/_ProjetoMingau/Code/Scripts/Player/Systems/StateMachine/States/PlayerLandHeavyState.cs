using UnityEngine;

public class PlayerLandHeavyState : PlayerBaseState
{
    float _exitTime = 0.1f;

    [Header("State transitions")]
    [SerializeField] private PlayerIdleState _idleState;

    public override void CheckExitState(PlayerStateManager player)
    {
        bool canJump = player.Dependencies.Jump.CanJump();
        bool jumpInput = player.Dependencies.Input.IsJumpPressed;
        if (canJump && jumpInput)
        {
            player.SwitchState(player.JumpState);
            return;
        }

        _exitTime -= Time.deltaTime;
        if (_exitTime > 0.0f) return;

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
        _exitTime = player.Dependencies.AnimationManager.LandLight.length;

        player.Dependencies.Jump.ResetJumpCount();
        player.Dependencies.Dash.ResetDashCount();

        player.Dependencies.AnimationManager.PlayInterpolated
            (player.Dependencies.AnimationManager.LandHeavy,
            player.Dependencies.AnimationManager.InstantTransitionTime);
    }

    public override void UpdateState(PlayerStateManager player)
    {
        player.Locomotion.Decelerate();
        player.Locomotion.RotateTowardsMovementDirection();
    }

    public override void ExitState(PlayerStateManager player) { }

    public override void PhysicsUpdateState(PlayerStateManager player) { }
}
