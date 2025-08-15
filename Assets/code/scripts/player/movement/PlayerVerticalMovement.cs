using UnityEngine;

public class PlayerVerticalMovement : MonoBehaviour
{

    // Internal references
    private PlayerLocomotionParams locomotionParams;
    private CharacterController characterController;

    // Slope movement
    private float slopeDetectionsDistance = 0.25f;
    private float slopeCollisionRadiusOffset = 0.001f;
    [SerializeField] private LayerMask groundMask;

    private float groundSnapDistance = 0.25f;
    private float slopeForce = 5.0f;

    public bool isGroundSnapingActive { get; private set; } = false;


    private void Start()
    {
        InitializeReferences();
    }

    private void Update()
    {
        // When active calculate movenet when in slopes
        if (isGroundSnapingActive) ApplyGroundSnaping();
    }

    public void CalculateVerticalMovement()
    {
        if (characterController.isGrounded && !locomotionParams.isJumping)
        {
            locomotionParams.SetVerticalVelocity(locomotionParams.GroundedGravityAcceleration);
            if (isGroundSnapingActive) CalculateSlopeMovement();
        }
        else
        {
            locomotionParams.SetVerticalVelocity(
                locomotionParams.verticalVelocity + locomotionParams.AerialGravityAcceleration * Time.deltaTime);
        }
    }

    private void CalculateSlopeMovement()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, slopeDetectionsDistance))
        {
            float slopeAngle = Vector3.Angle(hit.normal, Vector3.up);
            if (slopeAngle > characterController.slopeLimit)
            {
                // Apply downward force along slope normal
                locomotionParams.SetVerticalVelocity(
                    locomotionParams.verticalVelocity + slopeForce * Mathf.Sin(slopeAngle * Mathf.Deg2Rad));
            }
        }
    }

    private void ApplyGroundSnaping()
    {
        if (!characterController.isGrounded)
        {
            if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, groundSnapDistance))
            {
                characterController.Move(Vector3.down * hit.distance);
            }
        }
    }

    private void InitializeReferences()
    {
        try
        {
            // Object references
            locomotionParams = GetComponent<PlayerLocomotionParams>();
            characterController = GetComponent<CharacterController>();
        }
        catch
        {
            Debug.LogError("Some references were not assigned correctly.\nCheck external tag names and components assigned to this object.");
        }
    }

    public bool GetSlopeRelativeIsGrounded()
    {
        Vector3 checkPosition = transform.position + Vector3.down * slopeDetectionsDistance;
        return Physics.CheckSphere(checkPosition, (characterController.radius - slopeCollisionRadiusOffset), groundMask);
    }

    public void ToggleGroundSnaping(bool toggle) { isGroundSnapingActive = toggle; }
}
