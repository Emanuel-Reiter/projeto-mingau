using UnityEngine;

public abstract class PlayerBaseState : MonoBehaviour {
    public bool IsComplete { get; protected set; } = false;

    [Header("Exit params")]
    [SerializeField] private bool _hasExitTime = false;
    [SerializeField] private float _duration = 1.0f;

    private float _startTime;
    private float _currentTime => Time.time - _startTime;

    public abstract void CheckExitState(PlayerStateManager player);
    public abstract void EnterState(PlayerStateManager player);
    public abstract void UpdateState(PlayerStateManager player);
    public abstract void PhysicsUpdateState(PlayerStateManager player);
    public abstract void ExitState(PlayerStateManager player);

    public void InitializeState()
    {
        _startTime = Time.time;
        IsComplete = false;
    }

    public bool CompletedExitTime()
    {
        return _hasExitTime && (_currentTime >= _duration);
    }

    public void SetDuration(float duration) { _duration = duration; }
}