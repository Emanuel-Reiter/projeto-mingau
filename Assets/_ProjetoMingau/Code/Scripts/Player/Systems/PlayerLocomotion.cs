using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerLocomotion : MonoBehaviour
{
    private PlayerDependencies _dependencies;

    public bool IsGrounded { get; private set; } = false;
    public bool OnSteepSlope { get; private set; } = false;

    private Vector3 _horizontalVelocity = Vector3.zero;
    public Vector3 HorizontalVelocity => _horizontalVelocity;

    private float _verticalVelocity = 0.0f;
    public float VerticalVelocity => _verticalVelocity;

    private Vector3 _slopeVelocity = Vector3.zero;
    public Vector3 SlopeVelocity => _slopeVelocity;

    // Ground
    [Header("Params")]
    [SerializeField] private LayerMask _groundMask;
    [SerializeField] private float _checkDistance = 0.067f;

    private Vector3 _groundNormal = Vector3.zero;
    public float GroundAngle { get; private set; } = 0.0f;

    private void Start()
    {
        _dependencies = GetComponent<PlayerDependencies>();
    }

    private void Update()
    {
        ApplyMovement();

        GroundCheck();
        CalculateGroundAngle();
    }

    #region Movement
    private void ApplyMovement()
    {
        Vector3 verticalVelocityVector = new Vector3(0.0f, _verticalVelocity, 0.0f);
        _dependencies.Controller.Move((_horizontalVelocity + verticalVelocityVector) * Time.deltaTime);
    }

    public void CalculateHorizontalVelocity()
    {
        Vector2 inputVector = _dependencies.Input.MovementDirectionInput;
        Vector3 movementDirection = GetCameraRelativeDirection(inputVector);

        if (inputVector != Vector2.zero)
        {
            Vector3 targetVelocity = movementDirection * GetCurrentMovementSpeed();
            HandleAcceleration(targetVelocity, movementDirection);
        }
        else
        {
            Decelerate();
        }
    }
    public void SetHorizontalVelocity(Vector3 velocity)
    {
        _horizontalVelocity = velocity;
    }

    public void CalculateVerticalVelocity()
    {
        if (_dependencies.Jump.IsGravityDisabled) return;

        if (IsGrounded)
        {
            _verticalVelocity = _dependencies.LocomotionParams.GroundedGravity;
            return;
        }

        _verticalVelocity += _dependencies.LocomotionParams.Gravity * Time.deltaTime;

        float targetMin = _dependencies.LocomotionParams.MaxVerticalVelocity * -1.0f;
        float targetMax = _dependencies.LocomotionParams.MaxVerticalVelocity;
        _verticalVelocity = Mathf.Clamp(_verticalVelocity, targetMin, targetMax);
    }

    public void SetVerticalVelocity(float velocity)
    {
        _verticalVelocity = velocity;
    }
    #endregion

    #region Slopes
    public void CalculateSlopeVelocity()
    {
        if (!IsGrounded || !OnSteepSlope || _dependencies.Jump.IsJumping)
        {
            _slopeVelocity = Vector3.zero;
            return;
        }

        Vector3 slopeDirection = Vector3.ProjectOnPlane(Vector3.down, _groundNormal).normalized;

        Debug.DrawRay(transform.position, slopeDirection, Color.magenta);

        float slopeVelocityMultiplaier = GetAngleVelocityMultiplaier();

        Vector3 targetSlopeVelocity = slopeDirection * _dependencies.LocomotionParams.MaxSlopeVelocity * slopeVelocityMultiplaier;
        _slopeVelocity = Vector3.MoveTowards(_slopeVelocity, targetSlopeVelocity, (_dependencies.LocomotionParams.SlopeAcceleration * slopeVelocityMultiplaier) * Time.deltaTime);

        _dependencies.Controller.Move(_slopeVelocity * Time.deltaTime);
    }

    private float GetAngleVelocityMultiplaier()
    {
        float minAngle = _dependencies.Controller.slopeLimit;
        float maxAngle = 90.0f;
        float gravityInfluence = _dependencies.LocomotionParams.Gravity * -1.0f;

        float normalized = (GroundAngle - minAngle) / (maxAngle - minAngle);
        float mappedValue = 1.0f + normalized * (gravityInfluence - 1.0f);

        return mappedValue;
    }
    #endregion

    #region Acceleration
    public void HandleAcceleration(Vector3 targetVelocity, Vector3 moveDirection)
    {
        if (ClaculateGreatDifferenceInMovementDirection(moveDirection)) Decelerate();
        else Accelerate(targetVelocity);
    }

    private void Accelerate(Vector3 targetVelocity)
    {
        _horizontalVelocity = Vector3.MoveTowards(_horizontalVelocity, targetVelocity, GetCurrentMovementAcceleration() * Time.deltaTime);
    }

    public void Decelerate()
    {
        _horizontalVelocity = Vector3.MoveTowards(_horizontalVelocity, Vector3.zero, GetCurrentMovementAcceleration() * Time.deltaTime);
    }
    #endregion

    #region Rotation
    public void RotateTowardsMovementDirection()
    {
        if (_horizontalVelocity == Vector3.zero) return;

        Quaternion targetRotation = Quaternion.LookRotation(_horizontalVelocity);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, _dependencies.LocomotionParams.RotationSpeed * Time.deltaTime);
    }

    public void RotateTowardsInputDirection()
    {
        if (_dependencies.Input.MovementDirectionInput == Vector2.zero) return;

        Vector3 input = GetCameraRelativeDirection(_dependencies.Input.MovementDirectionInput);
        Quaternion targetRotation = Quaternion.LookRotation(input);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, _dependencies.LocomotionParams.RotationSpeed * Time.deltaTime);
    }

    public void RotateTowardsInputDirection(float rotationSpeed)
    {
        if (_dependencies.Input.MovementDirectionInput == Vector2.zero) return;

        Vector3 input = GetCameraRelativeDirection(_dependencies.Input.MovementDirectionInput);
        Quaternion targetRotation = Quaternion.LookRotation(input);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }
    #endregion

    #region Helper
    public float GetCurrentMovementSpeed()
    {
        return IsGrounded ? _dependencies.LocomotionParams.GroundedMovementSpeed : _dependencies.LocomotionParams.AerialMovementSpeed;
    }

    public float GetCurrentMovementAcceleration()
    {
        return IsGrounded ? _dependencies.LocomotionParams.GroundedMovementAcceleration : _dependencies.LocomotionParams.AerialMovementAcceleration;
    }

    private void GroundCheck()
    {
        if (_dependencies.Jump.IsJumping)
        {
            IsGrounded = false;
            return;
        }

        float radius = _dependencies.Controller.radius;
        Vector3 origin = new Vector3(transform.position.x, transform.position.y + radius, transform.position.z);
        Vector3 direction = Vector3.down;

        RaycastHit hit;
        bool hitSurface = Physics.SphereCast(origin, radius, direction, out hit, _checkDistance, _groundMask);
        if (hitSurface)
        {
            IsGrounded = true;
            _groundNormal = hit.normal;
            return;
        }
        else
        {
            IsGrounded = false;
            _groundNormal = Vector3.zero;
            return;
        }
    }

    private void CalculateGroundAngle()
    {
        if (_dependencies.Jump.IsJumping)
        {
            OnSteepSlope = false;
            return;
        }

        GroundAngle = Vector3.Angle(_groundNormal, Vector3.up);

        if (GroundAngle > _dependencies.Controller.slopeLimit) OnSteepSlope = true;
        else OnSteepSlope = false;
    }

    public bool ClaculateGreatDifferenceInMovementDirection(Vector3 moveDirection)
    {
        if (_horizontalVelocity.magnitude < Mathf.Epsilon) return false;

        float dotProduct = Vector3.Dot(_horizontalVelocity.normalized, moveDirection.normalized);
        float directionThreshold = Mathf.Cos(_dependencies.LocomotionParams.DirectionChangeThreshold * Mathf.Deg2Rad);
        return dotProduct < directionThreshold;
    }

    public Vector3 GetCameraRelativeDirection(Vector2 input)
    {
        Vector3 forward = _dependencies.MainCamera.transform.forward;
        Vector3 right = _dependencies.MainCamera.transform.right;
        forward.y = 0.0f;
        right.y = 0.0f;
        return (forward.normalized * input.y + right.normalized * input.x).normalized;
    }
    #endregion

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (!Application.isPlaying) return;

        float radius = _dependencies.Controller.radius;
        Vector3 originStart = new Vector3(transform.position.x, transform.position.y + radius, transform.position.z);
        Vector3 originEnd = new Vector3(transform.position.x, transform.position.y + radius, transform.position.z) + Vector3.down * _checkDistance;

        if (IsGrounded) Gizmos.color = Color.green;
        else Gizmos.color = Color.red;

        Gizmos.DrawSphere(originStart, radius);
        Gizmos.DrawWireSphere(originEnd, radius);
    }
#endif
}
