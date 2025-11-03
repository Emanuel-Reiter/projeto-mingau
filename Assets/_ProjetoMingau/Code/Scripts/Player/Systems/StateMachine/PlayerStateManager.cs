using UnityEngine;
using System;

public class PlayerStateManager : MonoBehaviour
{
    // Player states
    public PlayerIdleState IdleState = new PlayerIdleState();
    public PlayerRunState RunState = new PlayerRunState();
    public PlayerDashState DashState = new PlayerDashState();
    public PlayerJumpState JumpState = new PlayerJumpState();
    public PlayerFallState FallState = new PlayerFallState();
    public PlayerLandHeavyState LandHeavyState = new PlayerLandHeavyState();

    // State management
    public PlayerBaseState CurrentState { get; private set; }
    public PlayerBaseState PreviousState { get; private set; }

    public Type CurrentStateType => CurrentState?.GetType();
    public Type PreviousStateType => PreviousState?.GetType();

    // Dependencies
    private PlayerDependencies _dependencies;
    public PlayerDependencies Dependencies => _dependencies;

    private PlayerLocomotion _locomotion;
    public PlayerLocomotion Locomotion => _locomotion;

    private void Start()
    {
        _dependencies = GetComponent<PlayerDependencies>();
        _locomotion = GetComponent<PlayerLocomotion>();

        // Set the initial state
        SwitchState(IdleState);
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

    public bool WasPreviousState<T>() where T : PlayerBaseState { return PreviousState is T; }

    public bool IsCurrentState<T>() where T : PlayerBaseState { return CurrentState is T; }
}