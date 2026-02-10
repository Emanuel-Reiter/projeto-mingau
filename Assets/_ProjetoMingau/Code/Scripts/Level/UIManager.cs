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

    [Header("UI visual params")]
    [SerializeField] private float _buttonSelectScale = 1.05f;
    [SerializeField] private float _buttonConfirmScale = 1.1f;
    [SerializeField] private float _buttonTransitionTime = 0.1f;

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
        if (_interactionPrompt == null) return false;

        return true;
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
        if (_comboHUD == null ) return false;

        _collectablesHUD = FindFirstObjectByType<UICollectables>();
        if (_collectablesHUD == null) return false;

        return true;
    }

    public void InitializeMenus()
    {
        if (!LoadMenus())
        {
            Debug.LogError("Error loading menus.");
            return;
        }

        _titleScreenMenu.Initialize();
    }

    private bool LoadMenus()
    {
        _titleScreenMenu = FindFirstObjectByType<UITitleScreen>();
        if(_titleScreenMenu == null) return false;

        return true;
    }
    #endregion

    public void OnConfirmButtonEvent(Button button)
    {
        PlayConfirmSFX();

        button.interactable = false;

        button.transform.localScale = Vector3.one;
        button.transform.DOScale(_buttonConfirmScale, _buttonTransitionTime)
        .SetEase(Ease.InOutSine)
        .SetLoops(2, LoopType.Yoyo)
        .OnComplete(() => button.transform.DOKill());
    }

    public void OnMouseEnterEvent(GameObject target)
    {
        if (target == null) return;

        target.transform.localScale = Vector3.one;
        Tweener tween = target.transform.DOScale(_buttonSelectScale, _buttonTransitionTime)
            .SetEase(Ease.InOutSine);
        
        PlaySelectAudio();
    }

    public void OnMouseExitEvent(GameObject target)
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
        AudioSfxDef audio = Instantiate(_confirmAudio);
        AudioPool.Play(audio);
    }

    private void PlaySelectAudio()
    {
        AudioSfxDef audio = Instantiate(_selectAudio);
        AudioPool.Play(audio);
    }
    #endregion
}
