using UnityEngine;

public class HomBehaviour : UIBehaviour
{
    [SerializeField] 
    private GameObject homePanel, 
                        kpiItem,
                        kpiItemRoot;

    private bool isValuesUpdated = false;

    public override void Start()
    {
        base.Start();
        isValuesUpdated = false;
    }

    public override void GameStateChanged(GameState gameState)
    {
        homePanel.SetActive(gameState == GameState.Game || gameState == GameState.Menu);
        if (gameState == GameState.Game)
        {
            SetValues();
        }
    }

    private void SetValues()
    {
        if (!isValuesUpdated)
        {
            KPIConfigurations _configurations = GameManagerBehaviour.Instance.UserDetails.GetKpiConfigurations();
            KPICoins _kpiCoins = GameManagerBehaviour.Instance.UserDetails.GetKpiCoins();
            isValuesUpdated = true;

            foreach (CoinsDetail _kpi in _kpiCoins.CoinsDetails)
            {
                if (!(_configurations.KPIs.Exists(x => x.KPIId == _kpi.KPIId)))
                {
                    continue;
                }
                GameObject _go = Instantiate(kpiItem, kpiItemRoot.transform);
                _go.SetActive(true);
                KpiItemBehaviour _behaviour = _go.GetComponent<KpiItemBehaviour>();
                _behaviour.SetValues(_configurations.KPIs.Find(x => x.KPIId == _kpi.KPIId).KPIName, _kpi.ProgressBar, _kpi.Color);
            }
            
        }
        
    }
}
