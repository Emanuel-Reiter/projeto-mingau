using System.Collections;
using UnityEngine;

public class NPCTargetDetection : MonoBehaviour
{
    [SerializeField] private bool _isTargetSearchEnabled = false;

    [SerializeField] private float _viewRadius = 12.0f;
    [SerializeField][Range(0, 360)] private float _viewAngle = 90.0f;

    [SerializeField] private float _targetMemoryTimer = 2.0f;

    [SerializeField] private LayerMask _targetsMask;
    [SerializeField] private LayerMask _lowHitMask;

    [SerializeField] private Vector3 _viewOffset = Vector3.zero;

    public Transform CurrentTarget { get; private set; }

    private GlobalTimer _timer;
    private int _currentTimer = 0;

    private float _targetSearchDelay = 0.2f;

    private void Start()
    {
        ToggleTargetSearch(_isTargetSearchEnabled);
    }

    public void ToggleTargetSearch(bool toggle)
    {
        if (toggle)
        {
            _isTargetSearchEnabled = true;
            StartCoroutine(FindTargetsWithDelay(_targetSearchDelay));
        }
        else
        {
            _isTargetSearchEnabled = false;
            StopCoroutine(FindTargetsWithDelay(_targetSearchDelay));
            ForgetCurrentTarget();
        }
    }

    private IEnumerator FindTargetsWithDelay(float delay)
    {
        while (true)
        {
            yield return new WaitForSeconds(delay);
            if (CurrentTarget == null) SearchNewTarget();
            else LocateCurrentTarget();
        }
    }

    private void SearchNewTarget()
    {
        Vector3 adjustedPosition = transform.position + _viewOffset;

        Collider[] targetsInViewRadius = Physics.OverlapSphere(transform.position, _viewRadius, _targetsMask);

        foreach (Collider targetInViewRadius in targetsInViewRadius)
        {
            // TODO: This is a basic implementation. Needs to be reworked!!!
            bool isPlayer = targetInViewRadius.gameObject.CompareTag("Player");
            if (!isPlayer) continue;

            Transform target = targetInViewRadius.transform;
            Vector3 targetPosition = target.position + _viewOffset;
            Vector3 directionToTarget = (targetPosition - adjustedPosition).normalized;

            bool isTargetInFOV = Vector3.Angle(transform.forward, directionToTarget) < _viewAngle / 2.0f;
            if (!isTargetInFOV) continue;

            float distanceToTarget = Vector3.Distance(adjustedPosition, targetPosition);
            bool isViewObstructed = Physics.Raycast(adjustedPosition, directionToTarget, distanceToTarget, _lowHitMask);
            Debug.DrawRay(adjustedPosition, directionToTarget * distanceToTarget, Color.magenta, 0.1f);

            if (!isViewObstructed) CurrentTarget = target;
        }
    }

    private void LocateCurrentTarget()
    {
        bool hasLostTarget = false;
        Vector3 adjustedPosition = transform.position + _viewOffset;
        Vector3 targetPosition = CurrentTarget.position + _viewOffset;

        Vector3 directionToTarget = (targetPosition - adjustedPosition).normalized;

        bool isTargetInFOV = Vector3.Angle(transform.forward, directionToTarget) < _viewAngle / 2.0f;
        if (!isTargetInFOV)
        {
            hasLostTarget = true;
        }
        else
        {
            float distanceToTarget = Vector3.Distance(adjustedPosition, targetPosition);
            bool isViewObstructed = Physics.Raycast(adjustedPosition, directionToTarget, distanceToTarget, _lowHitMask);

            if (isViewObstructed) hasLostTarget = true;

            Debug.DrawRay(adjustedPosition, directionToTarget * distanceToTarget, Color.magenta, 0.1f);
        }

        if (hasLostTarget)
        {
            if (_currentTimer != 0) GlobalTimer.I.CancelTimer(_currentTimer);
            _currentTimer = GlobalTimer.I.StartTimer(_targetMemoryTimer, () => ForgetCurrentTarget());
        }
        else
        {
            if (_currentTimer != 0) GlobalTimer.I.CancelTimer(_currentTimer);
        }
    }



    private void ForgetCurrentTarget()
    {
        CurrentTarget = null;
    }

    public bool HaveTarget()
    {
        return CurrentTarget != null;
    }

    public Vector3 DirectionFromAngle(float angleInDegrees, bool angleIsGlobal)
    {
        if (!angleIsGlobal) angleInDegrees += transform.eulerAngles.y;
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }

    public float GetDistanceFromTarget()
    {
        float distance = 0.0f;
        if (CurrentTarget == null) distance = 999.0f;
        else distance = Vector3.Distance(transform.position, CurrentTarget.position);

        return distance;
    }

    private void OnDrawGizmos()
    {
#if UNITY_EDITOR
        if (!Application.isPlaying) return;

        Vector3 textPos = new Vector3(
            transform.position.x,
            transform.position.y + 2.3f,
            transform.position.z
            );

        string targetText;
        bool hasTarget = CurrentTarget != null;
        if (!hasTarget) targetText = "NoTarget";
        else targetText = CurrentTarget.gameObject.name;

        UnityEditor.Handles.Label(textPos, targetText + " ");
#endif
    }
}