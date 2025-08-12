using Unity.VisualScripting;
using UnityEngine;

public class PlayerStateManager : MonoBehaviour {

    // Player states
    public PlayerIdleState idleState = new PlayerIdleState();
    public PlayerRunState runState = new PlayerRunState();
    public PlayerJumpState jumpState = new PlayerJumpState();
    public PlayerFallState fallState = new PlayerFallState();
    public PlayerLandHeavyState landHeavyState = new PlayerLandHeavyState();
    public PlayerLandLightState landLightState = new PlayerLandLightState();
    public PlayerDashState dashState = new PlayerDashState();


    // State management
    public PlayerBaseState currentState { get; private set; }
    public PlayerBaseState previousState { get; private set; }

    // External references
    [HideInInspector] public PlayerInputManager input;
    [HideInInspector] public PlayerMovementAttributes movementAttributes;
    [HideInInspector] public PlayerAnimationManager animationManager;
    [HideInInspector] public CharacterController characterController;

    [HideInInspector] public PlayerMovement movement;
    [HideInInspector] public PlayerHorizontalMovement horizontalMovement;
    [HideInInspector] public PlayerVerticalMovement verticalMovement;

    [HideInInspector] public PlayerJump jump;
    [HideInInspector] public PlayerDash dash;

    [HideInInspector] public PlayerGraphicsRotationSync rotationSync;

    private void Start() {
        InitializeReferences();

        // Setups the default state implementation to avoid errors
        currentState = idleState;
        previousState = currentState;
    }

    private void Update() {
        currentState.CheckExitState(this);
        currentState.UpdateState(this);
    }

    private void FixedUpdate() {
        currentState.PhysicsUpdateState(this);
    }

    public void SwitchState(PlayerBaseState newState) {
        // Exists the current state
        currentState.ExitState(this); 
        previousState = currentState;

        // Eneter the new state
        currentState = newState;
        currentState.EnterState(this);
    }

    private void InitializeReferences() {
        try {
            // External references

            // Object references
            input = GetComponent<PlayerInputManager>();
            horizontalMovement = GetComponent<PlayerHorizontalMovement>();
            verticalMovement = GetComponent<PlayerVerticalMovement>();
            jump = GetComponent<PlayerJump>();
            dash = GetComponent<PlayerDash>();
            rotationSync = GetComponent<PlayerGraphicsRotationSync>();
            movementAttributes = GetComponent<PlayerMovementAttributes>();
            characterController = GetComponent<CharacterController>();
            animationManager = GetComponent<PlayerAnimationManager>();
            movement = GetComponent<PlayerMovement>();
        }
        catch {
            Debug.LogError("Some references were not assigned correctly.\nCheck external tag names and components assigned to this object.");
        }
    }
}
