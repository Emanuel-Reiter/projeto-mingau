using UnityEngine;

public abstract class NPCBaseState : MonoBehaviour
{
    public bool IsComplete { get; protected set; } = false;

    [Header("Exit params")]
    [SerializeField] private bool _hasExitTime = false;
    [SerializeField] private float _duration = 1.0f;

    [Header("Die state")]
    [SerializeField] protected NPCBaseState _dieState;

    private float _startTime;
    private float _currentTime => Time.time - _startTime;

    public abstract void CheckExitState(NPCStateManager npc);
    public abstract void EnterState(NPCStateManager npc);
    public abstract void UpdateState(NPCStateManager npc);
    public abstract void PhysicsUpdateState(NPCStateManager npc);
    public abstract void ExitState(NPCStateManager npc);

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