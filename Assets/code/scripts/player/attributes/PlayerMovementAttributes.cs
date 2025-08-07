using UnityEngine;

public class PlayerMovementAttributes : MonoBehaviour {
    // Movement attributes
    public Vector3 horizontalVelocity { get; private set; }
    public float verticalVelocity { get; private set; }

    // Movement speed
    private float baseSpeed = 7.0f;
    private float currentSpeed = 0.0f;

    private float runSpeedMultiplaier = 1.0f;
    private float airSpeedMultiplaier = 1.8f;

    // Acceleration
    public float accelerationRate { get; private set; } = 36.0f;
    public float decelerationRate { get; private set; } = 36.0f;

    // Gravity
    private float defaultGravity = -50.0f;

    public float groundedGravityAcceleration = -9.81f;
    public float gravityAcceleration { get; private set; }

    // Jump
    public bool isJumping { get; private set; } = false;
    public float jumpHeight { get; private set; } = 2.25f;

    private float jumpCooldown = 0.067f; 
    public bool isJumpOnCooldown { get; private set; } = false;

    private int maxAmountOfJumps = 2;
    private int currentAmountOfJumps = 0;



    // Timer
    private ActionOnTimer timer;

    private void Start() {
        // Attributes setup
        currentSpeed = baseSpeed;
        gravityAcceleration = defaultGravity;
        ResetAmountOfJumps();

        timer = GameObject.FindGameObjectWithTag("GlobalTimer").GetComponent<ActionOnTimer>();
    }

    // Horizontal movement speed
    public float GetCurrentSpeed(bool isGrounded) {
        currentSpeed = baseSpeed * (isGrounded ? runSpeedMultiplaier : airSpeedMultiplaier);
        return currentSpeed;
    }

    #region Velocity
    public void SetHorizontalVelocity(Vector3 horizontalVelocity) { this.horizontalVelocity = horizontalVelocity; }

    public void SetVerticalVelocity(float verticalVelocity) { this.verticalVelocity = verticalVelocity; }
    #endregion

    #region Jump
    public void SetIsJumping(bool isJumping) { this.isJumping = isJumping; }

    public int GetAmountOfJumpsRemaining() {  return currentAmountOfJumps; }

    public bool HaveRemainingJumps() { return currentAmountOfJumps > 0; }

    public void ResetAmountOfJumps() { currentAmountOfJumps = maxAmountOfJumps; }

    public void AddAmountOfJumps() { currentAmountOfJumps++; }

    public void DecreaseAmountOfJumps() { currentAmountOfJumps--; }

    public void TriggerJumpCooldown() { 
        SetIsJumpOnCooldown(true);
        int jumpCooldownTimer = timer.StartTimer(jumpCooldown, () => SetIsJumpOnCooldown(false));
    }

    private void SetIsJumpOnCooldown(bool isJumpOnCooldown) { this.isJumpOnCooldown = isJumpOnCooldown; }
    #endregion

}
