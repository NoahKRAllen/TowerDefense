using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    [Header("General")]
    public float range = 15.0f;
    [SerializeField]
    private Transform target;
    [SerializeField]
    private Enemy targetFunctions;

    [Header("Required Setup")]
    [SerializeField]
    private float turnSpeed = 10.0f;
    [SerializeField]
    private Transform partToRotate;
    [SerializeField]
    private Transform firePoint;
    [SerializeField]
    private string enemyTag = "Enemy";


    [Header("Use Bullets (Default)")]
    [SerializeField]
    private GameObject bulletPrefab;
    public float fireRate = 1.0f;
    [SerializeField]
    private float fireCountdown = 0.0f;

    [Header("Use Laser")]
    [SerializeField]
    private bool useLaser = false;
    [SerializeField]
    private LineRenderer lineRenderer;
    [SerializeField]
    private ParticleSystem laserImpactEffect;
    [SerializeField]
    private Light impactLight;
    [SerializeField]
    private float damageOverTime = 30.0f;
    [SerializeField]
    private float slowPercent = .5f;
    
    private void Start()
    {
        InvokeRepeating("UpdateTargeting", 0.0f, 0.5f);
    }

    private void UpdateTargeting()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);
        float shortestDist = Mathf.Infinity;
        GameObject nearestEnemy = null;
        foreach(GameObject enemy in enemies)
        {
            float distToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
            if(shortestDist > distToEnemy)
            {
                shortestDist = distToEnemy;
                nearestEnemy = enemy;
            }
        }

        if (nearestEnemy != null && shortestDist <= range)
        {
            target = nearestEnemy.transform;
            targetFunctions = nearestEnemy.GetComponent<Enemy>();
            return;
        }
        target = null;
        targetFunctions = null;
    }

    private void Update()
    {
        if(!useLaser)
        {
            fireCountdown -= Time.deltaTime;
        }
        if(target == null)
        {
            if (lineRenderer)
            {
                lineRenderer.enabled = false;
                laserImpactEffect.Stop();
                impactLight.enabled = false;
            }
            return; 
        }

        LockOnTarget();

        if(useLaser)
        {
            LaserFire();
            return;
        }

        if (fireCountdown <= 0.0f)
        {
            Shoot();
            fireCountdown = 1 / fireRate;
        }
    }
    
    private void LockOnTarget()
    {
        Vector3 dir = target.position - transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(dir);
        Vector3 rotation = Quaternion.Lerp(partToRotate.rotation, lookRotation, Time.deltaTime * turnSpeed).eulerAngles;
        partToRotate.rotation = Quaternion.Euler(0f, rotation.y, 0f);
    }

    private void LaserFire()
    {
        targetFunctions.TakeDamage(damageOverTime * Time.deltaTime);
        targetFunctions.Slow(slowPercent);
        if (!lineRenderer.enabled)
        {
            lineRenderer.enabled = true;
            laserImpactEffect.Play();
            impactLight.enabled = true;
        }
        lineRenderer.SetPosition(0, firePoint.position);
        lineRenderer.SetPosition(1, target.position);

        Vector3 dir = firePoint.position - transform.position;

        laserImpactEffect.transform.position = target.position + dir.normalized;

        laserImpactEffect.transform.rotation = Quaternion.LookRotation(dir);

    }

    private void Shoot()
    {
        GameObject bulletGO = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Bullet bulletSpawned = bulletGO.GetComponent<Bullet>();
        if (bulletSpawned != null)
        {
            bulletSpawned.SetSeekingTarget(target);
        }
    }



    /* Removed for now until I can figure out a better way to handle zone detection that works
    [Header("Target GameObjects")]
    [SerializeField]
    private Transform currentTarget;
    [SerializeField]
    private Transform previousTarget;
    
    
    [SerializeField]
    private TurretRangeControl rangeFinder;


    [Header("Logic Control")]
    [SerializeField]
    private bool invertTargetingCommand;
    public void swapTargetingInversion()
    {
        invertTargetingCommand = !invertTargetingCommand;
    }

    private List<Transform> targets = new List<Transform>();

    //Don't need a layermask for checks until game gets more enemy types that we need to sort through
    //public LayerMask enemyIdentifier;

    //These are the different targeting variants I wish to have in the game, currently working on getting code setup to handle it
    enum TargetingVariation
    { 
        DISTANCE,
        SPEED,
        CURRENTHEALTH,
        MAXIMUMHEALTH
    };

    [SerializeField]
    private TargetingVariation currentVariation = new TargetingVariation();

    // Start is called before the first frame update
    void Start()
    {
        rangeFinder.UpdateRange(range);
    }

    // Update is called once per frame
    void Update()
    {
        if(currentTarget == null)
        {
            if(targets.Count > 0)
            {
                targets.RemoveAt(0);
                currentTarget = targets[0];
            }
            else
            {
                return;
            }
        }
        Vector3 dir = currentTarget.position - transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(dir);
        Vector3 rotation = Quaternion.Lerp(partToRotate.rotation, lookRotation, Time.deltaTime * turnSpeed).eulerAngles;
        partToRotate.rotation = Quaternion.Euler(0f, rotation.y, 0f);

        if(fireCountdown <= 0.0f)
        {
            Shoot();
            fireCountdown = 1 / fireRate;
        }
        fireCountdown -= Time.deltaTime;
    }
    #region Target Tracking
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }
    private void SwapTarget()
    {
        Debug.Log("New target is: " + targets[0]);
        targets[0].GetComponent<EnemyVariables>().AmCurrentTarget();
        if(previousTarget != null)
        {
            previousTarget.GetComponent<EnemyVariables>().InRange();
        }
        currentTarget = targets[0];
    }
    public void EnteredRange(Transform other)
    {
        other.GetComponent<EnemyVariables>().InRange();
        CheckPotentialNewTarget(other);
    }
    public void ExitedRange(Transform other)
    {
        targets.Remove(other);
        if(targets.Count == 0)
        {
            return;
        }
        if(other != currentTarget)
        {
            return;
        }
        previousTarget = other;
        SwapTarget();
        other.GetComponent<EnemyVariables>().LeftRange();
    }

    //TODO: Fix current target not moving to next down the list when current target is destroyed
    private void KilledTarget(GameObject other)
    {
        Debug.Log("Killing " + other.name);
        targets.Remove(other.transform);
        if (targets.Count == 0)
        {
            Debug.Log("Targets are empty");
            return;
        }
        previousTarget = null;
        SwapTarget();
    }

    //This check currently breaks if I have enemies that are faster than others, as I'm not checking distances except for when they first enter the zone.
    private void CheckPotentialNewTarget(Transform incomingTarget)
    {
        if(targets.Count == 0)
        {
            targets.Add(incomingTarget.transform);
            SwapTarget();
            return;
        }
        FullEnemyComparison(incomingTarget);
    }

    private void FullEnemyComparison(Transform incomingTarget)
    {
        //Will be coming back to figure out rest of these variations, continuing down the tutorial now.
        switch (currentVariation)
        {
            case TargetingVariation.DISTANCE:
                CyclingThroughTargetList(incomingTarget.GetComponent<EnemyVariables>());
                break;
            case TargetingVariation.SPEED:
                break;
            case TargetingVariation.CURRENTHEALTH:
                break;
            case TargetingVariation.MAXIMUMHEALTH:
                break;
            default:
                break;
        }
    }

    private void CyclingThroughTargetList(EnemyVariables incomingTarget)
    {
        for (int targetIndex = 0; targetIndex < targets.Count; targetIndex++)
        {
            if (TargetComparison(targets[targetIndex].GetComponent<EnemyVariables>().GetCurrentDistance(), incomingTarget.GetCurrentDistance()))
            {
                InsertNewTargetIntoList(targetIndex, incomingTarget.transform);
                return;
            }
        }
        targets.Add(incomingTarget.transform);
    }

    private void InsertNewTargetIntoList(int targetIndex, Transform incomingTarget)
    {
        if(targets[targetIndex] != null)
        {
            previousTarget = targets[targetIndex];
        }
        targets.Insert(targetIndex, incomingTarget);
        if(targetIndex == 0)
        {
            SwapTarget();
        }
    }

    private bool TargetComparison(float previousTargetValue, float checkingTargetValue)
    {
        if(invertTargetingCommand)
        {
            float swapHolder = previousTargetValue;
            previousTargetValue = checkingTargetValue;
            checkingTargetValue = swapHolder;
        }
        if (previousTargetValue < checkingTargetValue)
        {
            return true;
        }
        return false;
    }
    #endregion

    private void Shoot()
    {
        GameObject bulletGO = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Bullet bulletSpawned = bulletGO.GetComponent<Bullet>();
        if(bulletSpawned != null)
        {
            bulletSpawned.SetSeekingTarget(currentTarget);

            KilledTarget(currentTarget.gameObject);
        }
    }
    */

}
