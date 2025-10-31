using DG.Tweening;
using UnityEngine;

public class PlayerLand : MonoBehaviour
{
    [SerializeField] private Transform _playerRig;

    [SerializeField] private float _landAnimationLength = 0.5f;
    public float LandAnimationLength => _landAnimationLength;

    [SerializeField] private Vector3 _landAnimationScale = Vector3.one;

    private bool _isPlayngLandAnimation = false;

    public void TriggerLandAnimation()
    {
        if (_isPlayngLandAnimation) return;

        _isPlayngLandAnimation = true;
        Sequence sequence = DOTween.Sequence();
        sequence.Append(_playerRig.transform.DOScale(_landAnimationScale, (_landAnimationLength * 0.333f)));
        sequence.Append(_playerRig.transform.DOScale(Vector3.one, (_landAnimationLength * 0.667f)));
        sequence.OnComplete(() => _isPlayngLandAnimation = false);
    }
}
