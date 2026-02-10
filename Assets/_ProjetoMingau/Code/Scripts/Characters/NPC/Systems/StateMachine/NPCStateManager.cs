using UnityEngine;
using System;

public class NPCStateManager : MonoBehaviour
{

    // State management
    [SerializeField] private NPCBaseState _initialState;
    public NPCBaseState CurrentState { get; private set; }
    public NPCBaseState PreviousState { get; private set; }

    public Type CurrentStateType => CurrentState?.GetType();
    public Type PreviousStateType => PreviousState?.GetType();

    // Dependencies
    public NPCDependencies Deps { get; private set; }

    private void Start()
    {
        Deps = GetComponent<NPCDependencies>();

        // Set the initial state
        SwitchState(_initialState);
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

    public void SwitchState(NPCBaseState newState)
    {
        if (newState == null) return;

        CurrentState?.ExitState(this);
        PreviousState = CurrentState;

        CurrentState = newState;
        CurrentState.InitializeState();
        CurrentState?.EnterState(this);
    }

    public bool WasPreviousState<T>() where T : NPCBaseState { return PreviousState is T; }

    public bool IsCurrentState<T>() where T : NPCBaseState { return CurrentState is T; }

    private void OnDrawGizmos()
    {
#if UNITY_EDITOR
        if (!Application.isPlaying) return;
        if (CurrentState == null) return;

        string stateText = CurrentState?.ToString().Split(' ')[0];
        Vector3 textPos = new Vector3(
            transform.position.x,
            transform.position.y + 2.5f,
            transform.position.z
            );

        UnityEditor.Handles.Label(textPos, stateText + " ");
#endif
    }
}