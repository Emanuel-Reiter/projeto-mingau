using UnityEngine;
using System.Collections;

public class PlayerJump : MonoBehaviour
{
    private PlayerDependencies _dependencies;
    private PlayerLocomotion _locomotion;

    [Header("Params")]
    [SerializeField] private float _jumpHeight = 2.25f;

    private bool _isJumping = false;
    public bool IsJumping => _isJumping;

    private bool _isGravityDisabled = false;
    public bool IsGravityDisabled => _isGravityDisabled;

    private bool _isJumpOnCooldown = false;
    public bool IsJumpOnCooldown => _isJumpOnCooldown;

    private float _jumpCooldown = 0.067f;

    [SerializeField] private int _maxAirJumpsCount = 1;

    private int _currentAirJumpsCount = 0;
    public int CurrentAirJumpsCount => _currentAirJumpsCount;

    private void Start()
    {
        _dependencies = GetComponent<PlayerDependencies>();
        _locomotion = GetComponent<PlayerLocomotion>();
    }

    public void PerformJump()
    {
        SetIsJumping(true);
        _isJumpOnCooldown = true;

        // Deactivate the gravity calc for a split second to allow the player to get off the ground
        StartCoroutine(GravityToggleCoroutine());

        int jumpCooldownTimer = TimerSingleton.Instance.StartTimer(_jumpCooldown, () => _isJumpOnCooldown = false);
        _locomotion.SetVerticalVelocity(0.0f);

        float jumpVelocity = Mathf.Sqrt(-2.0f * _dependencies.LocomotionParams.Gravity * _jumpHeight);
        _locomotion.SetVerticalVelocity(jumpVelocity);
    }

    private IEnumerator GravityToggleCoroutine()
    {
        SetDisableGravity(true);
        yield return new WaitForSeconds(0.005f);
        SetDisableGravity(false);
    }

    public void SetIsJumping(bool isJumping) { _isJumping = isJumping; }
    public void SetDisableGravity(bool isGravityDisabled) { _isGravityDisabled = isGravityDisabled; }

    public void ResetJumpCount() { _currentAirJumpsCount = _maxAirJumpsCount; }

    public void AddJump() { _currentAirJumpsCount++; }

    public void ConsumeAirJump() 
    { 
        if (!_locomotion.IsGrounded) _currentAirJumpsCount--; 
    }

    public bool CanJump() 
    { 
        return (_locomotion.IsGrounded || _currentAirJumpsCount > 0) && !_isJumpOnCooldown && !_locomotion.OnSteepSlope;
    }
}
