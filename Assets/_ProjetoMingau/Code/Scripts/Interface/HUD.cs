using DG.Tweening;
using TMPro;
using UnityEngine;

public class HUD : BaseUI
{
    private PlayerInventory _playerInventory;

    [Header("Collectables params")]
    [SerializeField] private GameObject _collectablesContainer;

    [SerializeField] private TMP_Text _collectablesCountText;
    [SerializeField] private TMP_Text _collectablesComboText;

    private float _baseComboTextSize;

    public override void Initialize()
    {
        GameManager.Instance.OnGameContextChanged += UpdateContext;
        UpdateContext(GameManager.Instance.GameContext);

        // Try loading player inventory
        try
        {
            _playerInventory = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInventory>();

            _playerInventory.OnCollectablesChanged += UpdateCollectablesCounter;
            _playerInventory.OnCollectComboChanged += UpdateCollectComboCounter;
        }
        catch
        {
            Debug.LogError("Player Inventory not found!");
            return;
        }

        // Collectables idle anim idle anim
        if (_collectablesCountText != null)
        {
            Vector3 startPos = _collectablesContainer.transform.position;
            Vector3 targetPos = new Vector3(startPos.x + -36.0f, startPos.y + 24.0f, 0.0f);
            _collectablesContainer.transform.DOMove(targetPos, 3.0f)
                .SetEase(Ease.InOutSine)
                .SetLoops(-1, LoopType.Yoyo);
        }

        if (_collectablesComboText != null)
        {
            _baseComboTextSize = _collectablesComboText.fontSize;

            _collectablesComboText.transform.DOScale(1.25f, 0.25f)
                .SetEase(Ease.InOutSine)
                .SetLoops(-1, LoopType.Yoyo);
        }

        // Initialize UI values
        UpdateCollectablesCounter();
        UpdateCollectComboCounter();
    }

    public override void UpdateContext(GameContextEnum gameContext)
    {
        switch (gameContext)
        {
            case GameContextEnum.Playing:
                gameObject.SetActive(true);
                break;

            default:
                gameObject.SetActive(false);
                break;
        }
    }

    private void UpdateCollectablesCounter()
    {
        if (_collectablesCountText == null || _collectablesContainer == null) return;

       _collectablesCountText.text = $"{_playerInventory.Colletables}";

        // UI bounce when collecting items
        if  (_playerInventory.Colletables > 0)
        {
            ResetCollectablesUITransform();

            _collectablesContainer.transform.DOScale(2.0f, 0.1f)
                .SetEase(Ease.InOutSine)
                .SetLoops(2, LoopType.Yoyo);

            float rotation = _playerInventory.Colletables % 2 == 0 ? 22.5f : -22.5f;
            _collectablesContainer.transform.DORotate(new Vector3(0.0f, 0.0f, rotation), 0.1f)
                .SetEase(Ease.InOutSine)
                .SetLoops(2, LoopType.Yoyo)
                .OnComplete(() => ResetCollectablesUITransform());
        }
    }

    private void UpdateCollectComboCounter()
    {
        // Collectables combo text
        if (_collectablesComboText == null) return;
        
        if (_playerInventory.CurrentColletCombo < 1)
        {
            _collectablesComboText.text = "";
            _collectablesComboText.DOFade(0.0f, 0.2f).SetEase(Ease.InOutSine);
            return;
        }

        Debug.Log($"Updating current combo! {_playerInventory.CurrentColletCombo}");
        _collectablesComboText.DOFade(1.0f, 0.2f).SetEase(Ease.InOutSine);
        _collectablesComboText.text = $"x {_playerInventory.CurrentColletCombo}";

        // Increase size of the collect combo text by combo ammount capped at 10
        _collectablesComboText.fontSize = _baseComboTextSize + (Mathf.Clamp(_playerInventory.CurrentColletCombo, 0, 20) * 2f);
    }

    private void ResetCollectablesUITransform()
    {
        _collectablesContainer.transform.localScale = Vector3.one;
        _collectablesContainer.transform.rotation = Quaternion.identity;
    }

    private void OnDestroy()
    {
        GameManager.Instance.OnGameContextChanged -= UpdateContext;

        if (_playerInventory != null)
        {
            _playerInventory.OnCollectablesChanged -= UpdateCollectablesCounter;
            _playerInventory.OnCollectComboChanged -= UpdateCollectComboCounter;
        }
    }
}
