using UnityEngine;

public abstract class BaseUI : MonoBehaviour
{
    public abstract void Initialize();
    public abstract void UpdateContext(GameContext gameContext);
}
