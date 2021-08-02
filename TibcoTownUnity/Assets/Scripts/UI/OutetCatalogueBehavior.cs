using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class OutetCatalogueBehavior : UIBehaviour
{
    [SerializeField] 
    private GameObject panel,
                        outletItem,
                        parent;

    [SerializeField] private OutletList outletList;

    public override void Start()
    {
        base.Start();
        /*foreach (var outlet in outletList.outlets)
        {
            GameObject _go = Instantiate(outletItem, parent.transform);
            _go.SetActive(true);
            CatalogueItemBehaviour _behaviour = _go.GetComponent<CatalogueItemBehaviour>();
            _behaviour.SetValues(outlet);
        }*/
    }

    public override void GameStateChanged(GameState gameState)
    {
        panel.SetActive(gameState == GameState.Outlets);
    }
}
