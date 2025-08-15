using UnityEngine;

public class PlayerLocomotionParams : MonoBehaviour
{
    // Movement attributes
    public Vector3 horizontalVelocity { get; private set; }
    public float verticalVelocity { get; private set; }

    [Header("Movement Speed")]
    private float baseSpeed = 7.0f;
    private float currentSpeed = 0.0f;

    [SerializeField] private float _runSpeedMultiplier = 1.0f;
    [SerializeField] private float _airSpeedMultiplier = 1.8f;

    [Header("Acceleration")]
    [SerializeField] private float _accelerationRate = 50.0f;
    public float AccelerationRate => _accelerationRate;

    [SerializeField] private float _decelerationRate = 50.0f;
    public float DecelerationRate => _decelerationRate;

    [Header("Gravity")]
    [SerializeField] private float _aerialGravityAcceleration = -50.0f;
    public float AerialGravityAcceleration => _aerialGravityAcceleration;

    [SerializeField] private float _groundedGravityAcceleration = -9.81f;
    public float GroundedGravityAcceleration => _groundedGravityAcceleration;

    [Header("Jump")]
    [SerializeField] private float _jumpHeight = 2.25f;
    public float JumpHeight => _jumpHeight;

    public bool isJumping { get; private set; } = false;

    private float jumpCooldown = 0.067f;
    public bool isJumpOnCooldown { get; private set; } = false;

    private int maxAmountOfJumps = 2;
    private int currentAmountOfJumps = 0;



    // Timer
    private GlobalTimer timer;

    private void Start()
    {
        // Attributes setup
        GetCurrentSpeed(true);
        ResetAmountOfJumps();

        timer = GameObject.FindGameObjectWithTag("GlobalTimer").GetComponent<GlobalTimer>();
    }

    // Horizontal movement speed
    public float GetCurrentSpeed(bool isGrounded)
    {
        currentSpeed = baseSpeed * (isGrounded ? _runSpeedMultiplier : _airSpeedMultiplier);
        return currentSpeed;
    }

    #region Velocity
    public void SetHorizontalVelocity(Vector3 horizontalVelocity) { this.horizontalVelocity = horizontalVelocity; }

    public void SetVerticalVelocity(float verticalVelocity) { this.verticalVelocity = verticalVelocity; }
    #endregion

    #region Jump
    public void SetIsJumping(bool isJumping) { this.isJumping = isJumping; }

    public int GetAmountOfJumpsRemaining() { return currentAmountOfJumps; }

    public bool HaveRemainingJumps() { return currentAmountOfJumps > 0; }

    public void ResetAmountOfJumps() { currentAmountOfJumps = maxAmountOfJumps; }

    public void AddAmountOfJumps() { currentAmountOfJumps++; }

    public void DecreaseAmountOfJumps() { currentAmountOfJumps--; }

    public void TriggerJumpCooldown()
    {
        SetIsJumpOnCooldown(true);
        int jumpCooldownTimer = timer.StartTimer(jumpCooldown, () => SetIsJumpOnCooldown(false));
    }

    private void SetIsJumpOnCooldown(bool isJumpOnCooldown) { this.isJumpOnCooldown = isJumpOnCooldown; }
    #endregion

}
