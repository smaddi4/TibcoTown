using UnityEngine;

public class LoadingPanelBehaviour : UIBehaviour
{
    [SerializeField] private GameObject loadingPanel;

    public override void Start()
    {
        base.Start();
        GameManagerBehaviour.Instance.ApiStateAction += ApiStateChanged;
    }

    public override void GameStateChanged(GameState gameState)
    {
    }

    public void ApiStateChanged(ApiState apiState)
    {
        Debug.LogFormat("Api state : {0}", apiState);

        switch (apiState)
        {
            case ApiState.Request:
                loadingPanel.SetActive(true);
                break;
            case ApiState.Response:
                loadingPanel.SetActive(false);
                break;
        }
    }
}
