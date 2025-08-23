using UnityEngine;

public class PlayerLocomotionParams : MonoBehaviour
{
    [Header("Dependencies")]
    private PlayerPhysics _physics;

    [Header("Movement Speed")]
    [SerializeField] private float _baseSpeed = 7.0f;
    public float BaseSpeed => _baseSpeed;

    [SerializeField] private float _runSpeedMultiplier = 1.0f;
    [SerializeField] private float _airSpeedMultiplier = 1.8f;

    [Header("Acceleration")]
    [SerializeField] private float _groundedAccelerationRate = 35.0f;
    public float GroundedAccelerationRate => _groundedAccelerationRate;

    [SerializeField] private float _aerialAccelerationRate = 60.0f;
    public float AerialAccelerationRate => _aerialAccelerationRate;

    [SerializeField] private float _dashAccelerationRate = 20.0f;
    public float DashAccelerationRate => _aerialAccelerationRate;

    [Header("Gravity")]
    [SerializeField] private float _aerialGravityAcceleration = -50.0f;
    public float AerialGravityAcceleration => _aerialGravityAcceleration;

    [SerializeField] private float _groundedGravityAcceleration = -9.81f;
    public float GroundedGravityAcceleration => _groundedGravityAcceleration;

    [Header("Character Rotation")]
    [SerializeField] private float _rotationSpeed = 20.0f;
    public float RotationSpeed => _rotationSpeed;

    [SerializeField] private float _directionChangeThreshold = 135.0f;
    public float DirectionChangeThreshold => _directionChangeThreshold;

    private void Start()
    {
        _physics = GetComponent<PlayerPhysics>();
    }

    // Horizontal movement speed
    public float GetGroundedRelativeSpeed()
    {
        return _physics.IsGrounded ? _baseSpeed * _runSpeedMultiplier : _baseSpeed * _airSpeedMultiplier;
    }

    public float GetGroundedRelativeAcceleration()
    {
        return _physics.IsGrounded ? GroundedAccelerationRate : AerialAccelerationRate;
    }
}
