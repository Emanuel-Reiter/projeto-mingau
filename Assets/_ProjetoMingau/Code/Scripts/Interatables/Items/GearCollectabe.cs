using DG.Tweening;
using UnityEngine;

public class GearCollectabe : BaseCollectable
{
    [Header("VFX params")]
    [SerializeField] private ParticleSystem _idleVFX;
    [SerializeField] private ParticleSystem _collectVFX;
    [SerializeField] private float _despawnTime = 1.0f;

    //[Header("Animation params")]
    //[SerializeField] private float _animDuration = 2.0f;

    private MeshRenderer _mesh;
    private Collider _collider;

    private void Start()
    {
        _mesh = GetComponent<MeshRenderer>();
        _collider = GetComponent<Collider>();

        if (_mesh == null || _collider == null)
        {
            Debug.LogError($"Missing components on {this.name} GameObject!");
        }

        if (_idleVFX != null) _idleVFX.Play();
    }

    public override void Collect()
    {
        if (_idleVFX != null) _idleVFX.Stop(); ;
        if (_collectVFX != null) _collectVFX.Play();

        _collider.enabled = false;

        Sequence sequence = DOTween.Sequence();
        sequence.Append(transform.DOScale(0.0f, 0.25f).SetEase(Ease.InOutCubic));
        sequence.OnComplete(() =>
        {
            _mesh.enabled = false;
            transform.DOKill();
        });

        GlobalTimer.I.StartTimer(_despawnTime, () => Disable());
    }

    private void Disable()
    {
        if (_collectVFX != null) _collectVFX.Stop();
        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        transform.DOKill();
    }
}
