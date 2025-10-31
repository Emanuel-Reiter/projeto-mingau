using UnityEngine;

public class PlayerJump : MonoBehaviour
{
    private PlayerDependencies _dependencies;
    private PlayerLocomotion _locomotion;
    private PlayerPhysics _physics;

    [Header("Params")]
    [SerializeField] private float _jumpHeight = 2.25f;

    private bool _isJumping = false;
    public bool IsJumping => _isJumping;

    private bool _isJumpOnCooldown = false;
    public bool IsJumpOnCooldown => _isJumpOnCooldown;

    private float _jumpCooldown = 0.067f;

    private int _maxAirJumpsCount = 1;

    private int _currentAirJumpsCount = 0;
    public int CurrentAirJumpsCount => _currentAirJumpsCount;

    private void Start()
    {
        _dependencies = GetComponent<PlayerDependencies>();
        _locomotion = GetComponent<PlayerLocomotion>();
        _physics = GetComponent<PlayerPhysics>();
    }

    public void PerformJump()
    {
        _isJumping = true;
        _isJumpOnCooldown = true;

        int jumpCooldownTimer = _dependencies.GlobalTimer.StartTimer(_jumpCooldown, () => _isJumpOnCooldown = false);

        _locomotion.SetVerticalVelocity(0.0f);

        float jumpVelocity = Mathf.Sqrt(-2.0f * _dependencies.LocomotionParams.AerialGravityAcceleration * _jumpHeight);
        _locomotion.SetVerticalVelocity(jumpVelocity);
    }

    public void SetIsJumping(bool isJumping) { _isJumping = isJumping; }

    public void ResetJumpCount() { _currentAirJumpsCount = _maxAirJumpsCount; }

    public void AddJump() { _currentAirJumpsCount++; }

    public void ConsumeAirJump() 
    { 
        if (!_physics.IsGrounded) _currentAirJumpsCount--; 
    }

    public bool CanJump() { 
        return (_physics.IsGrounded || _currentAirJumpsCount > 0) && !_isJumpOnCooldown && !_physics.GetOnSteepSlope();
    }
}
