using UnityEngine;

public class PlayerAttackLightState : PlayerBaseState
{
    float _attackTime, _attackLength;
    bool _queueAttack = false;

    int _currentCombo = 0;
    [SerializeField] private float _comboResetTimer = 0.5f;
    int _comboTimerIndex;

    [Header("Animation params")]
    [SerializeField] private AnimationClip[] _attackAnim;
    [SerializeField] private float _transitionTime;
    [SerializeField] private float[] _attckDuration;

    [Header("State transitions")]
    [SerializeField] private PlayerAttackLightState _attackLightState;
    [SerializeField] private PlayerFallState _fallState;
    [SerializeField] private PlayerIdleState _idleState;


    public override void CheckExitState(PlayerStateManager player)
    {
        if (!CompletedExitTime()) return;

        if (!player.Locomotion.IsGrounded)
        {
            player.SwitchState(_fallState);
            return;
        }

        if (_queueAttack) player.SwitchState(_attackLightState);
        else player.SwitchState(_idleState);
    }

    public override void EnterState(PlayerStateManager player)
    {
        // Resets combo timer
        GlobalTimer.Instance.CancelTimer(_comboTimerIndex);
        
        // Checks if the previous state was dash in order to use the last animation of the attack combo
        if (player.WasPreviousState<PlayerDashState>()) _currentCombo = _attackAnim.Length - 1;

        float speedMultiplier = _attackAnim[_currentCombo].length / _attckDuration[_currentCombo];
        player.Dependencies.AnimationManager.SetFloat("AttackSpeedMuitlplaier", speedMultiplier);

        // Sets the state duration to the current attack anim
        SetDuration(_attckDuration[_currentCombo]);

        player.Dependencies.AnimationManager.PlayInterpolated(_attackAnim[_currentCombo], _transitionTime);

        _attackLength = _attckDuration[_currentCombo];
        _attackTime = 0.0f;
        _queueAttack = false;
    }

    public override void UpdateState(PlayerStateManager player)
    {
        player.Locomotion.CalculateVerticalVelocity();
        player.Locomotion.CalculateSlopeVelocity();
        player.Locomotion.CalculateOnEntitiesVelocity();
        player.Locomotion.Decelerate();

        bool canAimAttack = _attackTime < _attackLength * 0.2f;
        if (canAimAttack) player.Locomotion.RotateTowardsMouseInput();

        _attackTime += Time.deltaTime;

        bool canQueueAttack = _attackTime > _attackLength * 0.5f;
        bool attackInput = player.Dependencies.Input.IsAttackLightPressed;
        if (canQueueAttack && attackInput)
        {
            _queueAttack = true;
        }
    }

    public override void PhysicsUpdateState(PlayerStateManager player)
    {

    }

    public override void ExitState(PlayerStateManager player)
    {
        _currentCombo++;
        if (_currentCombo >= _attackAnim.Length) ResetCombo();
        player.Dependencies.Attack.SetCurrentCombo(_currentCombo);

        _comboTimerIndex = GlobalTimer.Instance.StartTimer(_comboResetTimer, () => ResetCombo());
    }

    private void ResetCombo()
    {
        _currentCombo = 0;
    }
}
