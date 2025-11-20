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

        // Resets the fade amount
        _label.DOFade(0.0f, 0.0f);
        _panel.DOFade(0.0f, 0.0f);

        _isHidden = false;
        _panel.gameObject.SetActive(true);
        ResetPopupTransform();

        if (_panel != null)
        {
            _label.DOFade(1.0f, 0.1f).SetEase(Ease.InOutSine);
            _panel.DOFade(1.0f, 0.1f).SetEase(Ease.InOutSine);
        }

        UIManager.Instance.PlaySelectSFX();
    }

    public void HidePopup()
    {
        if (_isHidden) return;

        // Resets the fade amount
        _label.DOFade(1.0f, 0.0f);
        _panel.DOFade(1.0f, 0.0f);

        _isHidden = true;

        if (_panel != null)
        {
            _label.DOFade(0.0f, 0.1f).SetEase(Ease.InOutSine);
            _panel.DOFade(0.0f, 0.1f).SetEase(Ease.InOutSine)
                .OnComplete(() => { _panel.gameObject.SetActive(false); });
        }
    }

    public void Interact()
    {
        _panel.rectTransform.DOScale(1.333f, 0.1f)
            .SetEase(Ease.InOutSine)
            .SetLoops(2, LoopType.Yoyo)
            .OnComplete( () => {
                ResetPopupTransform();
                HidePopup();
            });

        UIManager.Instance.PlayConfirmSFX();
    }

    private void ResetPopupTransform()
    {
        _panel.rectTransform.localScale = Vector3.one;
        _panel.rectTransform.rotation = Quaternion.identity;
    }

    public override void Initialize()
    {

    }

    public override void UpdateContext(GameContextEnum gameContext)
    {

    }
}
