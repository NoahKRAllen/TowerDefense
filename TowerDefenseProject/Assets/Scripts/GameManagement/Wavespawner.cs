using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Wavespawner : MonoBehaviour
{
    [Header("Required GameObjects")]
    //[SerializeField]
    //private Transform enemyPrefabSlow;
    //[SerializeField]
    //private Transform enemyPrefabFast;
    [SerializeField]
    private Transform spawnPoint;


    [Header("UI Elements")]
    [SerializeField]
    private Text countDownUI;
    [SerializeField]
    private Text waveNumberUI;

    //private List<Transform> enemiesSpawnedInWave = new List<Transform>();
    public static int aliveEnemiesSpawned = 0;
    private int totalEnemiesSpawnedInWave = 0;
    [SerializeField]
    private GameManager gmInstance;

    [Header("Wave Variables")]
    private bool startedSpawningWave = false;
    private float waveCountdown;
    [SerializeField]
    private int currentWave = 0;
    [SerializeField]
    [NonReorderable]
    private Wave[] waves;

    public bool gameIsOver;
    // Start is called before the first frame update
    void Start()
    {
        PlayerPrefs.SetInt("TempPoints", 0);
        countDownUI.text = string.Format("{0:00.00}", waveCountdown);

        waveNumberUI.text = "Wave: 0";
        waveCountdown = waves[currentWave].NextWaveTimer;

        totalEnemiesSpawnedInWave = waves[currentWave].totalEnemiesInWave;
        gameIsOver = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(gameIsOver)
        {
            return;
        }
        if(waveCountdown <= 0.0f)
        {
            PlayerStats.WavesSurvived++;
            Debug.Log("Testing game over toggle");
            if(currentWave < waves.Length)
            {
                waveCountdown = waves[currentWave].NextWaveTimer;
                waveNumberUI.text = "Wave: " + (currentWave + 1).ToString();
                StartCoroutine(SpawnWave());
            }
            else if(isWaveDone())
            {
                gameIsOver = true;
                gmInstance.WonLevel();
                return;
            }
        }
        else if(isWaveDone())
        {
            if(currentWave == waves.Length)
            {
                return;
            }
            waveCountdown -= Time.deltaTime;
            waveCountdown = Mathf.Clamp(waveCountdown, 0f, Mathf.Infinity);
            countDownUI.text = string.Format("{0:00.00}", waveCountdown);
        }
        if(aliveEnemiesSpawned == 0 && startedSpawningWave && waveCountdown <= 0.0f && currentWave < waves.Length)
        {
            startedSpawningWave = false;
            currentWave++;
        }
    }

    private bool isWaveDone()
    {
        if(currentWave == waves.Length && aliveEnemiesSpawned == 0)
        {
            return true;
        }
        else
        {
            return((aliveEnemiesSpawned == 0 && totalEnemiesSpawnedInWave == waves[currentWave].totalEnemiesInWave));
        }
    }

    IEnumerator SpawnWave()
    {
        startedSpawningWave = true;
        totalEnemiesSpawnedInWave = 0;
        for(int i = 0; i < waves[currentWave].enemyGroupsInWave.Length; i++)
        {
            StartCoroutine(SpawnWaveGroup(waves[currentWave].enemyGroupsInWave[i], i));
            yield return new WaitForSecondsRealtime(waves[currentWave].enemyGroupsInWave[i].fullGroupSpawnDelay);
        }
        yield return 0;
    }
    IEnumerator SpawnWaveGroup(WaveEnemyGroup group, int groupNumber)
    {
        
        
        for (int i = 0; i < group.totalEnemiesInGroup; i++)
        {
            Transform holder = Instantiate(group.enemy, spawnPoint.position, spawnPoint.rotation).transform;
            holder.name = "Wave: " + currentWave + " Group: " + groupNumber + " Enemy: " + i;
            //Debug.Log(holder.name);
            holder.GetComponent<Enemy>().WaveVariableModifications(group.healthMultipler, group.speedMultipler, group.worthMultipler);
            holder.GetComponent<MeshRenderer>().material.color = group.enemyColor;
            aliveEnemiesSpawned++;
            totalEnemiesSpawnedInWave++;
            yield return new WaitForSecondsRealtime(group.inGroupSpawnDelay);
        }
    }

    //IEnumerator SpawnWaveMultipleEnemies()
    //{
    //    for (int i = 0; i < currentWave; i++)
    //    {
    //        if(i % 2 == 0)
    //        {
    //            SpawnEnemySlow();
    //            totalSlowEnemiesSpawned++;
    //        }
    //        else
    //        {
    //            SpawnEnemyFast();
    //            totalFastEnemiesSpawned++;
    //        }
    //        yield return new WaitForSecondsRealtime(spawnDelay);
    //    } 
    //}
    //private  void SpawnEnemySlow()
    //{
    //    Transform holder = Instantiate(enemyPrefabSlow, spawnPoint.position, spawnPoint.rotation).transform;
    //  
    //    holder.name = (holder.name + totalSlowEnemiesSpawned.ToString()); 
    //    enemiesInWave.Add(holder);
    //}
    //private void SpawnEnemyFast()
    //{
    //    Transform holder = Instantiate(enemyPrefabFast, spawnPoint.position, spawnPoint.rotation).transform;
    //
    //    holder.name = (holder.name + totalFastEnemiesSpawned.ToString());
    //    enemiesInWave.Add(holder);
    //}
    public static void EnemeyLeftWave(Transform enemy)
    {
        aliveEnemiesSpawned--;
    }
}
