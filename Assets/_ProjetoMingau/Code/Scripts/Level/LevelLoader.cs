using DG.Tweening;
using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelLoader : MonoBehaviour
{
    public static LevelLoader Instance { get; private set; }

    [Header("Loading screen")]
    [SerializeField] private CanvasGroup _loadingScreen;
    [SerializeField] private Slider _loadingBar;

    private float _targetLoadingValue = 0.0f;

    private float _loadingScreenTransitionTime = 0.333f;

    [Header("Level")]
    [SerializeField] private string _mainMenuScene;
    [SerializeField] private string _startLevelScene;

    private void Awake()
    {
        // Don't destroy on load settup
        transform.parent = null;
        DontDestroyOnLoad(gameObject);

        // Instance initialization
        if (Instance != null && Instance != this) Destroy(gameObject);
        else Instance = this;

        Initialize();
    }

    private void Initialize()
    {
        // Canvas setup
        _loadingScreen.alpha = 0.0f;
        _loadingScreen.gameObject.SetActive(false);
    }

    private void Update()
    {
        // Temp main menu return logic
        bool canReturn = GameManager.Instance.CurrentGameContext != GameContext.Cutscene
            && GameManager.Instance.CurrentGameContext != GameContext.MainMenu
            && GameManager.Instance.CurrentGameContext != GameContext.LoadingScreen;

        if (Input.GetKeyDown(KeyCode.Escape) && canReturn) ReturnToMainMenu();

        float loadingMaxDelta = 1.0f;
        if (GameManager.Instance.CurrentGameState == GameState.Loading)
        {
            _loadingBar.value = Mathf.MoveTowards(_loadingBar.value, _targetLoadingValue, loadingMaxDelta * Time.deltaTime);
        }
    }

    public async void LoadLevel(string sceneName, Action callback, GameContext nextLevelContext)
    {
        // Loading delay in miliseconds setup
        int loadDelayShort = 250;
        int loadDelayMedium = 500;
        int loadDelayLong = 2000;

        if (GameManager.Instance.Player != null) GameManager.Instance.TogglePlayer(false);

        // Wait in order finish playing animations and sfx
        await Task.Delay(loadDelayMedium);

        // Reset laoding bar
        _targetLoadingValue = 0.0f;
        _loadingBar.value = 0.0f;

        LoadingScreenFadeIn();
        GameManager.Instance.ChangeGameState(GameState.Loading);
        GameManager.Instance.ChangeGameContext(GameContext.LoadingScreen);

        await Task.Delay(loadDelayMedium);

        var scene = SceneManager.LoadSceneAsync(sceneName);
        scene.allowSceneActivation = false;

        do
        {
            await Task.Delay(loadDelayShort);
            // Added .1 to the progress bar to counter unity .9 scene loading threshold
            _targetLoadingValue = scene.progress + 0.1f;
        }
        while (scene.progress < 0.9f);

        callback?.Invoke();
        scene.allowSceneActivation = true;

        // Wait for scene to be fully active
        await Task.Delay(loadDelayShort);

        if (GameManager.Instance.Player != null)
        {
            // Reset player position after scene is fully loaded and active
            try
            {
                Transform spawnPoint = GameObject.FindGameObjectWithTag("LevelSpawnPoint").transform;

                GameManager.Instance.TogglePlayerMovement(false);
                GameManager.Instance.Player.transform.position = spawnPoint.transform.position;
                GameManager.Instance.Player.transform.rotation = spawnPoint.transform.rotation;

            }
            catch
            {
                // Fallback in case level spawn point wasn't locate properly
                GameManager.Instance.TogglePlayerMovement(false);
                GameManager.Instance.Player.transform.position = Vector3.zero;
                GameManager.Instance.Player.transform.rotation = Quaternion.identity;
            }
        }

        // Longer delay to ensrure correct level loading and avoid screen flasing
        await Task.Delay(loadDelayLong);

        // Finalize loading
        LoadingScreenFadeOut();

        if (GameManager.Instance.Player != null) GameManager.Instance.TogglePlayer(true);
        if (GameManager.Instance.Player != null) GameManager.Instance.TogglePlayerMovement(true);

        GameManager.Instance.ChangeGameState(GameState.Running);
        GameManager.Instance.ChangeGameContext(nextLevelContext);
    }

    #region Global routing
    public void StartGame()
    {
        LoadLevel(_startLevelScene, () => GameManager.Instance.InitializeGame(), GameContext.Cutscene);
    }

    public void ReturnToMainMenu()
    {
        LoadLevel(_mainMenuScene, () => GameManager.Instance.UnloadGame(), GameContext.MainMenu);
    }
    #endregion

    #region UI Fade
    public void LoadingScreenFadeIn()
    {
        _loadingScreen.gameObject.SetActive(true);
        _loadingScreen.alpha = 0.0f;

        _loadingScreen.DOFade(1.0f, _loadingScreenTransitionTime)
            .SetEase(Ease.InOutSine)
            .OnComplete(() =>
            {
                _loadingScreen.alpha = 1.0f;
            });
    }

    public void LoadingScreenFadeOut()
    {
        _loadingScreen.alpha = 1.0f;
        _loadingScreen.DOFade(0.0f, _loadingScreenTransitionTime)
            .SetEase(Ease.InOutSine)
            .OnComplete(() =>
            {
                _loadingScreen.alpha = 0.0f;
                _loadingScreen.gameObject.SetActive(false);
            });
    }
    #endregion
}
