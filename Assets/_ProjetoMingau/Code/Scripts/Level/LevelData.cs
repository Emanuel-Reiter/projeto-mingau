using UnityEngine;

[CreateAssetMenu(menuName = "Game/Level Data")]
public class LevelData : ScriptableObject
{
    [SerializeField] private string _sceneName;
    public string SceneName => _sceneName;

    public bool IsValid => !string.IsNullOrWhiteSpace(_sceneName);
}
