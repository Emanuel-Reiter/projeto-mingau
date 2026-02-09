using DG.Tweening;
using TMPro;
using UnityEngine;

public class UICollectables : UIBase
{
    [SerializeField] private CanvasGroup _collectablesCanvas;
    [SerializeField] private TMP_Text _collectablesText;

    public override void Initialize()
    {
        GameContext.I.PlayerInventory.OnCollectablesChanged += UpdateCollectablesText;
        UpdateCollectablesText(0);
    }

    private void OnDisable()
    {
        GameContext.I.PlayerInventory.OnCollectablesChanged -= UpdateCollectablesText;
    }

    public override void Toggle(bool toggle)
    {
        _collectablesCanvas.gameObject.SetActive(toggle);
    }

    private void UpdateCollectablesText(int collectables)
    {
        if (_collectablesText == null || _collectablesCanvas == null) return;

        _collectablesText.text = $"x {collectables}";

        if (collectables == 0) return;

        ResetUITransforms();

        _collectablesCanvas.transform.DOScale(2.0f, 0.1f)
            .SetEase(Ease.InOutSine)
            .SetLoops(2, LoopType.Yoyo);

        float rotation = collectables % 2 == 0 ? 22.5f : -22.5f;
        _collectablesCanvas.transform.DORotate(new Vector3(0.0f, 0.0f, rotation), 0.1f)
            .SetEase(Ease.InOutSine)
            .SetLoops(2, LoopType.Yoyo)
            .OnComplete(() => ResetUITransforms());
    }

    private void ResetUITransforms()
    {
        _collectablesCanvas.transform.localScale = Vector3.one;
        _collectablesCanvas.transform.rotation = Quaternion.identity;
    }
}
