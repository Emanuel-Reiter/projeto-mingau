using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.UI.Image;

public class PlayerPhysics : MonoBehaviour
{
    private PlayerDependencies _dependencies;
    private PlayerLocomotion _locomotion;

    [Header("Params")]
    [SerializeField] private float _slopeDetectionDistance = 0.85f;
    [SerializeField] private Vector3 _slopeDetectionOffset = Vector3.zero;
    [SerializeField] private float _slopeCollisionRadiusOffset = 0.0001f;
    [SerializeField] private LayerMask _slopeMask;
    [SerializeField] private LayerMask _unstableGroundMask;

    private float _groundSnapDistance = 0.25f;
    private float _slopeSlideForce = 5.0f;

    private float _slopeForceMultiplier = 3.0f;

    public bool UseGroundSnapping { get; private set; } = true;
    public bool IsGrounded { get; private set; }

    private bool _standingOnUnstableGround = false;
    public bool StandingOnUnstableGround => _standingOnUnstableGround;

    private Vector3 _groundNormal = Vector3.zero;
    public Vector3 GroundNormal => _groundNormal;

    private Vector3 _currentHitPoint = Vector3.zero;

    private void Start()
    {
        _dependencies = GetComponent<PlayerDependencies>();
        _locomotion = GetComponent<PlayerLocomotion>();
    }

    private void Update()
    {
        GroundCheck();
        UpdateGroundNormal();

        if (UseGroundSnapping) ApplyGroundSnapping();
    }

    public void GroundCheck()
    {
        bool onSteepSlope = GetOnSteepSlope();

        Vector3 origin = transform.position + _slopeDetectionOffset;
        float radius = _dependencies.Controller.radius - _slopeCollisionRadiusOffset;
        bool isGrounded = Physics.CheckSphere(origin, radius, _slopeMask);

        _standingOnUnstableGround = Physics.CheckSphere(origin, radius, _unstableGroundMask);

        if (isGrounded && !onSteepSlope && !_standingOnUnstableGround) IsGrounded = true;
        else IsGrounded = false;
    }

    public bool GetOnSteepSlope()
    {
        float groundAngle = Vector3.Angle(_groundNormal, Vector3.up);
        groundAngle = Mathf.CeilToInt(groundAngle);

        bool onSteepSlope = groundAngle > _dependencies.Controller.slopeLimit && groundAngle < 90.0f;
        return onSteepSlope;
    }

    private void UpdateGroundNormal()
    {
        Vector3 origin = _currentHitPoint + _slopeDetectionOffset;
        float distance = _slopeDetectionDistance;
        RaycastHit hit;

        Physics.Raycast(origin, Vector3.down, out hit, distance, _slopeMask);

        bool didHit = Physics.Raycast(origin, Vector3.down, out hit, distance, _slopeMask);
        Debug.DrawRay(origin, Vector3.down * distance, didHit ? Color.green : Color.red, 0.1f);

        _groundNormal = hit.normal;
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        // Gets the current character controller collision hit point
        _currentHitPoint = hit.point;
    }

    private void ApplyGroundSnapping()
    {
        if (!IsGrounded) return;

        Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, _groundSnapDistance);
        _dependencies.Controller.Move(Vector3.down * hit.distance);
    }

    public void CalculateSlopeMovement()
    {
        bool onSteepSlope = GetOnSteepSlope();
        if (!onSteepSlope) return;

        _slopeForceMultiplier = 3.0f;
        ProcessSlopeMovement();
    }

    public void CalculateUnstableGroundMovement()
    {
        _slopeForceMultiplier = 2.0f;
        ProcessSlopeMovement();
    }

    private void ProcessSlopeMovement()
    {
        Vector3 slopeDirection = Vector3.ProjectOnPlane(Vector3.down, _groundNormal).normalized;
        Vector3 slopeMovement = slopeDirection * (_slopeSlideForce * _slopeForceMultiplier);

        Vector3 splitHorizotanlVelociy = new Vector3(slopeMovement.x, 0.0f, slopeMovement.z);
        float splitVerticalVelocity = slopeMovement.y;

        _locomotion.SetHorizontalVelocity(splitHorizotanlVelociy);
        _locomotion.SetVerticalVelocity(splitVerticalVelocity);
    }

    public void ToggleGroundSnaping(bool toggle) { UseGroundSnapping = toggle; }

    private void OnDrawGizmos()
    {
        bool isApplicationRunning = Application.isPlaying;
        if (!isApplicationRunning) return;
        
        Vector3 origin = transform.position + _slopeDetectionOffset;
        float radius = _dependencies.Controller.radius - _slopeCollisionRadiusOffset;
        
        bool isGrounded = Physics.CheckSphere(origin, radius, _slopeMask);
        
        if (isGrounded) Gizmos.color = Color.green;
        else Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(origin, radius);
    }
}
