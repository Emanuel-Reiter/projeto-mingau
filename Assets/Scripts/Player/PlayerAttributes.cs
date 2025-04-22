using UnityEngine;

public class PlayerAttributes : MonoBehaviour {
    // Movement speed
    private float baseSpeed = 10.0f;
    private float currentSpeed = 0.0f;

    // Acceleration
    public float accelerationRate { get; private set; } = 20.0f;
    public float decelerationRate { get; private set; } = 20.0f;

    // Gravity
    private float defaultGravity = -50.0f;

    public float groundedGravityAcceleration = -5.0f;
    public float gravityAcceleration { get; private set; }

    // Jump
    public float jumpHeight { get; private set; } = 2.0f;

    public int maxAirJumps { get; private set; } = 0;
    public int airJumpsLeft { get; private set; } = 0;

    private void Start() {
        // Attributes setup
        currentSpeed = baseSpeed;
        gravityAcceleration = defaultGravity;
        ResetAirJumps();
    }

    public float GetCurrentSpeed(bool sprintInput) {
        return currentSpeed;
    }

    public void ResetAirJumps() { airJumpsLeft = maxAirJumps; }
    public void DecreaseAirJumps() { airJumpsLeft--; }
}
