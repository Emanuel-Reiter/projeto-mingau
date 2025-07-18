using UnityEngine;

public class PlayerMovement : MonoBehaviour {
    private Transform playerCamera;
    private PlayerMovementAttributes movementAttributes;
    private CharacterController characterController;

    // Rotation
    private float directionChangeThreshold = 135.0f;

    private void Start() {
        InitializeReferences();
    }

    private void Update() {
        ApplyMovement();
    }
    private void ApplyMovement() {
        Vector3 finalMovement = movementAttributes.horizontalVelocity + (Vector3.up * movementAttributes.verticalVelocity);
        characterController.Move(finalMovement * Time.deltaTime);
    }
    public void CalculateAcceleration(Vector3 targetVelocity, Vector3 moveDirection) {
        // Checks if there is a great change in the movement direction, and if there is, decelerate, else accelerate the body
        if (ClaculateGreatDifferenceInMovementDirection(moveDirection)) Decelerate();
        else Accelerate(targetVelocity);
    }

    public void Decelerate() {
        movementAttributes.SetHorizontalVelocity(
            Vector3.MoveTowards(movementAttributes.horizontalVelocity, Vector3.zero, movementAttributes.decelerationRate * Time.deltaTime));
    }

    public void Accelerate(Vector3 targetVelocity) {
        movementAttributes.SetHorizontalVelocity(
            Vector3.MoveTowards(movementAttributes.horizontalVelocity, targetVelocity, movementAttributes.accelerationRate * Time.deltaTime));
    }

    public bool ClaculateGreatDifferenceInMovementDirection(Vector3 moveDirection) {
        if (movementAttributes.horizontalVelocity.magnitude < Mathf.Epsilon) return false;

        float dotProduct = Vector3.Dot(movementAttributes.horizontalVelocity.normalized, moveDirection.normalized);
        float directionThreshold = Mathf.Cos(directionChangeThreshold * Mathf.Deg2Rad);
        return dotProduct < directionThreshold;
    }

    public Vector3 GetCameraRelativeDirection(Vector2 input) {
        // Calculate the facing vector of the camera
        Vector3 forward = playerCamera.forward;
        Vector3 right = playerCamera.right;
        forward.y = 0.0f;
        right.y = 0.0f;
        return (forward.normalized * input.y + right.normalized * input.x).normalized;
    }

    private void InitializeReferences() {
        try {
            // Object references
            playerCamera = GameObject.FindGameObjectWithTag("MainCamera").transform;
            movementAttributes = GetComponent<PlayerMovementAttributes>();
            characterController = GetComponent<CharacterController>();
        }
        catch {
            Debug.LogError("Some references were not assigned correctly.\nCheck external tag names and components assigned to this object.");
        }
    }
}
