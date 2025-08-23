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

    private int _maxJumpCount = 2;

    private int _currentJumpCount = 0;
    public int CurrentJumpCount => _currentJumpCount;

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

    public void ResetJumpCount() { _currentJumpCount = _maxJumpCount; }

    public void AddJump() { _currentJumpCount++; }

    public void ConsumeJump() { _currentJumpCount--; }

    public bool CanJump() { return _currentJumpCount > 0 && !_isJumpOnCooldown; }
}
