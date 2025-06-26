using UnityEngine;

public class PlayerMovement : MonoBehaviour {
    private Transform playerCameraParent;
    private PlayerAttributes playerAttributes;
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
        Vector3 finalMovement = playerAttributes.horizontalVelocity + (Vector3.up * playerAttributes.verticalVelocity);
        characterController.Move(finalMovement * Time.deltaTime);
    }
    public void CalculateAcceleration(Vector3 targetVelocity, Vector3 moveDirection) {
        // Checks if there is a great change in the movement direction, and if there is, decelerate, else accelerate the body
        if (ClaculateGreatDifferenceInMovementDirection(moveDirection)) Decelerate();
        else Accelerate(targetVelocity);
    }

    public void Decelerate() {
        playerAttributes.SetHorizontalVelocity(
            Vector3.MoveTowards(playerAttributes.horizontalVelocity, Vector3.zero, playerAttributes.decelerationRate * Time.deltaTime));
    }

    public void Accelerate(Vector3 targetVelocity) {
        playerAttributes.SetHorizontalVelocity(
            Vector3.MoveTowards(playerAttributes.horizontalVelocity, targetVelocity, playerAttributes.accelerationRate * Time.deltaTime));
    }

    public bool ClaculateGreatDifferenceInMovementDirection(Vector3 moveDirection) {
        if (playerAttributes.horizontalVelocity.magnitude < Mathf.Epsilon) return false;

        float dotProduct = Vector3.Dot(playerAttributes.horizontalVelocity.normalized, moveDirection.normalized);
        float directionThreshold = Mathf.Cos(directionChangeThreshold * Mathf.Deg2Rad);
        return dotProduct < directionThreshold;
    }

    public Vector3 GetCameraRelativeDirection(Vector2 input) {
        // Calculate the facing vector of the camera
        Vector3 forward = playerCameraParent.forward;
        Vector3 right = playerCameraParent.right;
        forward.y = 0.0f;
        right.y = 0.0f;
        return (forward.normalized * input.y + right.normalized * input.x).normalized;
    }

    private void InitializeReferences() {
        try {
            // Object references
            playerCameraParent = GameObject.FindGameObjectWithTag("PlayerCameraParent").transform;
            playerAttributes = GetComponent<PlayerAttributes>();
            characterController = GetComponent<CharacterController>();
        }
        catch {
            Debug.LogError("Some references were not assigned correctly.\nCheck external tag names and components assigned to this object.");
        }
    }
}
