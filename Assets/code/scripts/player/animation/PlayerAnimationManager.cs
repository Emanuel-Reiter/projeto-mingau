using UnityEngine;

public class PlayerAnimationManager : MonoBehaviour
{
    [Header("Idle")]
    public AnimationClip idle_01_anim;

    [Header("Run")]
    public AnimationClip run_01_anim;

    [Header("Fall")]
    public AnimationClip fall_01_anim;

    [Header("Land")]
    public AnimationClip land_heavy_01_anim;
    public AnimationClip land_light_01_anim;

    [Header("jump")]
    public AnimationClip jump_01_anim;
    public AnimationClip jump_02_anim;

    [Header("Dash")]
    public AnimationClip dash_01_anim;

    private Animator playerAnimator;

    [Header("Animation interpolation times")]
    public float interpolationTime_00 { get; private set; } = 0.025f;
    public float interpolationTime_01 { get; private set; } = 0.1f;
    public float interpolationTime_02 { get; private set; } = 0.25f;
    public float interpolationTime_03 { get; private set; } = 0.5f;
    public float interpolationTime_04 { get; private set; } = 1.0f;

    private void Start() {
        playerAnimator = GameObject.FindGameObjectWithTag("PlayerGraphics").gameObject.GetComponent<Animator>();
    }

    public void PlayAnimationInterpolated(AnimationClip animation, float interpolationTime) {
        playerAnimator.CrossFadeInFixedTime(animation.name, interpolationTime);
    }

    public void PlayAnimation(AnimationClip animation) {
        playerAnimator.Play(animation.name);
    }
}
