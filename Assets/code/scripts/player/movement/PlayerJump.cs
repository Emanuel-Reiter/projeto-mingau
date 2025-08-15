using UnityEngine;

public class PlayerJump : MonoBehaviour
{

    private PlayerLocomotionParams playerAttributes;
    private PlayerVerticalMovement verticalMovement;

    private void Start()
    {
        InitializeReferences();
    }

    public void Jump()
    {
        playerAttributes.DecreaseAmountOfJumps();

        playerAttributes.TriggerJumpCooldown();
        playerAttributes.SetIsJumping(true);
        verticalMovement.ToggleGroundSnaping(false);

        // Resets player vertical velocity in order to enssure full controll of the height of the jump
        playerAttributes.SetVerticalVelocity(0.0f);

        float jumpVelocity = Mathf.Sqrt(-2.0f * playerAttributes.AerialGravityAcceleration * playerAttributes.JumpHeight);
        playerAttributes.SetVerticalVelocity(jumpVelocity);
    }

    private void InitializeReferences()
    {
        try
        {
            // Object references
            playerAttributes = GetComponent<PlayerLocomotionParams>();
            verticalMovement = GetComponent<PlayerVerticalMovement>();
        }
        catch
        {
            Debug.LogError("Some references were not assigned correctly.\nCheck external tag names and components assigned to this object.");
        }
    }
}
