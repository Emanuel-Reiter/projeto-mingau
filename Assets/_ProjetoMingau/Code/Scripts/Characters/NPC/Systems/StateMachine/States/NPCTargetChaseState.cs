using System.Collections;
using UnityEngine;

public class NPCTargetChaseState : NPCBaseState
{
    [Header("State tranisitions")]
    [SerializeField] private NPCAttackState _attackState;
    [SerializeField] private NPCFlinchState _flinchState;
    [SerializeField] private NPCReturnToSpawnState _returnToSpawnState;

    [Header("State params")]
    [SerializeField] private float _destinationRecalculationCooldown = 0.333f;
    [SerializeField] private float _targetStopingDistance = 1.8f;
    [SerializeField] private AnimationClip _movementAnim;
    [SerializeField] private float _interpolationTime = 0.05f;

    public override void CheckExitState(NPCStateManager npc)
    {
        if (!npc.Deps.Attributes.IsAlive)
        {
            if (_dieState == null) return;

            npc.SwitchState(_dieState);
            return;
        }

        if (npc.Deps.Attributes.IsPostureBroken)
        {
            npc.SwitchState(_flinchState);
            return;
        }

        bool isTargetInAtcionRange = npc.Deps.TargetDetection.GetDistanceFromTarget() <= npc.Deps.NavMeshAgent.stoppingDistance;
        if (isTargetInAtcionRange)
        {
            npc.SwitchState(_attackState);
            return;
        }

        bool isTargetInSight = npc.Deps.TargetDetection.HaveTarget();
        if (!isTargetInSight)
        {
            npc.SwitchState(_returnToSpawnState);
            return;
        }
    }

    public override void EnterState(NPCStateManager npc)
    {
        SetTargetStopingDistance(npc, _targetStopingDistance);

        StartCoroutine(RecalculateDestinationCoroutine(_destinationRecalculationCooldown, npc));
        npc.Deps.NavMeshAgent.SetDestination(npc.Deps.TargetDetection.CurrentTarget.position);

        npc.Deps.Animation.PlayAnimationInterpolated(_movementAnim, _interpolationTime);
    }

    public override void UpdateState(NPCStateManager npc)
    {

    }

    public override void PhysicsUpdateState(NPCStateManager npc)
    {

    }

    public override void ExitState(NPCStateManager npc)
    {
        StopAllCoroutines();
    }

    private void SetTargetStopingDistance(NPCStateManager npc, float stopingDistance)
    {
        npc.Deps.NavMeshAgent.stoppingDistance = stopingDistance;
    }

    private IEnumerator RecalculateDestinationCoroutine(float delay, NPCStateManager npc)
    {
        while (true)
        {
            yield return new WaitForSeconds(delay);

            if (npc.Deps.TargetDetection.CurrentTarget == null) break;
            else npc.Deps.NavMeshAgent.SetDestination(npc.Deps.TargetDetection.CurrentTarget.position);
        }
    }
}
