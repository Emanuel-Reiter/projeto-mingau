using System;
using System.Threading.Tasks;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum GameStateEnum
{
    Running,
    Loading,
    Paused,
}

public enum GameContextEnum
{
    Playing,
    Dilaogue,
    MainMenu,
    ConfigMenu,
    LoadingScreen,
    CutScene,
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Game state")]
    private GameStateEnum _gameState;
    public GameStateEnum PreviousGameState { get; private set; }
    public GameStateEnum GameState
    {
        get => _gameState;
        set
        {
            PreviousGameState = _gameState;
            _gameState = value;

            OnGameStateChanged?.Invoke(_gameState);

            HandleGameState();
        }
    }

    public delegate void OnGameStateChangedDelegate(GameStateEnum gameState);
    public event OnGameStateChangedDelegate OnGameStateChanged;

    [Header("Game context")]
    private GameContextEnum _gameContext;
    public GameContextEnum PreviousGameContext { get; private set; }
    public GameContextEnum GameContext
    {
        get => _gameContext;
        set
        {
            PreviousGameContext = _gameContext;
            _gameContext = value;

            OnGameContextChanged?.Invoke(_gameContext);
        }
    }

    public delegate void OnGameContextChangedDelegate(GameContextEnum gameContext);
    public event OnGameContextChangedDelegate OnGameContextChanged;


    [Header("Params")]
    [SerializeField] private bool _showLogs = false;

    [Header("UI")]
    [SerializeField] private GameObject _canvasPrefab;
    [SerializeField] private GameObject _eventSystemPrefab;

    public GameObject CanvasRef { get; private set; }
    public GameObject EventSystemRef { get; private set; }
    public HUD HUDRef { get; private set; }
    public InterationPopup InterationPopupRef { get; private set; }
    public CursorLock CursorLockRef { get; private set; }

    [Header("Player")]
    [SerializeField] private GameObject _playerPrefab;
    [SerializeField] private GameObject _cinemachinePrefab;
    public GameObject Player { get; private set; }
    private GameObject _cinemachine;

    [Header("Dependencies")]
    [SerializeField] private Camera _mainCamera;
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
        ChangeGameState(GameStateEnum.Running);
        ChangeGameContext(GameContextEnum.MainMenu);


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
        if (_canvasPrefab != null) InitializeUI();
    }

    private void InitializeUI()
    {
        CanvasRef = Instantiate(_canvasPrefab, null);
        DontDestroyOnLoad(CanvasRef);

        EventSystemRef = Instantiate(_eventSystemPrefab, null);
        DontDestroyOnLoad(EventSystemRef);

        // Activates all UI elements before fetching refferences
        // Pass true as a parameter to get inactive objects
        Transform[] uiDescendants = CanvasRef.GetComponentsInChildren<Transform>(true);
        foreach (Transform t in uiDescendants) t.gameObject.SetActive(true);

        CursorLockRef = CanvasRef.GetComponent<CursorLock>();
        if (CursorLockRef != null) CursorLockRef.Initialize();

        HUDRef = CanvasRef.GetComponentInChildren<HUD>();
        if (HUDRef != null) HUDRef.Initialize();

        InterationPopupRef = HUDRef.GetComponentInChildren<InterationPopup>();
    }

    private void InitializePlayer()
    {
        if (Player != null) return;

        Player = Instantiate(_playerPrefab, Vector3.up, Quaternion.identity, null);
        DontDestroyOnLoad(Player);

        _cinemachine = Instantiate(_cinemachinePrefab, null);
        DontDestroyOnLoad(_cinemachine);

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
        if (!toggle)
        {
            PlayerLocomotion locomotion = Player.GetComponent<PlayerLocomotion>();
            locomotion.SetHorizontalVelocity(Vector3.zero);
        }

        PlayerInputManager input = Player.GetComponent<PlayerInputManager>();
        input.enabled = toggle;

        PlayerInventory inventory = Player.GetComponent<PlayerInventory>();
        inventory.HardResetCollectCombo();
    }
    public void TogglePlayerMovement(bool toggle)
    {
        PlayerLocomotion locomotion = Player.GetComponent<PlayerLocomotion>();
        locomotion.ToggleMovement(toggle);

        CharacterController controller = Player.GetComponent<CharacterController>();
        if (controller == null) return;

        controller.enabled = toggle;
    }

    private void HandleGameState()
    {
        switch (GameState)
        {
            case GameStateEnum.Running:
                Time.timeScale = 1.0f;
                break;

            case GameStateEnum.Paused:
                Time.timeScale = 0.0f;
                break;

            case GameStateEnum.Loading:
                Time.timeScale = 1.0f;
                break;
        }
    }

    private int GUIPositionY(int row, int height) => row * height;

    private void OnGUI()
    {
        int xOffset = 32;
        int width = 512;
        int height = 32;

        GUI.skin.label.fontSize = 24;

        if (!_showLogs) return;

        GUI.Label(new Rect(xOffset, GUIPositionY(1, height), width, height), $"gameState: {GameState}");
        GUI.Label(new Rect(xOffset, GUIPositionY(2, height), width, height), $"gameContext: {GameContext}");
    }

    public void ChangeGameState(GameStateEnum gameState) { GameState = gameState; }
    public void ChangeGameContext(GameContextEnum gameContext) { GameContext = gameContext; }

    public void UnloadGame()
    {
        Destroy(CanvasRef);
        Destroy(EventSystemRef);

        Destroy(_globalTimer);

        Destroy(Player);
        Destroy(_cinemachine);
    }
}
