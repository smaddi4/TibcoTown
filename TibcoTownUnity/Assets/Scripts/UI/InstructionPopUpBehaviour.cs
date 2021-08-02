using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstructionPopUpBehaviour : UIBehaviour {

	[SerializeField]
	private GameObject popUp;

	public override void GameStateChanged(GameState gameState)
	{
		switch (gameState)
		{
			case GameState.Login:
			case GameState.LoginError:
			case GameState.Game:
				popUp.gameObject.SetActive(false);
				break;
            case GameState.Instructions:
				popUp.gameObject.SetActive(true);
				break;
		}
	}

	public void OnButtonTapped()
	{
		GameManagerBehaviour.Instance.GameStateChanged(GameState.Game);
	}
}
