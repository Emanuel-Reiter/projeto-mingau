using DG.Tweening;
using UnityEngine;

public class InteractionPrompt : BaseUI
{
    [SerializeField] private CanvasGroup _canvasGroup;

    private bool _isHidden = true;

    private void Start()
    {
        _canvasGroup.gameObject.SetActive(false);
    }

    public void Show()
    {
        if (!_isHidden) return;

        // Resets the fade amount
        _canvasGroup.DOFade(0.0f, 0.0f);

        _isHidden = false;
        _canvasGroup.gameObject.SetActive(true);
        ResetPopupTransform();

        if (_canvasGroup != null) _canvasGroup.DOFade(1.0f, 0.1f).SetEase(Ease.InOutSine);

        UIManager.Instance.PlaySelectSFX();
    }

    public void Hide()
    {
        if (_isHidden) return;

        // Resets the fade amount
        _canvasGroup.DOFade(1.0f, 0.0f);

        _isHidden = true;

        if (_canvasGroup != null) _canvasGroup.DOFade(0.0f, 0.1f).SetEase(Ease.InOutSine).OnComplete(() => { _canvasGroup.gameObject.SetActive(false); });
    }

    public void Interact()
    {
        _canvasGroup.transform.DOScale(1.333f, 0.1f)
            .SetEase(Ease.InOutSine)
            .SetLoops(2, LoopType.Yoyo)
            .OnComplete( () => {
                ResetPopupTransform();
                Hide();
            });

        UIManager.Instance.PlayConfirmSFX();
    }

    private void ResetPopupTransform()
    {
        _canvasGroup.transform.localScale = Vector3.one;
        _canvasGroup.transform.rotation = Quaternion.identity;
    }

    public override void Initialize()
    {

    }

    public override void UpdateContext(GameContext gameContext)
    {

    }
}
