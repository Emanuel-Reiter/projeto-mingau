using UnityEngine;

[CreateAssetMenu(fileName = "New Level", menuName = "Level/Game Level")]
public class Level : ScriptableObject
{
    public string LevelName;
    public string LevelScene;
    public Sprite LevelThumbnail;
    public bool UseScore;
}
