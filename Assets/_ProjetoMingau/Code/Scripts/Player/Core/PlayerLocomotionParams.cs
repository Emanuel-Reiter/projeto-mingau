using UnityEngine;

public class PlayerLocomotionParams : MonoBehaviour
{
    [Header("Horizontal params")]
    [SerializeField] private float _groundedMovementSpeed = 8.0f;
    public float GroundedMovementSpeed => _groundedMovementSpeed;
    [SerializeField] private float _groundedMovementAcceleration = 30.0f;
    public float GroundedMovementAcceleration => _groundedMovementAcceleration;

    [SerializeField] private float _aerialMovementSpeed = 12.0f;
    public float AerialMovementSpeed => _aerialMovementSpeed;
    [SerializeField] private float _aerialMovementAcceleration = 45.0f;
    public float AerialMovementAcceleration => _aerialMovementAcceleration;

    [Header("Vertical params")]
    [SerializeField] private float _maxVerticalVelocity = 50.0f;
    public float MaxVerticalVelocity => _maxVerticalVelocity;
    [SerializeField] private float _gravity = -20.0f;
    public float Gravity => _gravity;
    [SerializeField] private float _groundedGravity = -10.0f;
    public float GroundedGravity => _groundedGravity;

    [Header("Slope params")]
    [SerializeField] private float _slopeAcceleration = 20.0f;
    public float SlopeAcceleration => _slopeAcceleration;
    [SerializeField] private float _maxSlopeVelocity = 50.0f;
    public float MaxSlopeVelocity => _maxSlopeVelocity;

    [Header("Rotation params")]
    [SerializeField] private float _rotationSpeed = 50.0f;
    public float RotationSpeed => _rotationSpeed;
    [SerializeField] private float _directionChangeThreshold = 135.0f;
    public float DirectionChangeThreshold => _directionChangeThreshold;
}
