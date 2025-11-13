using UnityEngine;

public class MainMenu : BaseUI
{
    public override void Initialize()
    {
        GameManager.Instance.OnGameStateChanged += UpdadateMainMenu;
        UpdadateMainMenu(GameManager.Instance.GameState);
    }

    private void UpdadateMainMenu(GameStateEnum gameState)
    {
        switch (gameState)
        {
            case GameStateEnum.MainMenu:
                gameObject.SetActive(true);
                break;
            default:
                gameObject.SetActive(false);
                break;
        }
    }

    public void PlayButtonEvent()
    {
        GameManager.Instance.ChangeGameState(GameStateEnum.Running);
    }

    private void OnDestroy()
    {
        GameManager.Instance.OnGameStateChanged -= UpdadateMainMenu;
    }
}
