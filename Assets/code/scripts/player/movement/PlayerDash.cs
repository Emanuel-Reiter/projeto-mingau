using UnityEngine;

public class PlayerDash : MonoBehaviour {
    // Internal references
    private CharacterController characterController;
    private PlayerAttributes playerAttributes;
    private PlayerInputManager input;

    private void Start() {
        InitializeReferences();
    }

    private void InitializeReferences() {
        try {
            // Object references
            characterController = GetComponent<CharacterController>();
            playerAttributes = GetComponent<PlayerAttributes>();
            input = GetComponent<PlayerInputManager>();
        }
        catch {
            Debug.LogError("Some references were not assigned correctly.\nCheck external tag names and components assigned to this object.");
        }
    }

}
