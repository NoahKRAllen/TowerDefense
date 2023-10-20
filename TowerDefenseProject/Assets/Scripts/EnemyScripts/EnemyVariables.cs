using UnityEngine;
using UnityEngine.UI;
public class EnemyVariables : MonoBehaviour
{
    [SerializeField]
    private float healthMaximum;
    [SerializeField]
    private float healthCurrent;
    [SerializeField]
    private float defaultMaxHealth = 100.0f;

    [SerializeField]
    private float defaultSpeed = 10.0f;
    [SerializeField]
    private float speed;
    [SerializeField]
    private float moddedSpeed;
    [SerializeField]
    private int worth;
    [SerializeField]
    private int defaultWorth = 10;


    [Header("Unity Objects")]
    [SerializeField]
    private Image healthBar;
    [SerializeField]
    private GameObject deathEffect;

    private void OnEnable()
    {
        if(speed == 0.0f)
        {
            moddedSpeed = defaultSpeed;
            speed = defaultSpeed;
        }
        if(healthCurrent == 0.0f)
        {
            healthCurrent = defaultMaxHealth;
        }
        if(healthMaximum == 0.0f)
        {
            healthMaximum = defaultMaxHealth;
        }
        if(worth == 0)
        {
            worth = defaultWorth;
        }
    }
    #region Speed
    public float GetCurrentSpeed()
    {
        return speed;
    }
    public float GetStartSpeed()
    {
        return defaultSpeed;
    }
    public void WaveModStartSpeed(float speedMod)
    {
        moddedSpeed = defaultSpeed * speedMod;
        speed = moddedSpeed;
    }
    public void SlowDown(float percent)
    {
        speed = moddedSpeed * (1f - percent);
    }
    public void ResetSpeed()
    {
        speed = moddedSpeed;
    }
    #endregion
    #region Health
    public float GetCurrentHealth()
    {
        return healthCurrent;
    }
    public float GetMaximumHealth()
    {
        return healthMaximum;
    }
    public void ReduceHealth(float amount)
    {
        healthCurrent -= amount;
        healthBar.fillAmount = healthCurrent/ healthMaximum;
    }
    public void WaveModMaxHealth(float healthMod)
    {
        healthMaximum = defaultMaxHealth;
        healthMaximum *= healthMod;
        healthCurrent = healthMaximum;
    }
    #endregion
    #region Worth
    public int GetWorth()
    {
        return worth;
    }
    public void WaveModWorth(int worthMod)
    {
        worth = defaultWorth;
        worth *= worthMod;
    }
    #endregion
    public GameObject GetDeathEffect()
    {
        return deathEffect;
    }

    /*Trimmed until better targeting can be handled
    public void AmCurrentTarget()
    {
        gameObject.GetComponent<MeshRenderer>().material.color = Color.red;
    }
    public void AddDistance(float moved)
    {
        currentDistance += moved;
    }
    public void InRange()
    {
        gameObject.GetComponent<MeshRenderer>().material.color = Color.green;
    }
    public void LeftRange()
    {
        gameObject.GetComponent<MeshRenderer>().material.color = Color.blue;
    }

    public float GetCurrentDistance()
    {
        return currentDistance;
    }
    */
}
