using System;
using System.Threading.Tasks;
using Unity.Cinemachine;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [Header("Player prefabs")]
    [SerializeField] private GameObject _playerPrefab;

    [Header("Camera prefabs")]
    [SerializeField] private GameObject _cinemachinePrefab;

    #region Game initialization
    public async Task InitializeGame()
    {
        bool playerLoaded = LoadPlayer();
        if (!playerLoaded) throw new InitializationException("Player not loaded correctly.");

        bool camerasLoaded = LoadCameras();
        if (!camerasLoaded) throw new InitializationException("Cameras not loaded correctly.");

        bool cameraConfigured = SetupPlayerCamera();
        if (!cameraConfigured) throw new InitializationException("Camera not configured correctly.");

        // Small delay just to get rid of the console alert
        await Task.Delay(100);
    }

    private bool LoadPlayer()
    {
        if (_playerPrefab == null)
        {
            Debug.LogError("Player prefab not assinged.");
            return false;
        }

        if (GameContext.I.PlayerRef != null)
        {
            Debug.LogWarning("Player already loaded in.");
            return true;
        }

        GameContext.I.SetPlayerRef(Instantiate(_playerPrefab, Vector3.zero, Quaternion.identity, null));
        return true;
    }

    private bool LoadCameras()
    {
        GameContext.I.SetMainCameraRef(Camera.main);
        if (GameContext.I.MainCameraRef == null)
        {
            Debug.LogError("Main camera not found.");
            return false;
        }

        // Disable audio listener to use the player one instead
        AudioListener cameraListener = GameContext.I.MainCameraRef.GetComponent<AudioListener>();
        if (cameraListener != null) 
        {
            cameraListener.enabled = false;
        }

        if (_cinemachinePrefab == null)
        {
            Debug.LogError("Cinemachine camera prefab not assinged.");
            return false;
        }

        if (GameContext.I.CinemachineRef != null)
        {
            Debug.LogWarning("Cinemachine camera already loaded in.");
            return true;
        }

        GameObject go = Instantiate(_cinemachinePrefab, Vector3.zero, Quaternion.identity, null);
        GameContext.I.SetCinemachineRef(go.GetComponent<CinemachineCamera>());

        return true;
    }

    private bool SetupPlayerCamera()
    {
        Transform cameraTarget = GameContext.I.PlayerRef.GetComponentInChildren<PlayerCameraTarget>().transform;
        if(cameraTarget == null)
        {
            Debug.LogError("Player camera target not found.");
            return false;
        }

        GameContext.I.CinemachineRef.Follow = cameraTarget;
        GameContext.I.CinemachineRef.LookAt = cameraTarget; 

        return true;
    }
    #endregion

    #region Player
    public void TogglePlayerMovement(bool toggle)
    {
        if (GameContext.I.PlayerRef == null) return;

        var locomotion = GameContext.I.PlayerRef.GetComponent<PlayerLocomotion>();
        if (locomotion != null)
        {
            locomotion.ToggleMovement(toggle);
            locomotion.SetHorizontalVelocity(Vector3.zero);
        }
        else
        {
            Debug.LogWarning("PlayerLocomotion not found on PlayerRef.");
        }

        var controller = GameContext.I.PlayerRef.GetComponent<CharacterController>();
        if (controller != null) controller.enabled = toggle;
    }

    public void MovePlayerToSpawn(Transform spawnPoint)
    {
        if (GameContext.I.PlayerRef == null) return;

        GameContext.I.PlayerRef.transform.position = spawnPoint.position;
        GameContext.I.PlayerRef.transform.rotation = spawnPoint.rotation;
    }
    #endregion
}

public class InitializationException : Exception
{
    public InitializationException(string message) : base(message) { }
}
