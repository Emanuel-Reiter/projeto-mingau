using UnityEngine;

public class PlayerPhysics : MonoBehaviour
{
    private PlayerDependencies _dependencies;
    private PlayerLocomotion _locomotion;

    [Header("Params")]
    [SerializeField] private float _slopeDetectionDistance = 0.25f;
    [SerializeField] private float _secondarySlopeDetectionDistance = 2.25f;
    [SerializeField] private float _slopeCollisionRadiusOffset = 0.0005f;
    [SerializeField] private LayerMask _groundMask;

    private float _groundSnapDistance = 0.25f;
    private float _slopeSlideForce = 5.0f;

    public bool UseGroundSnapping { get; private set; } = true;
    public bool IsGrounded { get; private set; }

    private Vector3 _currentGroundNormal = Vector3.zero;
    public Vector3 CurrentGroundNormal => _currentGroundNormal;

    private void Start()
    {
        _dependencies = GetComponent<PlayerDependencies>();
        _locomotion = GetComponent<PlayerLocomotion>();
    }

    private void Update()
    {
        GroundCheck();
        GetGroundNormal();

        if (UseGroundSnapping) ApplyGroundSnapping();
    }

    public void GroundCheck()
    {
        bool onSteepSlope = GetOnSteepSlope();
        
        Vector3 checkPosition = transform.position + Vector3.down * _slopeDetectionDistance;
        bool isGrounded = Physics.CheckSphere(checkPosition, (_dependencies.Controller.radius - _slopeCollisionRadiusOffset), _groundMask);

        if (isGrounded && !onSteepSlope) IsGrounded = true;
        else IsGrounded = false;
    }

    public void CalculateSlopeMovement()
    {
        Vector3 groundAngle = _currentGroundNormal;
        float slopeAngle = Vector3.Angle(groundAngle, Vector3.up);
        float slopeLimit = _dependencies.Controller.slopeLimit;

        bool onSteepSlope = slopeAngle > slopeLimit;
        if (!onSteepSlope) return;

        Vector3 slopeDirection = Vector3.ProjectOnPlane(Vector3.down, groundAngle).normalized;
        float forceMultiplier = 3.0f;

        Vector3 slopeMovement = slopeDirection * (_slopeSlideForce * forceMultiplier);

        Vector3 splitHorizotanlVelociy = new Vector3(slopeMovement.x, 0.0f, slopeMovement.z);
        float splitVerticalVelocity = slopeMovement.y;

        _locomotion.SetHorizontalVelocity(splitHorizotanlVelociy);
        _locomotion.SetVerticalVelocity(splitVerticalVelocity);
    }

    public bool GetOnSteepSlope()
    {
        float groundAngle = Vector3.Angle(_currentGroundNormal, Vector3.up);
        bool onSteepSlope = groundAngle > _dependencies.Controller.slopeLimit;
        return onSteepSlope;
    }

    private void GetGroundNormal()
    {
        Vector3 position = new Vector3(
            transform.position.x,
            transform.position.y + (_dependencies.Controller.height / 4.0f),
            transform.position.z);

        float radius = (_dependencies.Controller.radius - _slopeCollisionRadiusOffset);
        float maxDistance = _dependencies.Controller.height / 2.0f;

        RaycastHit hit;

        Physics.SphereCast(position, radius, Vector3.down, out hit, maxDistance, _groundMask);
        _currentGroundNormal = hit.normal;
    }

    private void ApplyGroundSnapping()
    {
        if (!IsGrounded) return;

        Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, _groundSnapDistance);
        _dependencies.Controller.Move(Vector3.down * hit.distance);
    }

    public void ToggleGroundSnaping(bool toggle) { UseGroundSnapping = toggle; }
}
