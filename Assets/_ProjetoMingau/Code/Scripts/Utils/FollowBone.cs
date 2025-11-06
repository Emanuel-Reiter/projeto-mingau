using UnityEngine;

public class FollowBone : MonoBehaviour {
    [Header("Params")]
    [SerializeField] private Transform _targetBone;

    [Header("Smoothing params")]
    [SerializeField] private bool _useSmoothing;
    [SerializeField] private float _smoothTime = 0.3f;
    private Vector3 _velocity = Vector3.zero;


    private void LateUpdate() {
        if (_useSmoothing) FollowSmooth();
        else Follow();
    }

    private void Follow()
    {
        transform.position = _targetBone.position;
        transform.rotation = _targetBone.rotation;
    }

    private void FollowSmooth()
    {
        transform.position = Vector3.SmoothDamp(transform.position, _targetBone.position, ref _velocity, _smoothTime);
        transform.rotation = _targetBone.rotation;
    }
}
