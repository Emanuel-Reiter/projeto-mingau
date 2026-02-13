using UnityEngine;
using UnityEngine.UI;

public class UILoadingScreen : UIBase
{
    [SerializeField] private CanvasGroup _loadingScreenCanvas;
    [SerializeField] private Slider _loadingSlider;

    private float _loadingProgress;
    private float _loadingSmoothTime = 1f;

    private void Start()
    {
        Toggle(false);
    }

    public override void Initialize()
    {
        LevelManager.I.OnLevelLoadingChanged += Toggle;
        LevelManager.I.OnLevelLoadPercentChanged += UpdateLoadingSlider;
    }

    private void OnDisable()
    {
        LevelManager.I.OnLevelLoadingChanged -= Toggle;
        LevelManager.I.OnLevelLoadPercentChanged -= UpdateLoadingSlider;
    }

    public override void Toggle(bool toggle)
    {
        _loadingScreenCanvas.gameObject.SetActive(toggle);
        _loadingSlider.value = 0;
    }

    private void UpdateLoadingSlider(float loadPercent)
    {
        _loadingProgress = loadPercent;
    }

    public void Update()
    {
        bool isLoading = LevelManager.I.IsLevelLoading;
        if (!isLoading) return;

        _loadingSlider.value = Mathf.MoveTowards(_loadingSlider.value, _loadingProgress, _loadingSmoothTime * Time.deltaTime);
    }
}
