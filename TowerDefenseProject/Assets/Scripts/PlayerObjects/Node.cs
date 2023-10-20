using UnityEngine;
using UnityEngine.EventSystems;
public class Node : MonoBehaviour
{
    [SerializeField]
    private Color hoverColor;
    [SerializeField]
    private Color notEnoughMoneyColor;
    [SerializeField]
    private Color turretAlreadyBuilt;
    [SerializeField]
    private Vector3 positionalOffset;

    private Color startColor;
    private Renderer rend;

    [HideInInspector]
    public GameObject turret;
    [HideInInspector]
    public TurretBlueprint blueprint;
    [HideInInspector]
    public bool isUpgraded;
    private BuildManager buildManager;

    private void Start()
    {
        rend = GetComponent<Renderer>();
        startColor = rend.material.color;
        buildManager = BuildManager.bmInstance;
        turret = null;
        blueprint = null;
        isUpgraded = false;
    }

    private void OnMouseDown()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }
        if (turret != null)
        {
            buildManager.SelectNode(this);
            return;
        }
        if (!buildManager.CanBuild)
        {
            return;
        }
        BuildTurret(buildManager.GetTurretToBuild());
    }
    private void BuildTurret(TurretBlueprint _blueprint)
    {
        if (PlayerStats.Currency < _blueprint.cost)
        {
            Debug.Log("Not enough Moolah to build");
            return;
        }

        PlayerStats.Currency -= _blueprint.cost;
        blueprint = _blueprint;
        GameObject _turret = Instantiate(_blueprint.prefab, GetBuildPosition(), Quaternion.identity);
        turret = _turret;

        GameObject _effect = Instantiate(buildManager.buildEffect, GetBuildPosition(), Quaternion.identity);
        Destroy(_effect, 5f);
    }

    public void UpgradeTurret()
    {
        if (PlayerStats.Currency < blueprint.upgradeCost)
        {
            Debug.Log("Not enough Moolah to upgrade");
            return;
        }

        PlayerStats.Currency -= blueprint.upgradeCost;

        //Removed old turret to make space for upgraded turret
        Destroy(turret);

        //Build upgraded turret
        GameObject _turret = Instantiate(blueprint.upgradedPrefab, GetBuildPosition(), Quaternion.identity);
        turret = _turret;

        GameObject _effect = Instantiate(buildManager.buildEffect, GetBuildPosition(), Quaternion.identity);
        Destroy(_effect, 5f);
        isUpgraded = true; 
    }

    public void SellTurret()
    {
        if(!isUpgraded)
        {
            PlayerStats.Currency += blueprint.sellCost;
        }
        else
        {
            PlayerStats.Currency += blueprint.upgradedSellCost;
        }
        Destroy(turret);
        GameObject effect = Instantiate(buildManager.sellEffect, GetBuildPosition(), Quaternion.identity);
        isUpgraded = false;
        Destroy(effect, 5f);
    }

    public Vector3 GetBuildPosition()
    {
        return transform.position + positionalOffset;
    }    

    private void OnMouseEnter()
    {
        if (!buildManager.CanBuild)
        {
            if(turret != null)
            {
                rend.material.color = turretAlreadyBuilt;
                return;
            }
            return;
        }
        if(EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }
        if(turret != null)
        {
            rend.material.color = turretAlreadyBuilt;
            return;
        }
        if(buildManager.HasCurrency)
        {
            rend.material.color = hoverColor;
        }
        else
        {
            rend.material.color = notEnoughMoneyColor;
        }
    }

    private void OnMouseExit()
    {
        rend.material.color = startColor;
    }


}
