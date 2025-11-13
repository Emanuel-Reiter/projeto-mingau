using UnityEngine;

public class PlayerDash : MonoBehaviour
{
    private PlayerDependencies _dependencies;
    private PlayerLocomotion _locomotion;

    [Header("Params")]
    [SerializeField] private float _groundedDashSpeed = 45.0f;
    [SerializeField] private float _airDashSpeed = 30.0f;


    private int _maxDashCount = 1;
    private int _currentDashCount = 0;

    private void Start()
    {
        _dependencies = GetComponent<PlayerDependencies>();
        _locomotion = GetComponent<PlayerLocomotion>();
    }

    public void PerformDash()
    {
        float targetDashSpeed = _locomotion.IsGrounded ? _groundedDashSpeed : _airDashSpeed;
        float dashVelocity = Mathf.Sqrt(2.0f * _locomotion.GetCurrentMovementSpeed() * targetDashSpeed);

        Vector2 inputVector = _dependencies.Input.MovementDirectionInput;

        Vector3 dashDirection = transform.forward;
        if (inputVector != Vector2.zero) dashDirection = _locomotion.GetCameraRelativeDirection(inputVector);

        _locomotion.SetVerticalVelocity(0.0f);
        _locomotion.SetHorizontalVelocity(dashDirection * dashVelocity);
    }

    #region Dash State Management
    public void ResetDashCount() { _currentDashCount = _maxDashCount; }

    public void AddDash() { _currentDashCount++; }

    public void ConsumeDash() { _currentDashCount--; }

    public bool CanDash() { return _currentDashCount > 0 && !_locomotion.OnSteepSlope; }
    #endregion
}
