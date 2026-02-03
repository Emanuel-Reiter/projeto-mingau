using UnityEngine;

public class LevelScore
{
    private Level _level;
    private int _score;
    private float _completionTime;
    private int _collectables;
    private int _enemiesDefeated;

    public LevelScore(Level level, float completionTime, int collectedGears, int enemiesDefeated)
    {
        _level = level;
        _completionTime = completionTime;
        _enemiesDefeated = enemiesDefeated;
        _collectables = collectedGears;

        _score = CalculateFinalScore();
    }

    private int CalculateFinalScore()
    {
        int finalScore = _collectables + (_enemiesDefeated * 2);
        return finalScore;
    }
}
