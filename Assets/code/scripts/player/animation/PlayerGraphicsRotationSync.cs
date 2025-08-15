using UnityEngine;

public class PlayerGraphicsRotationSync : MonoBehaviour {

    // References
    private PlayerLocomotionParams movementAttributes;
    private Transform playerGraphics;

    // Rotation attributes
    private float graphicsRotationSpeed = 20.0f;

    private void Start() {
        InitializeReferences();
    }

    private void Update() {
        RotateGraphics();
    }

    private void RotateGraphics() {
        if (movementAttributes.horizontalVelocity.magnitude > 0.1f) {
            // Calculate target rotation based on movement direction
            Quaternion targetRotation = Quaternion.LookRotation(movementAttributes.horizontalVelocity.normalized);

            // Smoothly interpolate rotation
            playerGraphics.rotation = Quaternion.Slerp(playerGraphics.rotation, targetRotation, graphicsRotationSpeed * Time.deltaTime);
        }
    }

    private void InitializeReferences() {
        try {
            // External references
            playerGraphics = GameObject.FindGameObjectWithTag("PlayerGraphics").transform;

            // Object references
            movementAttributes = GetComponent<PlayerLocomotionParams>();
        }
        catch {
            Debug.LogError("Some references were not assigned correctly.\nCheck external tag names and components assigned to this object.");
        }
    }
}
