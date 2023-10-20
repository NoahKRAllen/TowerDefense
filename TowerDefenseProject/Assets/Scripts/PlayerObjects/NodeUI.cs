using UnityEngine;
using UnityEngine.UI;

public class NodeUI : MonoBehaviour
{
    private Node target;

    [SerializeField]
    private GameObject UI;
    [SerializeField]
    private Text upgradeCost;
    [SerializeField]
    private Text upgradeText;
    [SerializeField]
    private Button upgradeButton;
    [SerializeField]
    private Text sellCost;

    private void Start()
    {
        Hide();
    }
    public void SetTarget(Node _target)
    {
        UI.SetActive(true);
        target = _target;
        if(!_target.isUpgraded)
        {
            upgradeButton.interactable = true;
            upgradeText.text = "UPGRADE";
            upgradeCost.text = "$" + _target.blueprint.upgradeCost;
            sellCost.text = "$" + _target.blueprint.sellCost;
        }
        else
        {
            upgradeButton.interactable = false;
            upgradeText.text = "FULLY";
            upgradeCost.text = "UPGRADED";
            sellCost.text = "$" + _target.blueprint.upgradedSellCost;
        }
        transform.position = _target.GetBuildPosition();
    }

    public void Hide()
    {
        UI.SetActive(false);
    }

    public void Upgrade()
    {
        target.UpgradeTurret();
        BuildManager.bmInstance.DeselectNode();
    }

    public void Sell()
    {
        target.SellTurret();
        BuildManager.bmInstance.DeselectNode();
    }

}
