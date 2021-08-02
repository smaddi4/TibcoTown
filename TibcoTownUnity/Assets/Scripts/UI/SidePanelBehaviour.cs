using UnityEngine;

public class SidePanelBehaviour : UIBehaviour
{
    [SerializeField] private GameObject sidePanel, menu;
    public override void GameStateChanged(GameState gameState)
    {
        sidePanel.SetActive(gameState == GameState.Menu || gameState == GameState.Game);
        if(gameState == GameState.Game)
            menu.SetActive(false);
    }

    public void OnButtonClick(int gameState)
    {
        GameManagerBehaviour.Instance.GameStateChanged((GameState)gameState);
    }

    public void OnExit()
    {
        Application.Quit();
    }
}
