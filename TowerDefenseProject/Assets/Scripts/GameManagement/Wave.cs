using UnityEngine;

[System.Serializable]
public class Wave
{
    public float NextWaveTimer;
    public int totalEnemiesInWave;
    [NonReorderable]
    public WaveEnemyGroup[] enemyGroupsInWave;
    
}
