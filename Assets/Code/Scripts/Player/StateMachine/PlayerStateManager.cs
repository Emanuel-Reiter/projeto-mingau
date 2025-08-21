using UnityEngine;

public class PlayerStateManager : MonoBehaviour
{
    // Player states
    public PlayerIdleState IdleState = new PlayerIdleState();
    public PlayerRunState RunState = new PlayerRunState();
    public PlayerDashState DashState = new PlayerDashState();
    public PlayerJumpState JumpState = new PlayerJumpState();
    public PlayerFallState FallState = new PlayerFallState();
    public PlayerLandHeavyState LandHeavyState = new PlayerLandHeavyState();
    public PlayerLandLightState LandLightState = new PlayerLandLightState();

    // State management
    public PlayerBaseState CurrentState { get; private set; }
    public PlayerBaseState PreviousState { get; private set; }

    // Dependencies
    private PlayerDependencies _dependencies;
    public PlayerDependencies Dependencies => _dependencies;
    
    private PlayerPhysics _physics;
    public PlayerPhysics Physics => _physics;

    private PlayerLocomotion _locomotion;
    public PlayerLocomotion Locomotion => _locomotion;

    private void Awake()
    {
        CurrentState = IdleState;
        PreviousState = CurrentState;
    }

    private void Start()
    {
        _dependencies = GetComponent<PlayerDependencies>();
        _physics = GetComponent<PlayerPhysics>();
        _locomotion = GetComponent<PlayerLocomotion>();
    }

    private void Update()
    {
        CurrentState?.CheckExitState(this);
        CurrentState?.UpdateState(this);
    }

    private void FixedUpdate()
    {
        CurrentState?.PhysicsUpdateState(this);
    }

    public void SwitchState(PlayerBaseState newState)
    {
        if (newState == null) return;

        CurrentState?.ExitState(this);
        PreviousState = CurrentState;

        CurrentState = newState;
        CurrentState?.EnterState(this);
    }
}
