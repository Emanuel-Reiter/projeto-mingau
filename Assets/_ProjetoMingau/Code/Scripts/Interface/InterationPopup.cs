using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InterationPopup : BaseUI
{
    [SerializeField] private Image _panel;
    [SerializeField] private TMP_Text _label;

    private bool _isHidden = true;

    private void Start()
    {
        _panel.gameObject.SetActive(false);
    }

    public void ShowPopup()
    {
        if (!_isHidden) return;

        _panel.gameObject.SetActive(true);
        ResetPopupTransform();

        if (_panel != null)
        {
            _label.DOFade(1.0f, 0.1f).SetEase(Ease.InOutSine);
            _panel.DOFade(1.0f, 0.1f).SetEase(Ease.InOutSine)
                .OnComplete(() => { _isHidden = false; });
        }
    }

    public void HidePopup()
    {
        if (_isHidden) return;

        if (_panel != null)
        {
            _label.DOFade(0.0f, 0.1f).SetEase(Ease.InOutSine);
            _panel.DOFade(0.0f, 0.1f).SetEase(Ease.InOutSine)
                .OnComplete(() =>
                {
                    _isHidden = true;
                    _panel.gameObject.SetActive(false);
                });
        }
    }

    public void Interact()
    {
        _panel.transform.DOScale(2.0f, 0.1f)
            .SetEase(Ease.InOutSine)
            .SetLoops(2, LoopType.Yoyo);

        _panel.transform.DORotate(new Vector3(0.0f, 0.0f, 30.0f), 0.1f)
            .SetEase(Ease.InOutSine)
            .SetLoops(2, LoopType.Yoyo)
            .OnComplete(() =>
            {
                ResetPopupTransform();
                HidePopup();
            });
    }

    private void ResetPopupTransform()
    {
        _panel.transform.localScale = Vector3.one;
        _panel.transform.rotation = Quaternion.identity;
    }

    public override void Initialize()
    {

    }

    public override void UpdateContext(GameContextEnum gameContext)
    {

    }
}
