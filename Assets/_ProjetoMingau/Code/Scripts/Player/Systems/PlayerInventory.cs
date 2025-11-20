using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    private int _collectables;
    public int Colletables
    {
        get => _collectables;
        set
        {
            _collectables = value;
            OnCollectablesChanged?.Invoke();
        }
    }

    public delegate void OnCollectablesChangedDelegate();
    public event OnCollectablesChangedDelegate OnCollectablesChanged;

    [Header("Params")]
    [SerializeField] private float _collectRadius = 0.667f;
    [SerializeField] private LayerMask _colletablesLayer;

    [Header("Audio params")]
    [SerializeField] private AudioSource _collectAudio;
    [SerializeField] private float _audioPitchResetTime = 1.0f;
    private float _basePitch = 1.0f;

    [Header("Collect combo")]
    private int _currentColletCombo = 0;
    public int CurrentColletCombo 
    {
        get => _currentColletCombo;
        set
        {
            _currentColletCombo = value;

            // Calculate rate of the combo bonus
            CollectComboBonus = Mathf.FloorToInt(CurrentColletCombo / 4);

            OnCollectComboChanged?.Invoke();
        }
    }

    public int CollectComboBonus { get; private set; } = 0;

    public delegate void OnCollectComboChangedDelegate();
    public event OnCollectComboChangedDelegate OnCollectComboChanged;
    
    private int _comboTimerIndex;

    private void Update()
    {
        SerchItems();
    }

    private void SerchItems()
    {
        Vector3 origin = transform.position + (Vector3.up * _collectRadius);
        Vector3 direction = Vector3.zero;

        Collider[] objectsInRange = Physics.OverlapSphere(origin, _collectRadius, _colletablesLayer);

        foreach (Collider obj in objectsInRange)
        {

            BaseCollectable collectable = obj.gameObject.GetComponent<BaseCollectable>();
            collectable.Collect();
            Colletables += collectable.Value;

            ManageCollectCombo();
            PlayCollectSFX(CollectComboBonus);
        }
    }

    private void ManageCollectCombo()
    {
        GlobalTimer.Instance.CancelTimer(_comboTimerIndex);
        CurrentColletCombo++;
        _comboTimerIndex = GlobalTimer.Instance.StartTimer(_audioPitchResetTime, () => HandleComboReset());
    }

    private void HandleComboReset()
    {
        int collectBonus = CollectComboBonus;

        Colletables += CollectComboBonus;
        CurrentColletCombo = 0;
        CollectComboBonus = 0;

        if (collectBonus > 0) PlayCollectSFX(CollectComboBonus);
    }

    public void HardResetCollectCombo()
    {
        GlobalTimer.Instance.CancelTimer(_comboTimerIndex);
        CurrentColletCombo = 0;
        CollectComboBonus = 0;
    }

    private void PlayCollectSFX(int comboDepth)
    {
        if (_collectAudio == null) return;

        // Plays sfx with increasing pitch clamped at 10 by collecting items in sequence
        _collectAudio.pitch = _basePitch + (Mathf.Clamp(comboDepth, 0, 10) * 0.1f);
        _collectAudio.Play();
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if(!Application.isPlaying) return;

        Vector3 origin = transform.position + (Vector3.up * _collectRadius);
        Gizmos.DrawWireSphere(origin, _collectRadius);
    }
#endif
}
