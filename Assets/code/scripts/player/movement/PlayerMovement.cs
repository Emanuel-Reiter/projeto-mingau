using UnityEngine;

public class PlayerMovement : MonoBehaviour {
    private Transform playerCameraParent;
    private PlayerAttributes playerAttributes;
    private CharacterController characterController;

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
