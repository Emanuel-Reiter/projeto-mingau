using System.Collections;
using UnityEngine;

public class NPCTargetChaseState : NPCBaseState
{
    [Header("State tranisitions")]
    [SerializeField] private NPCAttackState _attackState;
    [SerializeField] private NPCReturnToSpawnState _returnToSpawnState;

    [Header("State params")]
    [SerializeField] private float _destinationRecalculationCooldown = 0.333f;
    [SerializeField] private float _targetMemoryDuration = 1.0f;
    [SerializeField] private float _targetStopingDistance = 1.8f;
    [SerializeField] private AnimationClip _movementAnim;
    [SerializeField] private float _interpolationTime = 0.05f;

    public override void CheckExitState(NPCStateManager npc)
    {
        bool isTargetInAtcionRange = npc.Dependencies.TargetDetection.GetDistanceFromTarget() <= npc.Dependencies.NavMeshAgent.stoppingDistance;
        if(isTargetInAtcionRange)
        {
            npc.SwitchState(_attackState);
            return;
        }

        bool isTargetInSight = npc.Dependencies.TargetDetection.HaveTarget();
        if(!isTargetInSight)
        {
            npc.SwitchState(_returnToSpawnState);
            return;
        }
    }

    public override void EnterState(NPCStateManager npc)
    {
        SetTargetStopingDistance(npc, _targetStopingDistance);

        StartCoroutine(RecalculateDestinationCoroutine(_destinationRecalculationCooldown, npc));
        npc.Dependencies.NavMeshAgent.SetDestination(npc.Dependencies.TargetDetection.CurrentTarget.position);

        npc.Dependencies.Animation.PlayAnimationInterpolated(_movementAnim, _interpolationTime);
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
        //SetTargetStopingDistance(npc, 0.0f);
    }

    private void SetTargetStopingDistance(NPCStateManager npc, float stopingDistance)
    {
        npc.Dependencies.NavMeshAgent.stoppingDistance = stopingDistance;
    }

    private IEnumerator RecalculateDestinationCoroutine(float delay, NPCStateManager npc)
    {
        while (true)
        {
            yield return new WaitForSeconds(delay);

            if (npc.Dependencies.TargetDetection.CurrentTarget == null) break;
            else npc.Dependencies.NavMeshAgent.SetDestination(npc.Dependencies.TargetDetection.CurrentTarget.position);
        }
    }
}
