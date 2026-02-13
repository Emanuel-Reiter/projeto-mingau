using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    // Interaction prompt
    private UIInteractionPrompt _interactionPrompt;

    // HUD
    private UICombo _comboHUD;
    private UICollectables _collectablesHUD;

    // Menus
    private UITitleScreen _titleScreenMenu;

    // Loading screen
    private UILoadingScreen _loadingScreen;

    [Header("UI visual params")]
    [SerializeField] private float _buttonSelectScale = 1.1f;
    [SerializeField] private float _buttonConfirmScale = 1.2f;
    [SerializeField] private float _buttonTransitionTime = 0.2f;

    [Header("UI SFX")]
    [SerializeField] private AudioSfxDef _confirmAudio;
    [SerializeField] private AudioSfxDef _selectAudio;
    
    #region Initialization
    public void InitializeInteractPrompt()
    {
        if (!LoadInteractionPrompt())
        {
            Debug.LogError("Error loading interaction prompt.");
            return;
        }

        _interactionPrompt.Initialize();
    }

    private bool LoadInteractionPrompt()
    {
        _interactionPrompt = FindFirstObjectByType<UIInteractionPrompt>();
        
        if (CheckNull(_interactionPrompt)) return false;
        else return true;
    }

    public void InitializeHUD()
    {
        if (!LoadHUD())
        {
            Debug.LogError("Error loading HUD.");
            return;
        }

        _comboHUD.Initialize();
        _collectablesHUD.Initialize();
    }

    private bool LoadHUD()
    {
        _comboHUD = FindFirstObjectByType<UICombo>();
        _collectablesHUD = FindFirstObjectByType<UICollectables>();

        if (CheckNull(_comboHUD, _collectablesHUD)) return false;
        else return true;
    }

    public void InitializeMenus()
    {
        if (!LoadMenus())
        {
            Debug.LogError("Error loading menus.");
            return;
        }

        _titleScreenMenu.Initialize();
        _loadingScreen.Initialize();
    }

    private bool LoadMenus()
    {
        _titleScreenMenu = FindFirstObjectByType<UITitleScreen>();
        _loadingScreen = FindFirstObjectByType<UILoadingScreen>();
        
        if (CheckNull(_titleScreenMenu, _loadingScreen)) return false;
        else return true;
    }

    public static bool CheckNull(params Object[] objects)
    {
        foreach (var obj in objects)
        {
            if (obj == null)
            {
                return true;
            }
        }
        return false;
    }
    #endregion

    public void OnConfirmButtonEvent(Button target)
    {
        PlayConfirmSFX();

        target.interactable = false;

        target.transform.localScale = Vector3.one;
        target.transform.DOScale(_buttonConfirmScale, _buttonTransitionTime)
        .SetEase(Ease.InOutSine)
        .SetLoops(2, LoopType.Yoyo)
        .OnComplete(() => target.transform.DOKill());
    }

    public void OnMouseEnterEvent(Button target)
    {
        if (target == null) return;

        target.transform.localScale = Vector3.one;
        Tweener tween = target.transform.DOScale(_buttonSelectScale, _buttonTransitionTime)
            .SetEase(Ease.InOutSine);
        
        PlaySelectAudio();
    }

    public void OnMouseExitEvent(Button target)
    {
        if (target == null) return;

        target.transform.localScale = Vector3.one * _buttonSelectScale;
        target.transform.DOScale(1.0f, _buttonTransitionTime)
            .SetEase(Ease.InOutSine)
            .OnComplete(() => target.transform.DOKill());
    }

    #region Audio
    private void PlayConfirmSFX()
    {
        if(_confirmAudio == null) return;

        AudioSfxDef audio = Instantiate(_confirmAudio);
        AudioPool.Play(audio);
    }

    private void PlaySelectAudio()
    {
        if (_selectAudio == null) return;

        AudioSfxDef audio = Instantiate(_selectAudio);
        AudioPool.Play(audio);
    }
    #endregion
}
