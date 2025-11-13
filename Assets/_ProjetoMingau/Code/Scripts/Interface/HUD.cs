using UnityEngine;

public class HUD : BaseUI
{
    public override void Initialize()
    {
        GameManager.Instance.OnGameStateChanged += UpdateState;
        UpdateState(GameManager.Instance.GameState);
    }

    private void UpdateState(GameStateEnum gameState)
    {
        switch (gameState)
        {
            case GameStateEnum.Running:
                gameObject.SetActive(true);
                break;

            default:
                gameObject.SetActive(false);
                break;
        }
    }

    private void OnDestroy()
    {
        GameManager.Instance.OnGameStateChanged -= UpdateState;
    }
}
