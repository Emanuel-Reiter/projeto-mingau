using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputManager : MonoBehaviour {

    // Input Actions
    private PlayerInputActions playerInputActions;

    // Movement
    public Vector2 movementDirection { get; private set; } = Vector2.zero;
    public bool isJumpPressed { get; private set; } = false;
    public bool isSprintPressed { get; private set; } = false;
    public bool isDashPressed { get; private set; } = false;

    // Camera
    public Vector2 cameraLookDirection { get; private set; } = Vector2.zero;

    // Attack
    public bool isAttackLightPressed { get; private set; } = false;

    // Other
    public bool isInteractPressed { get; private set; } = false;

    private void Awake() {
        playerInputActions = new PlayerInputActions();
        playerInputActions.Default.Enable();
        SubscribeToAllActions();
    }

    private void Update() {
        ProcessMovementDirectionInput();
        ProcessCameraDirectionInput();
    }

    private void SubscribeToAllActions() {
        // Jump
        playerInputActions.Default.Jump.started += ProcessPerformedJumpInput;
        playerInputActions.Default.Jump.canceled += ProcessCanceledJumpInput;

        // Sprint (Hold)
        playerInputActions.Default.Sprint.performed += ProcessPerformedSprintInput;
        playerInputActions.Default.Sprint.canceled += ProcessCanceledSprintInput;

        // Dash
        playerInputActions.Default.Dash.started += ProcessPerformedDashInput;
        playerInputActions.Default.Dash.canceled += ProcessCanceledDashInput;

        // Attack Light
        playerInputActions.Default.AttackLight.started += ProcessPerformedAttackLightInput;
        playerInputActions.Default.AttackLight.canceled += ProcessCanceledAttackLightInput;

        // Interact
        playerInputActions.Default.Interact.started += ProcessPerformedInteractInput;
        playerInputActions.Default.Interact.canceled += ProcessCanceledInteractInput;
    }

    // Jump
    private void ProcessPerformedJumpInput(InputAction.CallbackContext context) { isJumpPressed = true; }
    private void ProcessCanceledJumpInput(InputAction.CallbackContext context) { isJumpPressed = false; }

    // Sprint 
    private void ProcessPerformedSprintInput(InputAction.CallbackContext context) { isSprintPressed = true; }
    private void ProcessCanceledSprintInput(InputAction.CallbackContext context) { isSprintPressed = false; }

    // Dash
    private void ProcessPerformedDashInput(InputAction.CallbackContext context) { isDashPressed = true; }
    private void ProcessCanceledDashInput(InputAction.CallbackContext context) { isDashPressed = false; }

    // Attack Light
    private void ProcessPerformedAttackLightInput(InputAction.CallbackContext context) { isAttackLightPressed = true; }
    private void ProcessCanceledAttackLightInput(InputAction.CallbackContext context) { isAttackLightPressed = false; }

    // Interact
    private void ProcessPerformedInteractInput(InputAction.CallbackContext context) { isInteractPressed = true; }
    private void ProcessCanceledInteractInput(InputAction.CallbackContext context) { isInteractPressed = false; }

    // Movement Direction
    private void ProcessMovementDirectionInput() {
        movementDirection = playerInputActions.Default.Movement.ReadValue<Vector2>();
    }

    // Camera Look
    private void ProcessCameraDirectionInput() {
        cameraLookDirection = playerInputActions.Default.CameraLook.ReadValue<Vector2>();
    }

}
