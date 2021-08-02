using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseButtonBehaviour : UIBehaviour
{
    [SerializeField]
    private GameObject pauseButton;

	public override void GameStateChanged(GameState gameState)
	{
		switch (gameState)
		{
			case GameState.Login:
			case GameState.LoginError:
			case GameState.Pause:
				pauseButton.gameObject.SetActive(false);
				break;
			case GameState.Game:
				pauseButton.gameObject.SetActive(true);
				break;
		}
	}

	
    public void OnPauseButtonTapped ()
    {
        GameManagerBehaviour.Instance.GameStateChanged(GameState.Pause);
    }
}
