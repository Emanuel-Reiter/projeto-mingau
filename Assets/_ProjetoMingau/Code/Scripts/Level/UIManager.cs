using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    [SerializeField] private UIInteractionPrompt _interactionPrompt;
    public UIInteractionPrompt InteractionPrompt => _interactionPrompt;

    public void InitializeInteractPrompt()
    {
        InteractionPrompt.Initialize();
    }
}
