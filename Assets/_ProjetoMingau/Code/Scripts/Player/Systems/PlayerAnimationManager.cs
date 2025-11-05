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

    [Header("Jump")]
    public AnimationClip[] Jump;

    [Header("Dash")]
    public AnimationClip Dash;

    [Header("Light Attack")]
    public AnimationClip[] AttackLight;

    private Animator _animator;

    [Header("Animation interpolation times")]
    public float InstantTransitionTime { get; private set; } = 0.025f;
    public float VeryFastTransitionTime { get; private set; } = 0.1f;
    public float FastTransitionTime { get; private set; } = 0.2f;
    public float MediumTransitionTime { get; private set; } = 0.5f;
    public float SlowTransitionTime { get; private set; } = 1.0f;
    public float VerySlowTransitionTime { get; private set; } = 2.0f;

    private void Start() {
        _animator = GetComponent<Animator>();
    }

    public void PlayInterpolated(AnimationClip animation, float interpolationTime) {
        _animator.CrossFadeInFixedTime(animation.name, interpolationTime);
    }

    public void Play(AnimationClip animation) {
        _animator.Play(animation.name);
    }
}
