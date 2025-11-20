using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : BaseUI
{
    [SerializeField] private Button _startButton;
    private float _buttonSelectScale = 1.05f;
    private float _buttonConfirmScale = 1.1f;
    private float _buttonTransitionTime = 0.1f;

    public override void Initialize()
    {
        GameManager.Instance.OnGameContextChanged += UpdateContext;
        UpdateContext(GameManager.Instance.GameContext);

        _startButton.interactable = true;
    }

    public override void UpdateContext(GameContextEnum gameContext)
    {
        switch (gameContext)
        {
            case GameContextEnum.MainMenu:
                gameObject.SetActive(true);
                break;
            default:
                gameObject.SetActive(false);
                break;
        }
    }

    public void StartButtonEvent()
    {
        UIManager.Instance.PlayConfirmSFX();
        
        _startButton.transform.localScale = Vector3.one;
        _startButton.transform.DOScale(_buttonConfirmScale, _buttonTransitionTime)
        .SetEase(Ease.InOutSine)
        .SetLoops(2, LoopType.Yoyo);

        _startButton.interactable = false;

        LevelLoader.Instance.StartGame();
    }

    public void OnMouseEnterEvent()
    {
        if (!_startButton.interactable) return;

        _startButton.transform.localScale = Vector3.one;

        _startButton.transform.DOScale(_buttonSelectScale, _buttonTransitionTime)
                .SetEase(Ease.InOutSine);

        UIManager.Instance.PlaySelectSFX();
    }

    public void OnMouseExitEvent()
    {
        if (!_startButton.interactable) return;

        _startButton.transform.localScale = Vector3.one * _buttonSelectScale;

        _startButton.transform.DOScale(1.0f, _buttonTransitionTime)
        .SetEase(Ease.InOutSine);
    }

    private void OnDestroy()
    {
        GameManager.Instance.OnGameContextChanged -= UpdateContext;
    }
}
