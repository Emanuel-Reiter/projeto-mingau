using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : Singleton<LevelManager>
{
    [SerializeField] private GameObject _playerPrefab;
    public GameObject PlayerRef { get; private set; }

    private LevelData _currentLoadedLevel;

    // On level loading trigger
    public delegate void OnLevelLoadingDelegate();
    public event OnLevelLoadingDelegate OnLevelLoadingChanged;

    private bool _isLevelLoading = false;
    public bool IsLevelLoading
    {
        get => _isLevelLoading;
        set
        {
            OnLevelLoadingChanged?.Invoke();
            _isLevelLoading = value;
        }
    }

    // Level loading percent trigger
    public delegate void OnLevelLoadPercentDelegate();
    public event OnLevelLoadPercentDelegate OnLevelLoadPercentChanged;

    private float _levelLoadPercent = 0f;
    public float LevelLoadPercent
    {
        get => _levelLoadPercent;
        set
        {
            OnLevelLoadPercentChanged?.Invoke();
            _levelLoadPercent = Mathf.Clamp(value, 0f, 1f);
        }
    }

    public async void LoadLevel(LevelData levelToLoad)
    {
        int playerAnimationsSyncUpWaitTime = 500;

        _levelLoadPercent = 0f;
        _isLevelLoading = true;

        await Task.Delay(playerAnimationsSyncUpWaitTime);

        await SceneManager.UnloadSceneAsync(_currentLoadedLevel.SceneName);
        _levelLoadPercent = 0.33f;

        await SceneManager.LoadSceneAsync(levelToLoad.SceneName);
        _levelLoadPercent = 0.67f;

        try
        {
            Transform spawnPoint = GameObject.FindGameObjectWithTag("LevelSpawPoint").transform;
            TogglePlayerMovement(false);
            SetupPlayerSpawnTransform(spawnPoint.position, spawnPoint.rotation);
        }
        catch
        {
            TogglePlayerMovement(false);
            SetupPlayerSpawnTransform(Vector3.zero, Quaternion.identity);
        }
    }

    #region Player
    public void TogglePlayerMovement(bool toggle)
    {
        PlayerLocomotion locomotion = PlayerRef.GetComponent<PlayerLocomotion>();
        locomotion.ToggleMovement(toggle);
        locomotion.SetHorizontalVelocity(Vector3.zero);

        CharacterController controller = PlayerRef.GetComponent<CharacterController>();
        if (controller == null) return;

        controller.enabled = toggle;
    }

    public void SetupPlayerSpawnTransform(Vector3 position, Quaternion rotation)
    {
        PlayerRef.transform.position = position;
        PlayerRef.transform.rotation = rotation;
    }
    #endregion
}
