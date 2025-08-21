using UnityEngine;

public class PlayerDash : MonoBehaviour
{
    private PlayerDependencies _dependencies;
    private PlayerLocomotion _locomotion;
    private PlayerPhysics _physics;

    [Header("Params")]
    [SerializeField] private float _dashSpeed = 48.0f;
    public float DashSpeed => _dashSpeed;

    private int _maxDashCount = 1;
    public int MaxDashCount => _maxDashCount;

    private int _currentDashCount = 0;
    public bool CurrentDashCount => _currentDashCount > 0;

    private bool _isDashing = false;
    public bool IsDashing => _isDashing;

    private void Start()
    {
        _dependencies = GetComponent<PlayerDependencies>();
        _locomotion = GetComponent<PlayerLocomotion>();
        _physics = GetComponent<PlayerPhysics>();
    }

    public void PerformDash()
    {
        _isDashing = true;

        float dashVelocity = Mathf.Sqrt(2.0f * _dependencies.LocomotionParams.GetCurrentSpeed() * _dashSpeed);

        Vector2 inputVector = _dependencies.Input.MovementDirectionInput;
        Vector3 dashDirection = _locomotion.GetCameraRelativeDirection(inputVector);

        _locomotion.SetHorizontalVelocity(dashDirection * dashVelocity);

        // TODO: Refactor the dash duration and deceleration loginc
        float dashDuration = 0.0f;
        if (_physics.IsGrounded) dashDuration = _dependencies.AnimationManager.dash_01_anim.length;
        else dashDuration = _dependencies.AnimationManager.dash_01_anim.length / 2.0f;

        int dashTimer = _dependencies.GlobalTimer.StartTimer(dashDuration, () => { _isDashing = false; });
    }

    #region Dash State Management
    public void ResetDashCount() { _currentDashCount = _maxDashCount; }

    public void AddDash() { _currentDashCount++; }

    public void ConsumeDash() { _currentDashCount--; }
    #endregion
}
