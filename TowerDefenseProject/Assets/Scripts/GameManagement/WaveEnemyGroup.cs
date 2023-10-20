using UnityEngine;

[System.Serializable]
public class WaveEnemyGroup
{
    public GameObject enemy;
    public int totalEnemiesInGroup;
    public float inGroupSpawnDelay;
    public float fullGroupSpawnDelay;
    public float healthMultipler;
    public int worthMultipler;
    public float speedMultipler;
    public Color enemyColor;
}
