using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    // External references
    private CharacterController characterController;
    private PlayerAttributes playerAttributes;
    private PlayerInputManager input;

    // Internal declaration
    private Vector3 movementDirection = Vector3.zero;

    private float yVelocity = 0.0f;

    private void Start() {
        characterController = GetComponent<CharacterController>();
        playerAttributes = GetComponent<PlayerAttributes>();
        input = GetComponent<PlayerInputManager>();
    }

    public void Move() {
        
        movementDirection = new Vector3(input.movementDirection.x, yVelocity, input.movementDirection.y);

        if (!characterController.isGrounded) {
            yVelocity += playerAttributes.gravityAcceleration * Time.deltaTime;
        }
        else {
            yVelocity = 0.0f;
        }

        Vector3 movement = (movementDirection * playerAttributes.currentSpeed) * Time.deltaTime;
        characterController.Move(movement);
    }

    public void ChangeDirection() {
        // Calculates the new foward direction and converts from radians to degrees
        float targetRotation = Mathf.Atan2(input.movementDirection.x, input.movementDirection.y) * Mathf.Rad2Deg;
        
        // Applys the new rotation whith interplation
        this.transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle( transform.eulerAngles.y, targetRotation,
            ref playerAttributes.rotationInterpolationlocity, playerAttributes.rotationInterpolationTime);
    }
}



