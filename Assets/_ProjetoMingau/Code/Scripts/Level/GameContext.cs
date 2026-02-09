using Unity.Cinemachine;
using UnityEngine;

public class GameContext : Singleton<GameContext>
{
    public CinemachineCamera CinemachineRef { get; private set; }
    public void SetCinemachineRef(CinemachineCamera value) { CinemachineRef = value; }
    
    public Camera MainCameraRef { get; private set; }
    public void SetMainCameraRef(Camera value) { MainCameraRef = value; }

    public GameObject PlayerRef { get; private set; }
    public void SetPlayerRef(GameObject value) { PlayerRef = value; }

    public PlayerInteract PlayerInteract { get; private set; }
    public PlayerActionCombo PlayerCombo { get; private set; }
    public PlayerSimpleInventory PlayerInventory { get; private set; }


    public void LoadPlayerRefs()
    {
        PlayerInteract = PlayerRef.GetComponent<PlayerInteract>();
        PlayerCombo = PlayerRef.GetComponent<PlayerActionCombo>();
        PlayerInventory = PlayerRef.GetComponent<PlayerSimpleInventory>();
    }
}
