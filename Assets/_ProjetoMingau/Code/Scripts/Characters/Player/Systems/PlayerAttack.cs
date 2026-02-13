using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private PlayerDependencies _dependencies;

    [Header("Attack params")]
    [SerializeField] private Transform _attackOrigin;
    [SerializeField] private float _attackRadius = 0.5f;
    [SerializeField] private LayerMask _targetLayer;

    [Header("VFX params")]
    [SerializeField] private ParticleSystem[] _rightHandattackVFX;
    [SerializeField] private ParticleSystem[] _leftHandattackVFX;

    [Header("Audio params")]
    [SerializeField] private AudioSfxDef _attackAudio;

    private bool _isHitDetectionEnabled = false;
    public bool IsHitDetectionEnabled => _isHitDetectionEnabled;

    private int _currentAtkCombo = 0;

    private HashSet<AttributesManager> _damagedTargets = new HashSet<AttributesManager>();

    public delegate void OnAttackHitDelegate();
    public event OnAttackHitDelegate OnAttackHit;

    private void Start()
    {
        try
        {
            _dependencies = GetComponent<PlayerDependencies>();
        }
        catch
        {
            Debug.LogError("Missing PlayerDependencies component!");
        }
    }

    private void Update()
    {
        if (_isHitDetectionEnabled) AttackTargets();
    }

    private void AttackTargets()
    {
        Collider[] targets = Physics.OverlapSphere(_attackOrigin.position, _attackRadius, _targetLayer);

        foreach (Collider target in targets) 
        {
            AttributesManager targetAttributes = target.gameObject.GetComponent<AttributesManager>();
                
            if (targetAttributes == null) continue;
            if (_damagedTargets.Contains(targetAttributes)) continue;
            
            try
            {
                targetAttributes.TakeDamage(_dependencies.Attributes.Damage);
                _damagedTargets.Add(targetAttributes);
                OnAttackHit?.Invoke();
            }
            catch 
            {
                continue;
            }
        }
    }

    public void SetCurrentCombo(int currentCombo) { _currentAtkCombo = currentCombo; }

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

    public void ToggleHitDetection(int toggle)
    {
        if (toggle == 1)
        {
            _isHitDetectionEnabled = true;
            _damagedTargets.Clear();
        }
        else _isHitDetectionEnabled = false;
    }

    public void PlaySFX()
    {
        if (_attackAudio == null) return;

        AudioSfxDef audioCopy = Instantiate(_attackAudio);
        audioCopy.Pitch = 1.0f + (_currentAtkCombo * 0.2f);
        Vector3 audioPos = transform.position + new Vector3(0f, 1.25f, 0.5f);
        AudioPool.Play(audioCopy, audioPos);
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (!Application.isPlaying) return;

        Gizmos.DrawWireSphere(_attackOrigin.position, _attackRadius);
    }
#endif
}
