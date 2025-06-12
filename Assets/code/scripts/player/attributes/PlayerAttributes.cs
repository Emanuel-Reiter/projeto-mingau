using UnityEngine;

public class PlayerAttributes : MonoBehaviour {
    // Movement attributes
    public Vector3 horizontalVelocity { get; private set; }
    public float verticalVelocity { get; private set; }

    // Movement speed
    private float baseSpeed = 10.0f;
    private float currentSpeed = 0.0f;

    private float runSpeedMultiplaier = 1.0f;
    private float airSpeedMultiplaier = 1.667f;
    private float dashSpeedMultiplaier = 2.5f;

    // Acceleration
    public float accelerationRate { get; private set; } = 40.0f;
    public float decelerationRate { get; private set; } = 40.0f;

    // Gravity
    private float defaultGravity = -50.0f;

    public float groundedGravityAcceleration = -9.81f;
    public float gravityAcceleration { get; private set; }

    // Jump
    public bool isJumping { get; private set; } = false;
    public float jumpHeight { get; private set; } = 4.75f;

    private float jumpCooldown = 0.067f; 
    public bool isJumpOnCooldown { get; private set; } = false;

    private int maxAmountOfJumps = 2;
    private int currentAmountOfJumps = 0;

    // Dash
    public bool isDashing { get; private set; } = false;
    private int maxAmountOfDashes = 1;
    private int currentAmountOfDashes = 0;

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
        timer.StartTimer(jumpCooldown, () => SetIsJumpOnCooldown(false));
    }

    private void SetIsJumpOnCooldown(bool isJumpOnCooldown) { this.isJumpOnCooldown = isJumpOnCooldown; }
    #endregion


    #region Dash
    public void SetIsDashing(bool isDashing) { this.isDashing = isDashing; }

    public int GetAmountOfDashesRemaining() { return currentAmountOfDashes; }

    public bool HaveReaminingDashes() { return currentAmountOfDashes > 0; }

    public void ResetAmountOfDashed() { currentAmountOfDashes = maxAmountOfDashes; }

    public void AddAmountOfDashes() { currentAmountOfDashes++; }

    public void DecreaseAmountOfDashes() { currentAmountOfDashes--; }
    #endregion
}
