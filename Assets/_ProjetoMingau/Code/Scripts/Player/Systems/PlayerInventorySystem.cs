using DG.Tweening;
using TMPro;
using UnityEditor.PackageManager;
using UnityEngine;

public class PlayerInventorySystem : MonoBehaviour
{
    private int _collectables = 0;

    [Header("Params")]
    [SerializeField] private float _collectRadius = 0.667f;
    [SerializeField] private LayerMask _colletablesLayer;

    [SerializeField] private TMP_Text _collectablesCountText;
    [SerializeField] private GameObject _collectablesContainer;

    [Header("Audio params")]
    [SerializeField] private AudioSource _collectAudio;
    [SerializeField] private float _audioPitchResetTime = 1.0f;
    private float _currentPitch = 1.0f;
    private int _pitchTimerIndex;

    private void Start()
    {
        UpdateCollectablesUI();

        // _collectablesContainer idle anim
        if(_collectablesCountText != null)
        {
            Vector3 startPos = _collectablesContainer.transform.position;
            Vector3 targetPos = new Vector3(startPos.x + -36.0f, startPos.y + 24.0f, 0.0f);
            _collectablesContainer.transform.DOMove(targetPos, 3.0f)
                .SetEase(Ease.InOutSine)
                .SetLoops(-1, LoopType.Yoyo);
        }
    }

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
            _collectables += collectable.Value;
            UpdateCollectablesUI();
            PlayCollectSFX();
        }

    }

    private void UpdateCollectablesUI()
    {
        if (_collectablesCountText != null) _collectablesCountText.text = $"x {_collectables}";
        
        // UI bounce when collecting items
        if (_collectablesContainer != null && _collectables > 0)
        {
            _collectablesContainer.transform.DOScale(2.0f, 0.1f)
                .SetEase(Ease.InOutSine)
                .SetLoops(2, LoopType.Yoyo);

            float rotation = _collectables % 2 == 0 ? 22.5f : -22.5f;
            _collectablesContainer.transform.DORotate(new Vector3(0.0f, 0.0f, rotation), 0.1f)
                .SetEase(Ease.InOutSine)
                .SetLoops(2, LoopType.Yoyo);
        }
    }

    private void PlayCollectSFX()
    {
        if (_collectAudio == null) return;

        TimerSingleton.Instance.CancelTimer(_pitchTimerIndex);

        _collectAudio.Play();

        // Play sfx with increasing pitch by collecting items in sequence
        _collectAudio.pitch += 0.1f;
        _collectAudio.pitch = Mathf.Clamp(_collectAudio.pitch, 1.0f, 2.0f);

        _pitchTimerIndex = TimerSingleton.Instance.StartTimer(_audioPitchResetTime, () => { _collectAudio.pitch = 1.0f; });
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
