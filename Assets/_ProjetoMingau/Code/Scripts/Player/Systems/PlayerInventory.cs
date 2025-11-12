using DG.Tweening;
using TMPro;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    private int _collectables;

    [Header("Params")]
    [SerializeField] private float _collectRadius = 0.667f;
    [SerializeField] private LayerMask _colletablesLayer;

    [SerializeField] private TMP_Text _collectablesCountText;
    [SerializeField] private TMP_Text _collectablesComboText;
    private float _baseComboTextSize;
    [SerializeField] private GameObject _collectablesContainer;

    [Header("Audio params")]
    [SerializeField] private AudioSource _collectAudio;
    [SerializeField] private float _audioPitchResetTime = 1.0f;
    private float _basePitch = 1.0f;
    private int _currentColletCombo = -1;
    private int _comboTimerIndex;

    private void Start()
    {
        if (_collectablesComboText != null)
        {
            _baseComboTextSize = _collectablesComboText.fontSize;

            _collectablesComboText.transform.DOScale(1.25f, 0.25f)
                .SetEase(Ease.InOutSine)
                .SetLoops(-1, LoopType.Yoyo);
        }

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

            ManageCollectCombo();
            PlayCollectSFX(_currentColletCombo);
            UpdateCollectablesUI();
        }
    }

    private void ManageCollectCombo()
    {
        GlobalTimer.Instance.CancelTimer(_comboTimerIndex);

        _currentColletCombo++;

        _comboTimerIndex = GlobalTimer.Instance.StartTimer(_audioPitchResetTime, () => HandleComboReset());
    }

    private void HandleComboReset()
    {
        int tempCombo = _currentColletCombo;

        _collectables += _currentColletCombo;
        _currentColletCombo = -1;

        if (tempCombo <= 0) return;
        UpdateCollectablesUI();
        PlayCollectSFX(0);
    }

    private void UpdateCollectablesUI()
    {
        // Collectables count text
        if (_collectablesCountText != null) _collectablesCountText.text = $"{_collectables}";

        // Collectables combo text
        if (_collectablesComboText != null)
        {
            if (_currentColletCombo > 0)
            {
                _collectablesComboText.DOFade(1.0f, 0.2f).SetEase(Ease.InOutSine);
                _collectablesComboText.text = $"x {_currentColletCombo}";
                
                // Increase size of the collect combo text by combo ammount capped at 10
                _collectablesComboText.fontSize = _baseComboTextSize + (Mathf.Clamp(_currentColletCombo, 0, 20) * 2f);
            }
            else
            {
                _collectablesComboText.text = "";
                _collectablesComboText.DOFade(0.0f, 0.2f).SetEase(Ease.InOutSine);
            }
        }

        // UI bounce when collecting items
        if (_collectablesContainer != null && _collectables > 0)
        {
            ResetCollectablesUITransform();

            _collectablesContainer.transform.DOScale(2.0f, 0.1f)
                .SetEase(Ease.InOutSine)
                .SetLoops(2, LoopType.Yoyo);

            float rotation = _collectables % 2 == 0 ? 22.5f : -22.5f;
            _collectablesContainer.transform.DORotate(new Vector3(0.0f, 0.0f, rotation), 0.1f)
                .SetEase(Ease.InOutSine)
                .SetLoops(2, LoopType.Yoyo)
                .OnComplete(() => ResetCollectablesUITransform());
        }
    }

    private void ResetCollectablesUITransform()
    {
        _collectablesContainer.transform.localScale = Vector3.one;
        _collectablesContainer.transform.rotation = Quaternion.identity;
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
