using UnityEngine;

public class UITitleScreen : UIBase
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

    public void StartGameEvent()
    {
        _ = LevelManager.I.InitalizeGame();
    }

    public override void Toggle(bool toggle)
    {
        _titleScreenCanvas.gameObject.SetActive(toggle);
    }

    private void DisableTitleScreen(float loadPercent)
    {
        if (loadPercent < 1f) return;

        Toggle(false);
    }

    public void ExitGameEvent()
    {
        GlobalTimer.I.StartTimer(1.0f, () => { Application.Quit(); });
    }
}
