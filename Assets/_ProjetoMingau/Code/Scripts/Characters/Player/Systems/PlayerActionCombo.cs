using DG.Tweening;
using UnityEngine;

public class PlayerActionCombo : MonoBehaviour
{
    private PlayerDependencies _deps;

    private float _comboResetTime = 2.0f;
    private int _comboResetTimer;

    public delegate void OnComboChangeDelegate(int comboIndex);
    public event OnComboChangeDelegate OnComboChange;
    
    private int _comboIndex;
    public int ComboIndex
    {
        get => _comboIndex;
        set
        {
            if (_comboIndex == value) return;
            
            _comboIndex = value;
            OnComboChange?.Invoke(ComboIndex);
            HandleComboReset();
        }
    }

    private void Start()
    {
        _deps = GetComponent<PlayerDependencies>();

        _deps.Inventory.OnCollectablesChanged += AddComboByCollect;
        _deps.Attack.OnAttackHit += AddComboByAttack;
    }

    private void OnDisable()
    {
        _deps.Inventory.OnCollectablesChanged -= AddComboByCollect;
        _deps.Attack.OnAttackHit -= AddComboByAttack;
    }

    private void HandleComboReset()
    {
        GlobalTimer.I.CancelTimer(_comboResetTimer);
        _comboResetTimer = GlobalTimer.I.StartTimer(_comboResetTime, () => { ComboIndex = 0; });
    }

    private void AddComboByCollect(int value)
    {
        // ignore value
        ComboIndex++;
    }

    public void AddComboByAttack()
    {
        ComboIndex++; 
    }
}
