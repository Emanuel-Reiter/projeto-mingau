using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    [SerializeField] private UIInteractionPrompt _interactionUI;
    public UIInteractionPrompt InteractionUI => _interactionUI;

    [SerializeField] private UICombo _comboUI;
    public UICombo ComboUI => _comboUI;

    [SerializeField] private UICollectables _collectablesUI;
    public UICollectables CollectablesUI => _collectablesUI;

    public void InitializeInteractPrompt()
    {
        InteractionUI.Initialize();
    }

    public void InitializeHUD()
    {
        ComboUI.Initialize();
        CollectablesUI.Initialize();
    }
}
