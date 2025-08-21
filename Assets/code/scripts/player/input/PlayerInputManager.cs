using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputManager : MonoBehaviour
{
    private PlayerInputActions _playerInputActions;

    // Movement
    private Vector2 _movementDirectionInput = Vector2.zero;
    public Vector2 MovementDirectionInput => _movementDirectionInput;

    private bool _isJumpPressed = false;
    public bool IsJumpPressed => _isJumpPressed;

    private bool _isSprintHold = false;
    public bool SprintHold => _isSprintHold;


    private bool _isDashPressed = false;
    public bool IsDashPressed => _isDashPressed;

    // Camera
    private Vector2 _cameraLookInput = Vector2.zero;
    public Vector2 CameraLookInput => _cameraLookInput;

    // Attack
    private bool _isAttackLightPressed = false;
    public bool IsAttackLightPressed => _isAttackLightPressed;

    // Other
    private bool _isInteractPressed = false;
    public bool InteractPressed => _isInteractPressed;

    private void Awake()
    {
        _playerInputActions = new PlayerInputActions();
        _playerInputActions.Default.Enable();
        SubscribeToAllActions();
    }

    private void OnDestroy()
    {
        UnsubscribeFromAllActions();
        _playerInputActions?.Default.Disable();
        _playerInputActions?.Dispose();
    }

    private void Update()
    {
        ProcessMovementDirectionInput();
        ProcessCameraDirectionInput();
    }

    private void SubscribeToAllActions()
    {
        // Jump
        _playerInputActions.Default.Jump.started += ProcessPerformedJumpInput;
        _playerInputActions.Default.Jump.canceled += ProcessCanceledJumpInput;

        // Sprint (Hold)
        _playerInputActions.Default.Sprint.performed += ProcessPerformedSprintInput;
        _playerInputActions.Default.Sprint.canceled += ProcessCanceledSprintInput;

        // Dash
        _playerInputActions.Default.Dash.started += ProcessPerformedDashInput;
        _playerInputActions.Default.Dash.canceled += ProcessCanceledDashInput;

        // Attack Light
        _playerInputActions.Default.AttackLight.started += ProcessPerformedAttackLightInput;
        _playerInputActions.Default.AttackLight.canceled += ProcessCanceledAttackLightInput;

        // Interact
        _playerInputActions.Default.Interact.started += ProcessPerformedInteractInput;
        _playerInputActions.Default.Interact.canceled += ProcessCanceledInteractInput;
    }

    private void UnsubscribeFromAllActions()
    {
        // Jump
        _playerInputActions.Default.Jump.started -= ProcessPerformedJumpInput;
        _playerInputActions.Default.Jump.canceled -= ProcessCanceledJumpInput;

        // Sprint
        _playerInputActions.Default.Sprint.performed -= ProcessPerformedSprintInput;
        _playerInputActions.Default.Sprint.canceled -= ProcessCanceledSprintInput;

        // Dash
        _playerInputActions.Default.Dash.started -= ProcessPerformedDashInput;
        _playerInputActions.Default.Dash.canceled -= ProcessCanceledDashInput;

        // Attack Light
        _playerInputActions.Default.AttackLight.started -= ProcessPerformedAttackLightInput;
        _playerInputActions.Default.AttackLight.canceled -= ProcessCanceledAttackLightInput;

        // Interact
        _playerInputActions.Default.Interact.started -= ProcessPerformedInteractInput;
        _playerInputActions.Default.Interact.canceled -= ProcessCanceledInteractInput;
    }

    // Jump
    private void ProcessPerformedJumpInput(InputAction.CallbackContext context) { StartCoroutine(ProcessPerformedJumpInputCoroutine()); }
    private void ProcessCanceledJumpInput(InputAction.CallbackContext context) { _isJumpPressed = false; }

    // Sprint 
    private void ProcessPerformedSprintInput(InputAction.CallbackContext context) { _isSprintHold = true; }
    private void ProcessCanceledSprintInput(InputAction.CallbackContext context) { _isSprintHold = false; }

    // Dash
    private void ProcessPerformedDashInput(InputAction.CallbackContext context) { StartCoroutine(ProcessPerformedDashInputCoroutine()); }
    private void ProcessCanceledDashInput(InputAction.CallbackContext context) { _isDashPressed = false; }

    // Attack Light
    private void ProcessPerformedAttackLightInput(InputAction.CallbackContext context) { StartCoroutine(ProcessPerformedAttackLightInputtCoroutine()); }
    private void ProcessCanceledAttackLightInput(InputAction.CallbackContext context) { _isAttackLightPressed = false; }

    // Interact
    private void ProcessPerformedInteractInput(InputAction.CallbackContext context) { StartCoroutine(ProcessPerformedInteractInputCoroutine()); }
    private void ProcessCanceledInteractInput(InputAction.CallbackContext context) { _isInteractPressed = false; }

    // Movement Direction
    private void ProcessMovementDirectionInput()
    {
        _movementDirectionInput = _playerInputActions.Default.Movement.ReadValue<Vector2>();
    }

    // Camera Look
    private void ProcessCameraDirectionInput()
    {
        _cameraLookInput = _playerInputActions.Default.CameraLook.ReadValue<Vector2>();
    }

    // Jump
    private IEnumerator ProcessPerformedJumpInputCoroutine()
    {
        _isJumpPressed = true;
        yield return null;
        // Check if object still exists
        if (this != null)
        {
            _isJumpPressed = false;
        }
    }

    // Dash
    private IEnumerator ProcessPerformedDashInputCoroutine()
    {
        _isDashPressed = true;
        yield return null;
        if (this != null) _isDashPressed = false;
    }

    // Attack Light
    private IEnumerator ProcessPerformedAttackLightInputtCoroutine()
    {
        _isAttackLightPressed = true;
        yield return null;
        if (this != null) _isAttackLightPressed = false;
    }

    // Interact
    private IEnumerator ProcessPerformedInteractInputCoroutine()
    {
        _isInteractPressed = true;
        yield return null;
        if (this != null) _isInteractPressed = false;
    }
}
