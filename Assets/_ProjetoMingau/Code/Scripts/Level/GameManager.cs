using System;
using System.Threading.Tasks;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum GameState
{
    Running,
    Loading,
    Paused,
}

public enum GameContext
{
    Playing,
    Dilaogue,
    MainMenu,
    GeneralMenu,
    LoadingScreen,
    Cutscene,
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Game state")]
    private GameState _currentGameState;
    public GameState PreviousGameState { get; private set; }
    public GameState CurrentGameState
    {
        get => _currentGameState;
        set
        {
            PreviousGameState = _currentGameState;
            _currentGameState = value;

            OnGameStateChanged?.Invoke(_currentGameState);

            HandleGameState();
        }
    }

    public delegate void OnGameStateChangedDelegate(GameState gameState);
    public event OnGameStateChangedDelegate OnGameStateChanged;

    [Header("Game context")]
    private GameContext _currentGameContext;
    public GameContext PreviousGameContext { get; private set; }
    public GameContext CurrentGameContext
    {
        get => _currentGameContext;
        set
        {
            PreviousGameContext = _currentGameContext;
            _currentGameContext = value;

            OnGameContextChanged?.Invoke(_currentGameContext);
        }
    }
    // Evenet callbacks
    public delegate void OnGameContextChangedDelegate(GameContext gameContext);
    public event OnGameContextChangedDelegate OnGameContextChanged;

    [Header("Params")]
    [SerializeField] private bool _showLogs = false;

    [Header("UI")]
    [SerializeField] private GameObject _playerCanvasPrefab;
    [SerializeField] private GameObject _playerEventSystemPrefab;

    public GameObject PlayerCanvas { get; private set; }
    public GameObject PlayerEventSystem { get; private set; }
    public HUD HUD { get; private set; }
    public InteractionPrompt InterationPrompt { get; private set; }
    public CursorLock CursorLock { get; private set; }

    [Header("Player")]
    [SerializeField] private GameObject _playerPrefab;
    [SerializeField] private GameObject _cinemachinePrefab;
    public GameObject Player { get; private set; }
    private GameObject _cinemachine;

    [Header("Dependencies")]
    private Camera _mainCamera;
    [SerializeField] private GameObject _globalTimerPrefab;
    private GameObject _globalTimer;

    private void Awake()
    {
        // Don't destroy on load settup
        transform.parent = null;
        DontDestroyOnLoad(gameObject);

        // Instance initialization
        if (Instance != null && Instance != this) Destroy(gameObject);
        else Instance = this;

        // Set start game state and context
        ChangeGameState(GameState.Running);
        ChangeGameContext(GameContext.MainMenu);
    
        // Initialize main camera
        _mainCamera = Camera.main;

        // Instantiate the global timer
        try
        {
            if (_globalTimerPrefab != null) _globalTimer = Instantiate(_globalTimerPrefab, null);
            DontDestroyOnLoad(_globalTimer);
        }
        catch
        {
            Debug.LogError("Missing global timer object reference.");
        }
    }

    public void InitializeGame()
    {
        if (_playerPrefab != null) InitializePlayer();
        if (_playerCanvasPrefab != null) InitializePlayerUI();
    }

    private void InitializePlayerUI()
    {
        PlayerCanvas = Instantiate(_playerCanvasPrefab, null);
        DontDestroyOnLoad(PlayerCanvas);

        PlayerEventSystem = Instantiate(_playerEventSystemPrefab, null);
        DontDestroyOnLoad(PlayerEventSystem);

        // Activates all UI elements before fetching refferences
        // Pass true as a parameter in GetComponentsInChildren to get inactive objects as well
        Transform[] uiDescendants = PlayerCanvas.GetComponentsInChildren<Transform>(true);
        foreach (Transform t in uiDescendants) t.gameObject.SetActive(true);

        CursorLock = PlayerCanvas.GetComponent<CursorLock>();
        if (CursorLock != null) CursorLock.Initialize();

        HUD = PlayerCanvas.GetComponentInChildren<HUD>();
        if (HUD != null) HUD.Initialize();

        InterationPrompt = HUD.GetComponentInChildren<InteractionPrompt>();
    }

    private void InitializePlayer()
    {
        if (Player != null) return;

        Player = Instantiate(_playerPrefab, Vector3.up, Quaternion.identity, null);
        DontDestroyOnLoad(Player);

        _cinemachine = Instantiate(_cinemachinePrefab, null);
        DontDestroyOnLoad(_cinemachine);

        // Cinemachine camera setup
        try
        {
            Transform cameraTarget = GameObject.FindGameObjectWithTag("PlayerCameraTarget").transform;

            CinemachineCamera cinemachineCamera = _cinemachine.GetComponent<CinemachineCamera>();
            cinemachineCamera.Follow = cameraTarget;
            cinemachineCamera.LookAt = cameraTarget;
        }
        catch
        {
            Debug.LogError("Player camera target not found!");
        }
    }

    public void TogglePlayer(bool toggle)
    {
        PlayerInputManager input = Player.GetComponent<PlayerInputManager>();
        input.enabled = toggle;

        PlayerInventory inventory = Player.GetComponent<PlayerInventory>();
        inventory.HardResetCollectCombo();
    }

    public void TogglePlayerMovement(bool toggle)
    {
        PlayerLocomotion locomotion = Player.GetComponent<PlayerLocomotion>();
        locomotion.ToggleMovement(toggle);
        locomotion.SetHorizontalVelocity(Vector3.zero);

        CharacterController controller = Player.GetComponent<CharacterController>();
        if (controller == null) return;

        controller.enabled = toggle;
    }

    private void HandleGameState()
    {
        switch (CurrentGameState)
        {
            case GameState.Running:
                Time.timeScale = 1.0f;
                break;

            case GameState.Paused:
                Time.timeScale = 0.0f;
                break;

            case GameState.Loading:
                Time.timeScale = 1.0f;
                break;
        }
    }
    public void ChangeGameState(GameState gameState) { CurrentGameState = gameState; }
    public void ChangeGameContext(GameContext gameContext) { CurrentGameContext = gameContext; }

    #region Debug
    private int GUIPositionY(int row, int height) => row * height;
    
    private void OnGUI()
    {
        int xOffset = 32;
        int width = 512;
        int height = 32;

        GUI.skin.label.fontSize = 24;

        if (!_showLogs) return;

        GUI.Label(new Rect(xOffset, GUIPositionY(1, height), width, height), $"gameState: {CurrentGameState}");
        GUI.Label(new Rect(xOffset, GUIPositionY(2, height), width, height), $"gameContext: {CurrentGameContext}");
    }

    public void UnloadGame()
    {
        Destroy(PlayerCanvas);
        Destroy(PlayerEventSystem);

        Destroy(_globalTimer);

        Destroy(Player);
        Destroy(_cinemachine);
    }
    #endregion
}
