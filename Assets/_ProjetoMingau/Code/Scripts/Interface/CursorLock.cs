using UnityEngine;

public class CursorLock : BaseUI
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
            case GameContextEnum.Playing:
                LockCursor(true);
                break;
            default:
                LockCursor(false);
                break;
        }
    }

    private void LockCursor(bool lockCursor)
    {
        if (lockCursor) 
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    private void OnDestroy()
    {
        GameManager.Instance.OnGameContextChanged -= UpdateContext;
    }
}
