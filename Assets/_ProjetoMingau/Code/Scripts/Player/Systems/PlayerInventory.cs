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
            OnCollectComboChanged?.Invoke();
        }
    }

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
            PlayCollectSFX(CurrentColletCombo);
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
        int tempCombo = CurrentColletCombo;

        Colletables += CurrentColletCombo;
        CurrentColletCombo = 0;

        if (tempCombo <= 0) return;
        PlayCollectSFX(0);
    }

    public void HardResetCollectCombo()
    {
        GlobalTimer.Instance.CancelTimer(_comboTimerIndex);
        CurrentColletCombo = 0;
    }

    private void PlayCollectSFX(int comboDepth)
    {
        if (_collectAudio == null) return;

        // Plays sfx with increasing pitch clamped at 10 by collecting items in sequence
        _collectAudio.pitch = _basePitch + (Mathf.Clamp(comboDepth, 0, 20) * 0.05f);
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
