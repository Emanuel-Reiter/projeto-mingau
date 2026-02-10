using System.Collections.Generic;
using UnityEngine;

public class NPCAttack : MonoBehaviour
{
    private NPCDependencies _dependencies;

    [Header("Attack params")]
    [SerializeField] private Transform _attackOrigin;
    [SerializeField] private float _attackRadius = 0.5f;
    [SerializeField] private LayerMask _targetLayer;

    [Header("VFX params")]
    [SerializeField] private ParticleSystem _attackVFX;

    [Header("Audio params")]
    [SerializeField] private AudioSfxDef _attackAudio;

    private bool _isHitDetectionEnabled = false;
    public bool IsHitDetectionEnabled => _isHitDetectionEnabled;

    private HashSet<AttributesManager> _damagedTargets = new HashSet<AttributesManager>();

    private void Start()
    {
        _dependencies = GetComponent<NPCDependencies>();
    }

    private void Update()
    {
        if (_isHitDetectionEnabled) SearchTargets();
    }

    private void SearchTargets()
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
            }
            catch
            {
                continue;
            }
        }
    }

    public void PlayVFX() 
    {
        if (_attackVFX != null) _attackVFX.Play(); 
    }

    public void StopVFX() 
    {
        if (_attackVFX != null) _attackVFX.Stop();
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
        Vector3 auidoPos = transform.position + Vector3.up;
        audioCopy.Pitch = Random.Range(0.9f, 1.1f);
        AudioPool.Play(audioCopy, auidoPos);
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (!Application.isPlaying) return;

        Gizmos.DrawWireSphere(_attackOrigin.position, _attackRadius);
    }
#endif
}
