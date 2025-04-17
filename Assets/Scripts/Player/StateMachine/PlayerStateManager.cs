using UnityEngine;

public class PlayerStateManager : MonoBehaviour {

    // Player states declarations
    public PlayerIdleState idleState = new PlayerIdleState();
    public PlayerRunState runState = new PlayerRunState();

    // State management declarations
    private PlayerBaseState currentState;
    private PlayerBaseState previousState;

    // External references
    [HideInInspector] public PlayerInputManager input;
    [HideInInspector] public PlayerMovement movement;

    private void Start() {
        InitializeReferences();

        // Setups the default state implementation to avoid errors
        currentState = idleState;
        previousState = currentState;

        idleState.EnterState(this);
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
        input = GetComponent<PlayerInputManager>();
        movement = GetComponent<PlayerMovement>();
    }
}
