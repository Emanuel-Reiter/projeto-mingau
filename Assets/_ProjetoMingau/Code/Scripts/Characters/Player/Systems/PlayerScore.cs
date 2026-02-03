using System.Collections.Generic;
using UnityEngine;

public class PlayerScore : MonoBehaviour
{
    private PlayerDependencies _dependencies;

    private List<LevelScore> _levelScore = new List<LevelScore>();

    private int _currentLevelTime = 0;
    private int _currentLevelCollectables = 0;
    private int _currentLevelEnemiesDefeated = 0;

    private void Start()
    {
        _dependencies = GetComponent<PlayerDependencies>();

        _dependencies.Inventory.OnLevelCollectablesChanged += UpdateCurrentLevelCollectables;
    }

    private void UpdateCurrentLevelCollectables()
    {
        _currentLevelCollectables = _dependencies.Inventory.LevelColletables;
    }

}
