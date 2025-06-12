using UnityEngine;

public class PlayerVerticalMovement : MonoBehaviour {

    // Internal references
    private PlayerAttributes playerAttributes;
    private CharacterController characterController;

    // Slope movement
    private float slopeRaycastDistance = 0.5f;
    private float groundSnapDistance = 0.25f;
    private float slopeForce = 5.0f;

    public bool isGroundSnapingActive { get; private set; } = false;


    private void Start()
    {
        InitializeReferences();
    }

    private void Update() {
        // When active calculate movenet when in slopes
        if (isGroundSnapingActive) ApplyGroundSnaping();
    }

    public void CalculateVerticalMovement() {
        if (characterController.isGrounded && !playerAttributes.isJumping) {
            playerAttributes.SetVerticalVelocity(playerAttributes.groundedGravityAcceleration);
            if (isGroundSnapingActive) CalculateSlopeMovement();
        }
        else {
            playerAttributes.SetVerticalVelocity(
                playerAttributes.verticalVelocity + playerAttributes.gravityAcceleration * Time.deltaTime);
        }
    }

    private void CalculateSlopeMovement() {
        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, slopeRaycastDistance)) {
            float slopeAngle = Vector3.Angle(hit.normal, Vector3.up);
            if (slopeAngle > characterController.slopeLimit) {
                // Apply downward force along slope normal
                playerAttributes.SetVerticalVelocity(
                    playerAttributes.verticalVelocity + slopeForce * Mathf.Sin(slopeAngle * Mathf.Deg2Rad));
            }
        }
    }

    private void ApplyGroundSnaping() {
        if (!characterController.isGrounded) {
            if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, groundSnapDistance)) {
                characterController.Move(Vector3.down * hit.distance);
            }
        }
    }

    private void InitializeReferences() {
        try {
            // Object references
            playerAttributes = GetComponent<PlayerAttributes>();
            characterController = GetComponent<CharacterController>();
        }
        catch {
            Debug.LogError("Some references were not assigned correctly.\nCheck external tag names and components assigned to this object.");
        }
    }

    public bool GetSlopeRelativeIsGrounded() {
        return Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, slopeRaycastDistance);
    }

    public void ToggleGroundSnaping(bool toggle) { isGroundSnapingActive = toggle; }
}
