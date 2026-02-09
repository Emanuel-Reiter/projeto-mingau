using UnityEngine;

public class PlayerSimpleInventory : MonoBehaviour
{
    public delegate void OnCollectablesChangedDelegate(int collectables);
    public event OnCollectablesChangedDelegate OnCollectablesChanged;

    private int _collectables;
    public int Collectables
    {
        get => _collectables;
        set
        {
            if (_collectables == value) return;

            _collectables = value;
            OnCollectablesChanged?.Invoke(Collectables);
        }
    }
}
