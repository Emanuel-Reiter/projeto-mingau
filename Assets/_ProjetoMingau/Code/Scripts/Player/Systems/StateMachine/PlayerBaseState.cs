using UnityEngine;

public abstract class PlayerBaseState : MonoBehaviour {
    public abstract void CheckExitState(PlayerStateManager player);
    public abstract void EnterState(PlayerStateManager player);
    public abstract void UpdateState(PlayerStateManager player);
    public abstract void PhysicsUpdateState(PlayerStateManager player);
    public abstract void ExitState(PlayerStateManager player);
}