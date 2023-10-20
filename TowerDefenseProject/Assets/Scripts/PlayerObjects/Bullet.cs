using UnityEngine;

public class Bullet : MonoBehaviour
{

    [Header("Particle System Effects")]
    [SerializeField]
    private GameObject impactEffect;
    [SerializeField]
    private float effectDuration;

    [Header("Bullet Logic Variables")]
    [SerializeField]
    private Transform target;
    [SerializeField]
    private float speed = 70.0f;
    [SerializeField]
    private float explosionRadius = 0.0f;
    [SerializeField]
    private int damage = 50;

    // Update is called once per frame
    void Update()
    {
        if(target == null)
        {
            Destroy(gameObject);
            return;
        }

        Vector3 dir = target.position - transform.position;
        float distanceThisFrame = speed * Time.deltaTime;

        if(dir.magnitude <= distanceThisFrame)
        {
            HitTarget();
            return;
        }
        transform.Translate(dir.normalized * distanceThisFrame, Space.World);
        transform.LookAt(target.transform);
    }
    public void SetSeekingTarget(Transform newTarget)
    {
        target = newTarget;
    }

    private void HitTarget()
    {
        GameObject effectInstance = Instantiate(impactEffect, transform.position, transform.rotation);
        Destroy(effectInstance, effectDuration);
        if(explosionRadius > 0)
        {
            Explode(target);
        }
        else
        {
            Damage(target);
        }
        Destroy(gameObject);
    }


    private void Damage(Transform enemy)
    {
        Enemy e = enemy.GetComponent<Enemy>();
        if(e == null)
        {
            return;
        }    
        e.TakeDamage(damage);
    }

    private void Explode(Transform enemy)
    {
        Collider[] targetsInZone = Physics.OverlapSphere(enemy.position, explosionRadius);
        foreach(Collider collider in targetsInZone)
        {
            if (collider.tag == "Enemy")
            {
                Damage(collider.transform);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
