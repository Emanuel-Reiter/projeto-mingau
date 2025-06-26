using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerHorizontalMovement : MonoBehaviour {

    // Internal references
    private PlayerAttributes playerAttributes;
    private PlayerInputManager input;
    private PlayerVerticalMovement verticalMovement;
    private PlayerMovement movement;



    private void Start() {
        InitializeReferences();
    }

    public void CalculateHorizontalMovement() {
        Vector2 inputVector = input.movementDirection;
        Vector3 moveDirection = movement.GetCameraRelativeDirection(inputVector);

        if (inputVector.magnitude > 0.1f) {
            Vector3 targetVelocity = moveDirection * playerAttributes.GetCurrentSpeed(verticalMovement.GetSlopeRelativeIsGrounded());
            movement.CalculateAcceleration(targetVelocity, moveDirection);
        }
        else {
            movement.Decelerate();
        }
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