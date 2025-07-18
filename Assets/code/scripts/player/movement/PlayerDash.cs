using UnityEngine;
using UnityEngine.Windows;

public class PlayerDash : MonoBehaviour {
    // Internal references
    private PlayerMovementAttributes movementAttributes;
    private PlayerAnimationManager animationManager;
    private PlayerInputManager input;
    private PlayerMovement movement;
    private PlayerVerticalMovement verticalMovement;
    private ActionOnTimer timer;

    public float dashSpeed { get; private set; } = 48.0f;
    public bool isDashing { get; private set; } = false;
    private int maxAmountOfDashes = 1;
    private int currentAmountOfDashes = 0;

    private void Start() {
        InitializeReferences();
    }

    public void Dash() {
        SetIsDashing(true);

        float dashVelocity = Mathf.Sqrt(2.0f * movementAttributes.GetCurrentSpeed(verticalMovement.GetSlopeRelativeIsGrounded()) * dashSpeed);
        Debug.Log(dashVelocity); 

        Vector2 inputVector = input.movementDirection;
        Vector3 dashDirection = movement.GetCameraRelativeDirection(inputVector);

        movementAttributes.SetHorizontalVelocity(dashDirection * dashVelocity);

        float dashDuration = 0.0f;
        if (verticalMovement.GetSlopeRelativeIsGrounded()) dashDuration = animationManager.dash_01_anim.length;
        else dashDuration = animationManager.dash_01_anim.length / 2.0f;

        int dashEndTimer = timer.StartTimer(dashDuration, () => { SetIsDashing(false); });
    }

    #region Utility methods
    public void SetIsDashing(bool isDashing) { this.isDashing = isDashing; }

    public int GetAmountOfDashesRemaining() { return currentAmountOfDashes; }

    public bool HaveReaminingDashes() { return currentAmountOfDashes > 0; }

    public void ResetAmountOfDashes() { currentAmountOfDashes = maxAmountOfDashes; }

    public void AddAmountOfDashes() { currentAmountOfDashes++; }

    public void DecreaseAmountOfDashes() { currentAmountOfDashes--; }
    #endregion

    private void InitializeReferences() {
        try {
            // Object references
            movementAttributes = GetComponent<PlayerMovementAttributes>();
            animationManager = GetComponent<PlayerAnimationManager>();
            movement = GetComponent<PlayerMovement>();
            input = GetComponent<PlayerInputManager>();
            verticalMovement = GetComponent<PlayerVerticalMovement>();
            timer = GameObject.FindGameObjectWithTag("GlobalTimer").GetComponent<ActionOnTimer>();
        }
        catch {
            Debug.LogError("Some references were not assigned correctly.\nCheck external tag names and components assigned to this object.");
        }
    }

}
