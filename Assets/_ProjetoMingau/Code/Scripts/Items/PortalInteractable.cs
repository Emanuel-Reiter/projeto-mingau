using UnityEngine;

public class PortalInteractable : BaseInteractable
{
    [SerializeField] private string _targetLevel;

    public override void Interact()
    {
        SetHasBeenInteracted(true);
        GameManager.Instance.LoadLevel(_targetLevel, () => { });
    }
}
