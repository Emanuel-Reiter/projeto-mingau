using UnityEngine;

public class PlayerGraphicsRotationSync : MonoBehaviour {

    // References
    private PlayerAttributes playerAttributes;
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
        if (playerAttributes.horizontalVelocity.magnitude > 0.1f) {
            // Calculate target rotation based on movement direction
            Quaternion targetRotation = Quaternion.LookRotation(playerAttributes.horizontalVelocity.normalized);

            // Smoothly interpolate rotation
            playerGraphics.rotation = Quaternion.Slerp(playerGraphics.rotation, targetRotation, graphicsRotationSpeed * Time.deltaTime);
        }
    }

    private void InitializeReferences() {
        try {
            // External references
            playerGraphics = GameObject.FindGameObjectWithTag("PlayerGraphics").transform;

            // Object references
            playerAttributes = GetComponent<PlayerAttributes>();
        }
        catch {
            Debug.LogError("Some references were not assigned correctly.\nCheck external tag names and components assigned to this object.");
        }
    }
}
