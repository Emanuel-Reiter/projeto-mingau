using UnityEngine;

public class PlayerJump : MonoBehaviour {

    private PlayerMovementAttributes playerAttributes;
    private PlayerVerticalMovement verticalMovement;

    private void Start() {
        InitializeReferences();
    }

    public void Jump() {
        playerAttributes.DecreaseAmountOfJumps();

        playerAttributes.TriggerJumpCooldown();
        playerAttributes.SetIsJumping(true);
        verticalMovement.ToggleGroundSnaping(false);

        // Resets player vertical velocity in order to enssure full controll of the height of the jump
        playerAttributes.SetVerticalVelocity(0.0f);

        float jumpVelocity = Mathf.Sqrt(-2.0f * playerAttributes.gravityAcceleration * playerAttributes.jumpHeight);
        playerAttributes.SetVerticalVelocity(jumpVelocity);
    }

    private void InitializeReferences() {
        try {
            // Object references
            playerAttributes = GetComponent<PlayerMovementAttributes>();
            verticalMovement = GetComponent<PlayerVerticalMovement>();
        }
        catch {
            Debug.LogError("Some references were not assigned correctly.\nCheck external tag names and components assigned to this object.");
        }
    }
}
