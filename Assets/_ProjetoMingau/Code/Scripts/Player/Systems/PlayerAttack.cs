using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [Header("VFX params")]
    [SerializeField] private ParticleSystem[] _rightHandattackVFX;
    [SerializeField] private ParticleSystem[] _leftHandattackVFX;

    [Header("Audio params")]
    [SerializeField] private AudioSource _attackAudio;

    private bool _isHitDetectionEnabled = false;
    public bool IsHitDetectionEnabled => _isHitDetectionEnabled;

    public void PlayVFX(int handIndex)
    {
        if (handIndex == 0)
        {
            if (_rightHandattackVFX == null) return;
            foreach (ParticleSystem vfx in _rightHandattackVFX) vfx.Play();
        }
        else if (handIndex == 1)
        {
            if (_leftHandattackVFX == null) return;
            foreach (ParticleSystem vfx in _leftHandattackVFX) vfx.Play();
        }
        else
        {
            if (_rightHandattackVFX == null || _leftHandattackVFX == null) return;
            foreach (ParticleSystem vfx in _rightHandattackVFX) vfx.Play();
            foreach (ParticleSystem vfx in _leftHandattackVFX) vfx.Play();
        }
    }

    public void StopVFX()
    {
        if (_rightHandattackVFX == null || _leftHandattackVFX == null) return;
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
        if (_attackAudio == null) return;

        _attackAudio.pitch = Random.Range(0.9f, 1.2f);
        _attackAudio.Play();
    }
}
