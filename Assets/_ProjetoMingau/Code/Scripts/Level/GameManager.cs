using UnityEngine.SceneManagement;
using UnityEngine;
using System.Collections.Generic;

public enum GameStateEnum
{
    Running,
    Loading,
    Paused,
    GameMenu,
    MainMenu
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
            PreviousGameState = GameState;
            _gameState = value;

            OnGameStateChanged?.Invoke(_gameState);

            HandleGameState();
        }
    }

    public delegate void OnGameStateChangedDelegate(GameStateEnum gameState);
    public event OnGameStateChangedDelegate OnGameStateChanged;

    [Header("UI")]
    [SerializeField] private GameObject _UIPrefab;
    [SerializeField] private GameObject _UIEventPrefab;

    private GameObject _ui;
    private GameObject _uiEvent;
    private HUD _hud;
    private CursorLock _cursorLock;
    private MainMenu _mainMenu;

    [Header("Player")]
    [SerializeField] private GameObject _playerPrefab;

    private GameObject _player;

    [Header("Level")]
    [SerializeField] private Scene _startLevel;

    private void Awake()
    {
        // Don't destroy on load settup
        transform.parent = null;
        DontDestroyOnLoad(gameObject);

        // Instance initialization
        if (Instance != null && Instance != this) Destroy(this);
        else Instance = this;

        // Set start game state
        GameState = GameStateEnum.Running;

        InitializeGame();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P)) 
        {
            if (GameState != GameStateEnum.Running)
            {
                Debug.Log("Changing game state to running!");
                GameState = GameStateEnum.Running;
            }
            else
            {
                Debug.Log("Changing game state to menu!");
                GameState = GameStateEnum.MainMenu;
            }
        }
    }

    private void InitializeGame()
    {
        if (_UIPrefab != null) InitializeUI();
        if (_playerPrefab != null) InitializePlayer();
    }

    private void InitializeUI()
    {
        _ui = Instantiate(_UIPrefab, null);
        DontDestroyOnLoad(_ui);

        _uiEvent = Instantiate(_UIEventPrefab, null);
        DontDestroyOnLoad(_uiEvent);

        // Activates all UI elements before fetching refferences
        // Pass true as a parameter to get inactive objects
        Transform[] uiDescendants = _ui.GetComponentsInChildren<Transform>(true); 
        foreach (Transform t in uiDescendants) t.gameObject.SetActive(true);

        _cursorLock = _ui.GetComponent<CursorLock>();
        if (_cursorLock != null) _cursorLock.Initialize();

        _hud = _ui.GetComponentInChildren<HUD>();
        if (_hud != null) _hud.Initialize();

        _mainMenu = _ui.GetComponentInChildren<MainMenu>();
        if (_mainMenu != null) _mainMenu.Initialize();
    }

    private void InitializePlayer()
    {
        _player = Instantiate(_playerPrefab, null);
        DontDestroyOnLoad(_player);

        _player.SetActive(false);
    }

    public void StartGame()
    {
        ChangeGameState(GameStateEnum.Running);

        SceneManager.LoadScene(_startLevel.name);

        _player.SetActive(true);
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

            case GameStateEnum.GameMenu:
                Time.timeScale = 1.0f;
                break;

            case GameStateEnum.MainMenu:
                Time.timeScale = 1.0f;
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
        
        bool useLogs = false; 
        if (!useLogs) return;
        
        GUI.Label(new Rect(xOffset, GUIPositionY(1, height), width, height), $"gameState: {GameState}");
    }

    public void ChangeGameState(GameStateEnum gameState) { GameState = gameState; }
}
