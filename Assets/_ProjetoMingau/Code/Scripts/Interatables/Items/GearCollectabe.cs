using DG.Tweening;
using System.Net.NetworkInformation;
using UnityEngine;

public class GearCollectabe : BaseCollectable
{
    [Header("VFX params")]
    [SerializeField] private ParticleSystem _idleVFX;
    [SerializeField] private ParticleSystem _collectVFX;
    [SerializeField] private float _collectDuration = 1.0f;

    [Header("Animation params")]
    [SerializeField] private float _animDuration = 2.0f;


    private MeshRenderer _mesh;
    private Collider _collider;

    private void Start()
    {
        try
        {
            _mesh = GetComponent<MeshRenderer>();
            _collider = GetComponent<Collider>();
        }
        catch
        {
            Debug.LogError($"Missing components on {this.name} GameObject!");
        }

        if (_idleVFX != null) _idleVFX.Play();

        // Give some ramdomness to _animDuration in order to avoid
        // repetition in long srteams of collectables
        float animOffest = Random.Range(-0.5f, 0.5f);
        _animDuration += animOffest;
        
        // DOTwenn idle animation
        Vector3 originalPos = transform.position;
        transform.DOMoveY(originalPos.y + 0.25f, _animDuration)
            .SetEase(Ease.InOutSine)
            .SetLoops(-1, LoopType.Yoyo);

        transform.DORotate(new Vector3(0.0f, 360.0f, 360.0f), _animDuration, RotateMode.LocalAxisAdd)
            .SetEase(Ease.Linear)
            .SetLoops(-1, LoopType.Incremental);
    }

    public override void Collect()
    {
        if (_idleVFX != null) _idleVFX.Stop(); ;
        if (_collectVFX != null) _collectVFX.Play();

        _collider.enabled = false;

        // Fade mesh size
        Sequence sequence = DOTween.Sequence();
        sequence.Append(transform.DOScale(0.0f, 0.25f).SetEase(Ease.InOutCubic));
        sequence.OnComplete(() => _mesh.enabled = false);

        GlobalTimer.Instance.StartTimer(_collectDuration, () => Disable());
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
