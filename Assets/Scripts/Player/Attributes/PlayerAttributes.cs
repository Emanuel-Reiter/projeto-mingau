using System.Collections;
using UnityEngine;

public class PlayerAttributes : MonoBehaviour {
    // Movement speed
    private float baseSpeed = 10.0f;
    private float currentSpeed = 0.0f;

    private float runSpeedMultiplaier = 1.0f;

    private float airSpeedMultiplaier = 1.75f;

    // Acceleration
    public float accelerationRate { get; private set; } = 30.0f;
    public float decelerationRate { get; private set; } = 30.0f;

    // Gravity
    private float defaultGravity = -50.0f;

    public float groundedGravityAcceleration = -5.0f;
    public float gravityAcceleration { get; private set; }

    // Jump
    public float jumpHeight { get; private set; } = 3.25f;

    private float jumpCooldown = 0.067f; 
    public bool isJumpOnCooldown { get; private set; } = false;

    private int maxAirJumps = 1;
    private int airJumpsLeft = 0;

    // Timer
    private ActionOnTimer timer;

    private void Start() {
        // Attributes setup
        currentSpeed = baseSpeed;
        gravityAcceleration = defaultGravity;
        ResetAirJumps();

        timer = GameObject.FindGameObjectWithTag("GlobalTimer").GetComponent<ActionOnTimer>();
    }

    public float GetCurrentSpeed(bool isGrounded) {
        currentSpeed = baseSpeed * (isGrounded ? runSpeedMultiplaier : airSpeedMultiplaier);
        return currentSpeed;
    }

    public int GetAirJumpsLeft() {  return airJumpsLeft; }
    public bool haveAirJumpsLeft() { return airJumpsLeft > 0;}
    public void ResetAirJumps() { airJumpsLeft = maxAirJumps; }
    public void DecreaseAirJumps() { airJumpsLeft--; }

    public void TriggerJumpCooldown() { 
        SetIsJumpOnCooldown(true);
        timer.StartTimer(jumpCooldown, () => SetIsJumpOnCooldown(false));
    }

    private void SetIsJumpOnCooldown(bool isJumpOnCooldown) { this.isJumpOnCooldown = isJumpOnCooldown; }
}
