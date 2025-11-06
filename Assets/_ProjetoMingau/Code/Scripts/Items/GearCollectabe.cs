using DG.Tweening;
using UnityEngine;

public class GearCollectabe : BaseCollectable
{
    [Header("VFX params")]
    [SerializeField] private ParticleSystem _idleVFX;
    [SerializeField] private ParticleSystem _collectVFX;
    [SerializeField] private float _collectDuration = 1.0f;

    [Header("Animation params")]
    [SerializeField] private float _rotationDuration = 2.0f;

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

        //Sequence sequence = DOTween.Sequence();
        //sequence.Append(_playerRig.transform.DOScale(_landAnimationScale, (_landAnimationLength * 0.333f)));
        //sequence.Append(_playerRig.transform.DOScale(Vector3.one, (_landAnimationLength * 0.667f)));
        //sequence.OnComplete(() => _isPlayngLandAnimation = false);

        transform.DORotate(new Vector3(0.0f, 360.0f, 360.0f), _rotationDuration, RotateMode.LocalAxisAdd)
            .SetEase(Ease.Linear)
            .SetLoops(-1, LoopType.Incremental);
    }

    public override void Collect()
    {
        if (_idleVFX != null) _idleVFX.Stop(); ;
        if (_collectVFX != null) _collectVFX.Play();

        _mesh.enabled = false;
        _collider.enabled = false;

        TimerSingleton.Instance.StartTimer(_collectDuration, () => Disable());
    }

    private void Disable()
    {
        if (_collectVFX != null) _collectVFX.Stop();
        gameObject.SetActive(false);
    }
}
