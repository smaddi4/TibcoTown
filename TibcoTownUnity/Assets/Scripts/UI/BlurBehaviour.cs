using UnityEngine;

public class BlurBehaviour : UIBehaviour
{
    [SerializeField] private GameObject panel;

    public override void GameStateChanged(GameState gameState)
    {
       panel.SetActive(!(gameState == GameState.CityBuild || gameState == GameState.Game));
    }
}
