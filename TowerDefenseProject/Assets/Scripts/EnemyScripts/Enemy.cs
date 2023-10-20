using UnityEngine;

public class Enemy : MonoBehaviour
{
    

    private Transform target;
    private int waypointIndex = 0;
    private bool reachedEndOfPath = false;
    private EnemyVariables ownVariables;
    private void Start()
    {
        if(ownVariables == null)
        {
            ownVariables = gameObject.GetComponent<EnemyVariables>();
        }
        target = Waypoints.waypoints[waypointIndex];
    }

    private void Update()
    {
        Vector3 dir = target.position - transform.position;
        transform.Translate(dir.normalized * ownVariables.GetCurrentSpeed() * Time.deltaTime,Space.World);
        if(Vector3.Distance(transform.position, target.position) < .4f && !reachedEndOfPath)
        {
            GetNextWaypoint();
        }
        ownVariables.ResetSpeed();
    }

    public void TakeDamage(float amount)
    {
        ownVariables.ReduceHealth(amount);
        if(ownVariables.GetCurrentHealth() <= 0)
        {
            Die();
        }
    }
    public void Slow(float slowPercent)
    {
        ownVariables.SlowDown(slowPercent);
    }
    private void Die()
    {
        PlayerStats.Currency += ownVariables.GetWorth();
        PlayerPrefs.SetInt("TempPoints", PlayerPrefs.GetInt("TempPoints") + 1);
        
        GameObject effect = Instantiate(ownVariables.GetDeathEffect(), transform.position, Quaternion.identity);
        Destroy(effect, 5f);
        Destroy(gameObject);
    }

    private void GetNextWaypoint()
    {
        if(waypointIndex >= Waypoints.waypoints.Length - 1)
        {
            EndPath();
            reachedEndOfPath = true;
            return;
        }
        waypointIndex++;
        target = Waypoints.waypoints[waypointIndex];

    }
    private void OnDestroy()
    {
        Wavespawner.EnemeyLeftWave(transform);
    }

    private void EndPath()
    {
        Destroy(gameObject);
        PlayerStats.Lives--;
    }
    
    public void WaveVariableModifications(float lifeMod, float speedMod, int worthMod)
    {
        ownVariables = gameObject.GetComponent<EnemyVariables>();
        ownVariables.WaveModMaxHealth(lifeMod);
        ownVariables.WaveModStartSpeed(speedMod);
        ownVariables.WaveModWorth(worthMod);
    }

    public float GetModifiedHealth()
    {
        ownVariables = gameObject.GetComponent<EnemyVariables>();
        return ownVariables.GetCurrentHealth();
    }
}
