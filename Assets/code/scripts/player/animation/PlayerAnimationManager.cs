using UnityEngine;

public class PlayerAnimationManager : MonoBehaviour
{
    [Header("Animation clip references")]
    public AnimationClip idleAnimation;
    public AnimationClip runAnimation;
    public AnimationClip fallAnimation;
    public AnimationClip jumpAnimation01;
    public AnimationClip jumpAnimation02;

    private Animator playerAnimator;

    [Header("Animation interpolation times")]
    public float instantaneousInterpolationTime { get; private set; } = 0.05f;
    public float fastInterpolationTime { get; private set; } = 0.2f;
    public float averageInterpolationTime { get; private set; } = 0.4f;
    public float slowInterpolationTime { get; private set; } = 0.8f;

    private void Start() {
        playerAnimator = GameObject.FindGameObjectWithTag("PlayerGraphics").gameObject.GetComponent<Animator>();
    }

    public void PlayAnimationInterpolated(AnimationClip animation, float interpolationTime) {
        playerAnimator.CrossFadeInFixedTime(animation.name, interpolationTime);
    }
}
