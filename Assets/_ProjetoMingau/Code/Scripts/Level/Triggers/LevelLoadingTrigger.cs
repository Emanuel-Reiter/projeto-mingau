using UnityEngine;

public class LevelLoadingTrigger : MonoBehaviour
{
    [Header("Objects to Load")]
    [SerializeField] private ReflectionProbe[] _reflectionProbeToLoad;
    [SerializeField] private MeshRenderer[] _meshesToLoad;
    [SerializeField] private GameObject[] _gameObjectsToLoad;

    [Header("Objects to Unload")]
    [SerializeField] private ReflectionProbe[] _reflectionProbesToUnload;
    [SerializeField] private MeshRenderer[] _meshesToUnload;
    [SerializeField] private GameObject[] _gameObjectsToUnload;

    [Header("NPCs to Load")]
    [SerializeField] private GameObject[] _npcsToLoad;
    [SerializeField] private NPCTargetDetection[] _npcsToEnableDetection;

    [Header("NPCs to Unload")]
    [SerializeField] private GameObject[] _npcsToUnload;
    [SerializeField] private NPCTargetDetection[] _npcsToDisableDetection;

    public void Load()
    {
        foreach (ReflectionProbe probe in _reflectionProbeToLoad)
        {
            probe.enabled = true;
        }

        foreach(MeshRenderer mesh in _meshesToLoad)
        {
            mesh.enabled = true;
        }

        foreach (GameObject gameObject in _gameObjectsToLoad)
        {
            gameObject.SetActive(true);
        }

        foreach (GameObject npc in _npcsToLoad)
        {
            npc.SetActive(true);
        }

        foreach (NPCTargetDetection npc in _npcsToEnableDetection)
        {
            npc.ToggleTargetSearch(true);
        }
    }

    public void Unload()
    {
        foreach (ReflectionProbe probe in _reflectionProbesToUnload)
        {
            probe.enabled = false;
        }

        foreach (MeshRenderer mesh in _meshesToUnload)
        {
            mesh.enabled = false;
        }

        foreach (GameObject gameObject in _gameObjectsToUnload)
        {
            gameObject.SetActive(false);
        }

        foreach (GameObject npc in _npcsToUnload)
        {
            npc.SetActive(false);
        }

        foreach (NPCTargetDetection npc in _npcsToDisableDetection)
        {
            npc.ToggleTargetSearch(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other == null) return;
        if(!other.gameObject.CompareTag("Player")) return;

        Load();
        Unload();
    }
}
