using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretRangeControl : MonoBehaviour
{
    [SerializeField]
    Turret turretControl;

    public void UpdateRange(float newRangeRadius)
    {
        if(newRangeRadius > 0.0f)
        {
            newRangeRadius *= 2;
            transform.localScale = new Vector3(newRangeRadius, newRangeRadius, newRangeRadius);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //turretControl.EnteredRange(other.transform);
    }
    private void OnTriggerExit(Collider other)
    {
        //turretControl.ExitedRange(other.transform);
    }
}
