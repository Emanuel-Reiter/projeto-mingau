using UnityEngine;

public class MainMenu : BaseUI
{
    public override void Initialize()
    {
        GameManager.Instance.OnGameContextChanged += UpdateContext;
        UpdateContext(GameManager.Instance.GameContext);
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

    public void StartButtonEvent() { GameManager.Instance.StartGame(); }

    private void OnDestroy()
    {
        GameManager.Instance.OnGameContextChanged -= UpdateContext;
    }
}
