using UnityEngine;

public class NPCAnimationManager : MonoBehaviour
{
    private Animator _animator;

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    public void PlayAnimationInterpolated(AnimationClip animation, float interpolationTime)
    {
        _animator.CrossFadeInFixedTime(animation.name, interpolationTime);
    }

    public void PlayAnimation(AnimationClip animation)
    {
        _animator.Play(animation.name);
    }

    public void ToggleRootMotion(bool toggle)
    {
        _animator.applyRootMotion = toggle;
    }
}
