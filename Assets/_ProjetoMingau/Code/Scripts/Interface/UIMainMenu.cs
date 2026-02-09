using UnityEngine;

public class UIMainMenu : UIBase
{
    [SerializeField] private CanvasGroup _titleScreenCanvas;

    public override void Initialize()
    {
        LevelManager.I.OnLevelLoadPercentChanged += DisableTitleScreen;
    }

    private void OnDisable()
    {
        LevelManager.I.OnLevelLoadPercentChanged -= DisableTitleScreen;
    }

    public void StartGame()
    {
        _ = LevelManager.I.InitalizeGame();
    }

    public override void Toggle(bool toggle)
    {
        _titleScreenCanvas.gameObject.SetActive(toggle);
    }

    private void DisableTitleScreen(float loadPercent)
    {
        Debug.Log($"Entrou! {loadPercent}");

        if (loadPercent < 1f) return;

        Toggle(false);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
