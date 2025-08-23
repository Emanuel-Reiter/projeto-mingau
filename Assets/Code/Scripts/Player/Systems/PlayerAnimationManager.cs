using UnityEngine;

public class PlayerAnimationManager : MonoBehaviour
{
    [Header("Idle")]
    public AnimationClip Idle;

    [Header("Run")]
    public AnimationClip Run;

    [Header("Fall")]
    public AnimationClip Fall;

    [Header("Land")]
    public AnimationClip LandLight;
    public AnimationClip LandHeavy;

    [Header("jump")]
    public AnimationClip[] Jump;

    [Header("Dash")]
    public AnimationClip Dash;

    private Animator _animator;

    [Header("Animation interpolation times")]
    public float ShortInterpolationTime { get; private set; } = 0.025f;
    public float InterpolationTimeInSeconds2 { get; private set; } = 0.1f;
    public float interpolationTime_02 { get; private set; } = 0.25f;
    public float interpolationTime_03 { get; private set; } = 0.5f;
    public float interpolationTime_04 { get; private set; } = 1.0f;

    private void Start() {
        _animator = GetComponent<Animator>();
    }

    public void PlayAnimationInterpolated(AnimationClip animation, float interpolationTime) {
        _animator.CrossFadeInFixedTime(animation.name, interpolationTime);
    }

    public void PlayAnimation(AnimationClip animation) {
        _animator.Play(animation.name);
    }
}
