using System.Collections;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour {
    // Camera
    private Transform playerCameraParent;
    
    // External references
    private CharacterController characterController;
    private PlayerAttributes playerAttributes;
    private PlayerInputManager input;

    // Rotation
    private float directionChangeThreshold = 135.0f;

    private Transform playerGraphics;
    private float graphicsRotationSpeed = 20.0f;

    // Velocity
    public Vector3 horizontalVelocity { get; private set; }
    public float verticalVelocity { get; private set; }

    // Slope movement
    private float slopeRaycastDistance = 0.5f;
    private float groundSnapDistance = 0.25f;
    private float slopeForce = 5.0f;

    // Jump
    public bool isJumping { get; private set; } = false; 

    // Input
    private Vector2 inputVector;

    // Toggles
    private bool isGroundedMovementEnabled = false;
    private bool isGroundSnapingActive = false;

    private void Start() {
        AssignReferences();
    }

    private void Update() {
        // Restricts the movement input if the player is not currently in a valid movement state
        if (isGroundedMovementEnabled) inputVector = input.movementDirection;
        else inputVector = Vector2.zero;

        // General velocity calculations
        CalculateHorizontalMovement();
        CalculateVerticalMovement();

        // Calculate movenet when in slopes when active
        if(isGroundSnapingActive) ApplyGroundSnaping();

        // Apply final movement
        ApplyMovement();

        // Rotate the player graphics to match the movement direction
        RotateGraphics();
    }

    private void CalculateHorizontalMovement() {
        Vector2 inputVector = this.inputVector;
        Vector3 moveDirection = GetCameraRelativeDirection(inputVector);

        if (inputVector.magnitude > 0.1f) {
            Vector3 targetVelocity = moveDirection * playerAttributes.GetCurrentSpeed(input.isSprintPressed);
            CalculateAcceleration(targetVelocity, moveDirection);
        }
        else {
            Decelerate();
        }
    }

    private void CalculateVerticalMovement() {
        if (characterController.isGrounded && !isJumping) {
            verticalVelocity = playerAttributes.groundedGravityAcceleration;
            if (isGroundSnapingActive) CalculateSlopeMovement();
        }
        else {
            verticalVelocity += playerAttributes.gravityAcceleration * Time.deltaTime;
        }
    }

    private void CalculateSlopeMovement() {
        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, slopeRaycastDistance)) {
            float slopeAngle = Vector3.Angle(hit.normal, Vector3.up);
            if (slopeAngle > characterController.slopeLimit) {
                // Apply downward force along slope normal
                verticalVelocity += slopeForce * Mathf.Sin(slopeAngle * Mathf.Deg2Rad);
            }
        }
    }

    private void CalculateAcceleration(Vector3 targetVelocity, Vector3 moveDirection) {
        // Checks if there is a great change in the movement direction, and if there is, decelerate, else accelerate the body
        if (ClaculateGreatDifferenceInMovementDirection(moveDirection)) Decelerate();
        else Accelerate(targetVelocity);
    }

    private void Decelerate() {
        horizontalVelocity = Vector3.MoveTowards(horizontalVelocity, Vector3.zero, playerAttributes.decelerationRate * Time.deltaTime);
    }

    private void Accelerate(Vector3 targetVelocity) {
        horizontalVelocity = Vector3.MoveTowards(horizontalVelocity, targetVelocity, playerAttributes.accelerationRate * Time.deltaTime);
    }
    
    private void ApplyGroundSnaping() {
        if (!characterController.isGrounded) {
            if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, groundSnapDistance)) {
                characterController.Move(Vector3.down * hit.distance);
            }
        }
    }

    private void ApplyMovement() {
        Vector3 finalMovement = horizontalVelocity + (Vector3.up * verticalVelocity);
        characterController.Move(finalMovement * Time.deltaTime);
    }

    private Vector3 GetCameraRelativeDirection(Vector2 input) {
        // Calculate the facing vector of the camera
        Vector3 forward = playerCameraParent.forward;
        Vector3 right = playerCameraParent.right;
        forward.y = 0;
        right.y = 0;
        return (forward.normalized * input.y + right.normalized * input.x).normalized;
    }

    private bool ClaculateGreatDifferenceInMovementDirection(Vector3 moveDirection) {
        if (horizontalVelocity.magnitude < Mathf.Epsilon) return false;

        float dot = Vector3.Dot(horizontalVelocity.normalized, moveDirection.normalized);
        float threshold = Mathf.Cos(directionChangeThreshold * Mathf.Deg2Rad);
        return dot < threshold;
    }

    private void RotateGraphics() {
        if (horizontalVelocity.magnitude > 0.1f) {
            // Calculate target rotation based on movement direction
            Quaternion targetRotation = Quaternion.LookRotation(horizontalVelocity.normalized);

            // Smoothly interpolate rotation
            playerGraphics.rotation = Quaternion.Slerp(playerGraphics.rotation, targetRotation, graphicsRotationSpeed * Time.deltaTime );
        }
    }

    private void AssignReferences() {
        try {
            // External references
            playerCameraParent = GameObject.FindGameObjectWithTag("PlayerCameraParent").transform;
            playerGraphics = GameObject.FindGameObjectWithTag("PlayerGraphics").transform;

            // Object references
            characterController = GetComponent<CharacterController>();
            playerAttributes = GetComponent<PlayerAttributes>();
            input = GetComponent<PlayerInputManager>();
        }
        catch {
            Debug.LogError("Some references were not assigned correctly.\nCheck external tag names and components assigned to this object.");
        }
    }
   
    // Public access methods
    public void ApplyJump() {
        if (characterController.isGrounded) {
            SetIsJumping(true);
            ToggleGroundSnaping(false);

            // Resets player vertical velocity in order to assure full controll of the height of the jump
            verticalVelocity = 0.0f;

            float jumpVelocity = Mathf.Sqrt(-2.0f * playerAttributes.gravityAcceleration * (playerAttributes.jumpHeight * 1.25f));
            verticalVelocity = jumpVelocity;
        }
    }

    public void ToggleHorizontalMovementInput(bool toggle) { isGroundedMovementEnabled = toggle; }
    public void ToggleGroundSnaping(bool toggle) { isGroundSnapingActive = toggle; }
    public void SetIsJumping(bool isJumping) { this.isJumping = isJumping; }
}