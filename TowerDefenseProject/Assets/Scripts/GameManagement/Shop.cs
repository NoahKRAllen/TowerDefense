using UnityEngine;

public class Shop : MonoBehaviour
{
    [SerializeField]
    private TurretBlueprint standardTurret;
    [SerializeField]
    private TurretBlueprint rocketTurret;
    [SerializeField]
    private TurretBlueprint laserTurret;

    BuildManager buildmanager;

    private void Start()
    {
        buildmanager = BuildManager.bmInstance;
    }

    public void SelectStandardTurret()
    {
        Debug.Log("Standard turret selected");
        buildmanager.SetTurretToBuild(standardTurret);
    }
    public void SelectRocketTurret()
    {
        Debug.Log("Rocket turret selected");
        buildmanager.SetTurretToBuild(rocketTurret);
    }
    public void SelectLaserTurret()
    {
        Debug.Log("Laser turret selected");
        buildmanager.SetTurretToBuild(laserTurret);
    }
}
