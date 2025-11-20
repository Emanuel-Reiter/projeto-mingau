using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [Header("Audio")]
    [SerializeField] private AudioSource _uiAudioSource;

    [Header("Audio clips")]
    [SerializeField] private AudioClip _selectAudio;
    [SerializeField] private AudioClip _confrimAudio;

    private void Awake()
    {
        // Don't destroy on load settup
        transform.parent = null;
        DontDestroyOnLoad(gameObject);

        // Instance initialization
        if (Instance != null && Instance != this) Destroy(gameObject);
        else Instance = this;
    }

    public void PlaySelectSFX()
    {
        _uiAudioSource.clip = _selectAudio;
        _uiAudioSource.Play();
    }

    public void PlayConfirmSFX()
    {
        _uiAudioSource.clip = _confrimAudio;
        _uiAudioSource.Play();
    }
}
