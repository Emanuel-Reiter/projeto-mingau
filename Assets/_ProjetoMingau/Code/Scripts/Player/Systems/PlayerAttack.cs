using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private ParticleSystem[] _rightHandattackVFX;
    [SerializeField] private ParticleSystem[] _leftHandattackVFX;

    private bool _isHitDetectionEnabled = false;
    public bool IsHitDetectionEnabled => _isHitDetectionEnabled;

    public void PlayVFX(int handIndex)
    {
        if (handIndex == 0)
        {
            foreach (ParticleSystem vfx in _rightHandattackVFX) vfx.Play();
        }
        else if (handIndex == 1)
        {
            foreach (ParticleSystem vfx in _leftHandattackVFX) vfx.Play();
        }
        else
        {
            foreach (ParticleSystem vfx in _rightHandattackVFX) vfx.Play();
            foreach (ParticleSystem vfx in _leftHandattackVFX) vfx.Play();
        }
    }

    public void StopVFX()
    {
        foreach (ParticleSystem vfx in _rightHandattackVFX) vfx.Stop();
        foreach (ParticleSystem vfx in _leftHandattackVFX) vfx.Stop();
    }

    public void EnableHitDetection()
    {
        _isHitDetectionEnabled = true;
    }

    public void DisableHitDetection()
    {
        _isHitDetectionEnabled = false;
    }

    public void PlaySFX()
    {

    }
}
