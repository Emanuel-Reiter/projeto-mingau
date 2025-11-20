using UnityEngine;

public class PortalInteractable : BaseInteractable
{
    [SerializeField] private string _targetLevel;

    public override void Interact()
    {
        SetHasBeenInteracted(true);
        LevelLoader.Instance.LoadLevel(_targetLevel, () => { });
    }
}
