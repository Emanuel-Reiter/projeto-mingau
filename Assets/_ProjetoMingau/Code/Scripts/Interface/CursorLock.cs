using UnityEngine;

public class CursorLock : BaseUI
{
    public override void Initialize()
    {
        GameManager.Instance.OnGameStateChanged += UpdateLockCursor;
        UpdateLockCursor(GameManager.Instance.GameState);
    }

    private void UpdateLockCursor(GameStateEnum gameState)
    {
        switch (gameState)
        {
            case GameStateEnum.Running:
                LockCursor(true);
                break;

            case GameStateEnum.Loading:
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
        GameManager.Instance.OnGameStateChanged -= UpdateLockCursor;
    }
}
