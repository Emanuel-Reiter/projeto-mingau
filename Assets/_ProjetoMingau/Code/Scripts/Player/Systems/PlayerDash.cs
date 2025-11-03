using UnityEngine;

public class PlayerDash : MonoBehaviour
{
    private PlayerDependencies _dependencies;
    private PlayerLocomotion _locomotion;

    [Header("Params")]
    [SerializeField] private float _dashSpeed = 48.0f;

    private int _maxDashCount = 1;
    public int MaxDashCount => _maxDashCount;

    private int _currentDashCount = 0;
    public int CurrentDashCount => _currentDashCount;

    private bool _isDashing = false;
    public bool IsDashing => _isDashing;

    private void Start()
    {
        _dependencies = GetComponent<PlayerDependencies>();
        _locomotion = GetComponent<PlayerLocomotion>();
    }

    public void PerformDash()
    {
        _isDashing = true;

        float dashVelocity = Mathf.Sqrt(2.0f * _locomotion.GetCurrentMovementSpeed() * _dashSpeed);

        Vector2 inputVector = _dependencies.Input.MovementDirectionInput;

        Vector3 dashDirection = transform.forward;
        if (inputVector != Vector2.zero) dashDirection = _locomotion.GetCameraRelativeDirection(inputVector);

        _locomotion.SetVerticalVelocity(0.0f);
        _locomotion.SetHorizontalVelocity(dashDirection * dashVelocity);

        // TODO: Refactor the dash duration and deceleration loginc
        float dashDuration = 0.0f;
        if (_locomotion.IsGrounded) dashDuration = _dependencies.AnimationManager.Dash.length;
        else dashDuration = _dependencies.AnimationManager.Dash.length / 2.0f;

        int dashTimer = _dependencies.GlobalTimer.StartTimer(dashDuration, () => { _isDashing = false; });
    }

    #region Dash State Management
    public void ResetDashCount() { _currentDashCount = _maxDashCount; }

    public void AddDash() { _currentDashCount++; }

    public void ConsumeDash() { _currentDashCount--; }

    public bool CanDash() { return _currentDashCount > 0 && !_locomotion.OnSteepSlope; }
    #endregion
}
