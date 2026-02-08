using UnityEngine;

public class UIInteractionPrompt : MonoBehaviour
{
    [SerializeField] private GameObject _interactionPrompt;

    public void Initialize()
    {
        GameContext.I.PlayerInteract.OnInteractionAvailableChanged += Toggle;
    }

    private void OnDisable()
    {
        GameContext.I.PlayerInteract.OnInteractionAvailableChanged -= Toggle;
    }

    private void Start()
    {
        Toggle(false);
    }

    private void Toggle(bool value)
    {
        _interactionPrompt.SetActive(value);
    }
}
