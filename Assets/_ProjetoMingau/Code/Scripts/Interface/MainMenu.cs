using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : BaseUI
{
    private float _buttonSelectScale = 1.05f;
    private float _buttonConfirmScale = 1.1f;
    private float _buttonTransitionTime = 0.1f;

    public override void Initialize()
    {
        GameManager.Instance.OnGameContextChanged += UpdateContext;
        UpdateContext(GameManager.Instance.CurrentGameContext);
    }

    public override void UpdateContext(GameContext gameContext)
    {
        switch (gameContext)
        {
            case GameContext.MainMenu:
                gameObject.SetActive(true);
                break;
            default:
                gameObject.SetActive(false);
                break;
        }
    }

    public void StartButtonEvent()
    {
        LevelLoader.Instance.StartGame();
    }

    public void QuitButtonEvent()
    {
        GlobalTimer.Instance.StartTimer(1.0f, () => { Application.Quit(); });
    }

    public void OnConfirmButtonEvent(Button button)
    {
        UIManager.Instance.PlayConfirmSFX();

        button.interactable = false;

        button.transform.localScale = Vector3.one;
        button.transform.DOScale(_buttonConfirmScale, _buttonTransitionTime)
        .SetEase(Ease.InOutSine)
        .SetLoops(2, LoopType.Yoyo);
    }

    public void OnMouseEnterEvent(GameObject target)
    {
        if (target == null) return;

        target.transform.localScale = Vector3.one;
        target.transform.DOScale(_buttonSelectScale, _buttonTransitionTime).SetEase(Ease.InOutSine);
        UIManager.Instance.PlaySelectSFX();
    }

    public void OnMouseExitEvent(GameObject target)
    {
        if (target == null) return;

        target.transform.localScale = Vector3.one * _buttonSelectScale;
        target.transform.DOScale(1.0f, _buttonTransitionTime).SetEase(Ease.InOutSine);
    }

    private void OnDestroy()
    {
        GameManager.Instance.OnGameContextChanged -= UpdateContext;
    }
}
