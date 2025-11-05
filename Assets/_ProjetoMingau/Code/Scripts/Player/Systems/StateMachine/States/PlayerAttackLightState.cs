using UnityEngine;

public class PlayerAttackLightState : PlayerBaseState
{
    float _attackTime, _attackLength;
    bool _queueAttack = false;

    int _currentCombo = 0;
    float _comboResetTimer = 1.0f;
    int _timerIndex;

    public override void CheckExitState(PlayerStateManager player)
    {
        if (_attackTime < player.Dependencies.AnimationManager.AttackLight[_currentCombo].length)
        {
            return;
        }

        if (_queueAttack) player.SwitchState(player.AttackLightState);
        else player.SwitchState(player.IdleState);
    }

    public override void EnterState(PlayerStateManager player)
    {
        player.Dependencies.GlobalTimer.CancelTimer(_timerIndex);

        // Checks if the previous state was dash in order to use the last animation of the attack combo
        if (player.WasPreviousState<PlayerDashState>()) _currentCombo = player.Dependencies.AnimationManager.AttackLight.Length - 1;

        player.Dependencies.AnimationManager.PlayInterpolated(
            player.Dependencies.AnimationManager.AttackLight[_currentCombo],
            player.Dependencies.AnimationManager.FastTransitionTime);

        _attackLength = player.Dependencies.AnimationManager.AttackLight[_currentCombo].length;
        _attackTime = 0.0f;
        _queueAttack = false;
    }

    public override void UpdateState(PlayerStateManager player)
    {
        player.Locomotion.CalculateVerticalVelocity();
        player.Locomotion.CalculateSlopeVelocity();
        player.Locomotion.Decelerate();

        bool canAimAttack = _attackTime < _attackLength * 0.25f;
        if (canAimAttack) player.Locomotion.RotateTowardsInputDirection(10.0f);

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
        if (_currentCombo >= player.Dependencies.AnimationManager.AttackLight.Length) ResetCombo();

        _timerIndex = player.Dependencies.GlobalTimer.StartTimer(_comboResetTimer, () => ResetCombo());
    }

    private void ResetCombo()
    {
        _currentCombo = 0;
    }
}
