using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerLocomotion : MonoBehaviour
{
    private PlayerDependencies _dependencies;
    private PlayerPhysics _physics;

    private Vector3 _horizontalVelocity = Vector3.zero;
    public Vector3 HorizontalVelocity => _horizontalVelocity;

    private float _verticalVelocity = 0.0f;
    public float VerticalVelocity => _verticalVelocity;

    private void Start()
    {
        _dependencies = GetComponent<PlayerDependencies>();
        _physics = GetComponent<PlayerPhysics>();
    }

    private void Update()
    {
        ApplyMovement();
    }

    private void ApplyMovement()
    {
        Vector3 finalMovement = (_horizontalVelocity) + (Vector3.up * _verticalVelocity);
        _dependencies.Controller.Move(finalMovement * Time.deltaTime);
    }


    #region Movement calculation
    public void CalculateHorizontalMovement()
    {
        Vector2 inputVector = _dependencies.Input.MovementDirectionInput;
        Vector3 moveDirection = GetCameraRelativeDirection(inputVector);

        if (inputVector.magnitude > 0.1f)
        {
            Vector3 targetVelocity = moveDirection * _dependencies.LocomotionParams.GetGroundedRelativeSpeed();
            HandleAcceleration(targetVelocity, moveDirection);
        }
        else
        {
            Decelerate(_dependencies.LocomotionParams.GroundedAccelerationRate);
        }
    }

    public void CalculateVerticalMovement()
    {
        if (_physics.IsGrounded && !_dependencies.Jump.IsJumping)
        {
            _verticalVelocity = _dependencies.LocomotionParams.GroundedGravityAcceleration;
            if (_physics.UseGroundSnapping) _physics.CalculateSlopeMovement();
        }
        else
        {
            _verticalVelocity = _verticalVelocity + (_dependencies.LocomotionParams.AerialGravityAcceleration * Time.deltaTime);
        }
    }

    public void CalculateRotation()
    {
        if (_horizontalVelocity.magnitude > 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(_horizontalVelocity.normalized);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, _dependencies.LocomotionParams.RotationSpeed * Time.deltaTime);
        }
    }

    public void SetHorizontalVelocity(Vector3 velocity)
    {
        _horizontalVelocity = velocity;
    }

    public void SetVerticalVelocity(float velocity)
    {
        _verticalVelocity = velocity;
    }
    #endregion

    #region Acceleration
    public void HandleAcceleration(Vector3 targetVelocity, Vector3 moveDirection)
    {
        if (ClaculateGreatDifferenceInMovementDirection(moveDirection)) Decelerate(_dependencies.LocomotionParams.GroundedAccelerationRate);
        else Accelerate(targetVelocity);
    }

    private void Accelerate(Vector3 targetVelocity)
    {
        _horizontalVelocity = (Vector3.MoveTowards(_horizontalVelocity, targetVelocity, _dependencies.LocomotionParams.GroundedAccelerationRate * Time.deltaTime));
    }

    public void Decelerate(float accelerationRate)
    {
        _horizontalVelocity = (Vector3.MoveTowards(_horizontalVelocity, Vector3.zero, accelerationRate * Time.deltaTime));
    }
    #endregion

    #region Helper methods
    public bool ClaculateGreatDifferenceInMovementDirection(Vector3 moveDirection)
    {
        if (_horizontalVelocity.magnitude < Mathf.Epsilon) return false;

        float dotProduct = Vector3.Dot(_horizontalVelocity.normalized, moveDirection.normalized);
        float directionThreshold = Mathf.Cos(_dependencies.LocomotionParams.DirectionChangeThreshold * Mathf.Deg2Rad);
        return dotProduct < directionThreshold;
    }

    public Vector3 GetCameraRelativeDirection(Vector2 input)
    {
        // Calculate the facing vector of the camera
        Vector3 forward = _dependencies.MainCamera.transform.forward;
        Vector3 right = _dependencies.MainCamera.transform.right;
        forward.y = 0.0f;
        right.y = 0.0f;
        return (forward.normalized * input.y + right.normalized * input.x).normalized;
    }
    #endregion
}
