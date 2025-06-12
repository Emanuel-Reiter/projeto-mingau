using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerHorizontalMovement : MonoBehaviour {

    // Internal references
    private PlayerAttributes playerAttributes;
    private PlayerInputManager input;
    private PlayerVerticalMovement verticalMovement;
    private PlayerMovement movement;

    // Rotation
    private float directionChangeThreshold = 135.0f;

    private void Start() {
        InitializeReferences();
    }

    public void CalculateHorizontalMovement() {
        Vector2 inputVector = input.movementDirection;
        Vector3 moveDirection = movement.GetCameraRelativeDirection(inputVector);

        if (inputVector.magnitude > 0.1f) {
            Vector3 targetVelocity = moveDirection * playerAttributes.GetCurrentSpeed(verticalMovement.GetSlopeRelativeIsGrounded());
            CalculateAcceleration(targetVelocity, moveDirection);
        }
        else {
            Decelerate();
        }
    }

    private void CalculateAcceleration(Vector3 targetVelocity, Vector3 moveDirection) {
        // Checks if there is a great change in the movement direction, and if there is, decelerate, else accelerate the body
        if (ClaculateGreatDifferenceInMovementDirection(moveDirection)) Decelerate();
        else Accelerate(targetVelocity);
    }

    private void Decelerate() {
        playerAttributes.SetHorizontalVelocity(
            Vector3.MoveTowards(playerAttributes.horizontalVelocity, Vector3.zero, playerAttributes.decelerationRate * Time.deltaTime));
    }

    private void Accelerate(Vector3 targetVelocity) {
        playerAttributes.SetHorizontalVelocity(
            Vector3.MoveTowards(playerAttributes.horizontalVelocity, targetVelocity, playerAttributes.accelerationRate * Time.deltaTime));
    }

    private bool ClaculateGreatDifferenceInMovementDirection(Vector3 moveDirection) {
        if (playerAttributes.horizontalVelocity.magnitude < Mathf.Epsilon) return false;

        float dotProduct = Vector3.Dot(playerAttributes.horizontalVelocity.normalized, moveDirection.normalized);
        float directionThreshold = Mathf.Cos(directionChangeThreshold * Mathf.Deg2Rad);
        return dotProduct < directionThreshold;
    }

    private void InitializeReferences() {
        try {
            // Object references
            playerAttributes = GetComponent<PlayerAttributes>();
            input = GetComponent<PlayerInputManager>();
            verticalMovement = GetComponent<PlayerVerticalMovement>();
            movement = GetComponent<PlayerMovement>();
        }
        catch {
            Debug.LogError("Some references were not assigned correctly.\nCheck external tag names and components assigned to this object.");
        }
    }
}