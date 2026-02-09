using UnityEngine;

public class UIInteractionPrompt : UIBase
{
    [SerializeField] private CanvasGroup _interactionCanvas;

    private void Start()
    {
        Toggle(false);
    }

    public override void Initialize()
    {
        GameContext.I.PlayerInteract.OnInteractionAvailableChanged += Toggle;
    }
    private void OnDisable()
    {
        GameContext.I.PlayerInteract.OnInteractionAvailableChanged -= Toggle;
    }

    public override void Toggle(bool toggle)
    {
        _interactionCanvas.gameObject.SetActive(toggle);
    }
}
