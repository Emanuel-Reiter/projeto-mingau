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

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        Initialize();
    }

    public void Initialize()
    {
        _loadingScreen.alpha = 0.0f;
        _loadingScreen.gameObject.SetActive(false);
    }

    private void Update()
    {
        // Temp
        if (Input.GetKeyDown(KeyCode.P)) ReturnToMainMenu();

        float loadingMaxDelta = 1.0f;
        if (GameManager.Instance.GameState == GameStateEnum.Loading)
        {
            _loadingBar.value = Mathf.MoveTowards(_loadingBar.value, _targetLoadingValue, loadingMaxDelta * Time.deltaTime);
        }
    }

    public async void LoadLevel(string sceneName, Action callback)
    {
        if (GameManager.Instance.Player != null) GameManager.Instance.TogglePlayer(false);

        // Wait a bit in order finish playing animations and sfx
        await Task.Delay(500);

        // Resets laoding bar
        _targetLoadingValue = 0.0f;
        _loadingBar.value = 0.0f;
        LoadingScreenFadeIn();

        await Task.Delay(500);

        GameManager.Instance.ChangeGameState(GameStateEnum.Loading);
        GameManager.Instance.ChangeGameContext(GameContextEnum.LoadingScreen);

        var scene = SceneManager.LoadSceneAsync(sceneName);
        scene.allowSceneActivation = false;

        do
        {
            await Task.Delay(100);
            // Added .1 to the progress bar to counter unity .9 scene loading threshold
            _targetLoadingValue = scene.progress + 0.1f;
        }
        while (scene.progress < 0.9f);

        callback?.Invoke();
        scene.allowSceneActivation = true;

        // Wait for scene to be fully active
        await Task.Delay(100);

        // reset player position after scene is fully loaded and active
        if (GameManager.Instance.Player != null)
        {
            Transform spawnPoint = GameObject.FindGameObjectWithTag("LevelSpawnPoint").transform;
            if (spawnPoint != null)
            {
                GameManager.Instance.TogglePlayerMovement(false);

                GameManager.Instance.Player.transform.position = spawnPoint.transform.position;
                GameManager.Instance.Player.transform.rotation = spawnPoint.transform.rotation;
            }

        }

        // Small delay to disable loading screen to
        // ensrure correct loading and avoid screen flasing
        await Task.Delay(2000);

        // Finalize loading
        LoadingScreenFadeOut();

        GameManager.Instance.TogglePlayerMovement(true);
        GameManager.Instance.ChangeGameState(GameStateEnum.Running);
        GameManager.Instance.ChangeGameContext(GameContextEnum.Playing);

        if (GameManager.Instance.Player != null) GameManager.Instance.TogglePlayer(true);
    }

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

    public void StartGame()
    {
        LoadLevel(_startLevelScene, () => GameManager.Instance.InitializeGame());
    }

    public void ReturnToMainMenu()
    {
        LoadLevel(_mainMenuScene, () => GameManager.Instance.UnloadGame());
    }
}
