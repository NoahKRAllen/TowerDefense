using UnityEngine;

public class BuildManager : MonoBehaviour
{
    public static BuildManager bmInstance;


    private void Awake()
    {
        if (bmInstance != null)
        {
            Debug.Log("More than one build manager in scene.");
            return; 
        }
        bmInstance = this;
    }
    [SerializeField]
    private GameObject standardTurretPrefab;
    [SerializeField]
    private GameObject upgradedTurretPrefab;
    [SerializeField]
    private GameObject rocketTurretPrefab;
    [SerializeField]
    private GameObject laserTurretPrefab;
    public GameObject buildEffect;
    public GameObject sellEffect;
    [SerializeField]
    private NodeUI nodeUI;
    private Node selectedNode;

    private void Start()
    {
        turretToBuild = null;
    }

    private TurretBlueprint turretToBuild;

    public bool CanBuild { get { return turretToBuild != null; } }
    public bool HasCurrency { get { return PlayerStats.Currency >= turretToBuild.cost; } }
    public void SetTurretToBuild(TurretBlueprint newTurret)
    {
        turretToBuild = newTurret;

        DeselectNode();
    }
    public void SelectNode(Node node)
    {
        if(selectedNode == node)
        {
            DeselectNode();
            return;
        }

        selectedNode = node;
        turretToBuild = null;

        nodeUI.SetTarget(node);
    }
    public void DeselectNode()
    {
        selectedNode = null;
        nodeUI.Hide();
    }
    public TurretBlueprint GetTurretToBuild()
    {
        return turretToBuild;
    }
    public void BuildTurretOn(Node node)
    {

        if(PlayerStats.Currency < turretToBuild.cost)
        {
            Debug.Log("Not enough Moolah to build");
            return;
        }

        PlayerStats.Currency -= turretToBuild.cost;

        GameObject turret = Instantiate(turretToBuild.prefab, node.GetBuildPosition(), Quaternion.identity);
        node.turret = turret;

        GameObject effect = Instantiate(buildEffect, node.GetBuildPosition(), Quaternion.identity);
        Destroy(effect, 5f);
        Debug.Log("Turret built, currency left: " + PlayerStats.Currency);
    }

    public GameObject GetStandardTurret()
    {
        return standardTurretPrefab;
    }
    public GameObject GetRocketTurret()
    {
        return rocketTurretPrefab;
    }
    public GameObject GetLaserTurret()
    {
        return laserTurretPrefab;
    }
}
