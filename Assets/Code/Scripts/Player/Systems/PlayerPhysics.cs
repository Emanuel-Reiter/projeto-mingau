using UnityEngine;

public class PlayerPhysics : MonoBehaviour
{
    private PlayerDependencies _dependencies;
    private PlayerLocomotion _locomotion;

    [Header("Params")]
    [SerializeField] private float _slopeDetectionsDistance = 0.25f;
    [SerializeField] private float _slopeCollisionRadiusOffset = 0.0005f;
    [SerializeField] private LayerMask _groundMask;

    private float _groundSnapDistance = 0.25f;
    private float _slopeForce = 5.0f;

    public bool UseGroundSnapping { get; private set; } = true;
    public bool IsGrounded { get; private set; }

    private void Start()
    {
        _dependencies = GetComponent<PlayerDependencies>();
        _locomotion = GetComponent<PlayerLocomotion>();
    }

    private void Update()
    {
        GroundCheck();

        if (UseGroundSnapping) ApplyGroundSnapping();
    }

    public void GroundCheck()
    {
        Vector3 checkPosition = transform.position + Vector3.down * _slopeDetectionsDistance;
        Physics.CheckSphere(checkPosition, (_dependencies.Controller.radius - _slopeCollisionRadiusOffset), _groundMask);
    }

    public void CalculateSlopeMovement()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, _slopeDetectionsDistance))
        {
            float slopeAngle = Vector3.Angle(hit.normal, Vector3.up);
            if (slopeAngle > _dependencies.Controller.slopeLimit)
            {
                // Apply downward force along slope normal
                float velocity = _locomotion.VerticalVelocity + _slopeForce * Mathf.Sin(slopeAngle * Mathf.Deg2Rad);
                _dependencies.Controller.Move(Vector3.down * velocity);
            }
        }
    }

    private void ApplyGroundSnapping()
    {
        //if (IsGrounded) return;

        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, _groundSnapDistance))
        {
            _dependencies.Controller.Move(Vector3.down * hit.distance);
        }
    }

    public void ToggleGroundSnaping(bool toggle) { UseGroundSnapping = toggle; }
}
