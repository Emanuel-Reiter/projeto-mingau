using System.Threading.Tasks;
using UnityEngine;

public class PortalInteractable : BaseInteractable
{
    [SerializeField] private LevelData _targetLevel;

    public async override void Interact()
    {
        if (HasBeenInteracted) return;
        await TriggerLevelLoad();
    }

    private async Task TriggerLevelLoad()
    {
        if (LevelManager.I.IsLevelLoading) return;
        SetHasBeenInteracted(true);
        await LevelManager.I.LoadLevel(_targetLevel);
    }
}
