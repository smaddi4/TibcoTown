using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstructionButtonBehaviour : UIBehaviour {

	[SerializeField]
	private GameObject instructionButton;

	public override void GameStateChanged(GameState gameState)
	{
		switch (gameState)
		{
			case GameState.Login:
			case GameState.LoginError:
			case GameState.Pause:
            case GameState.Instructions:
				instructionButton.gameObject.SetActive(false);
				break;
            case GameState.Game:
				instructionButton.gameObject.SetActive(true);
				break;
		}
	}


	public void OnButtonTapped()
	{
		GameManagerBehaviour.Instance.GameStateChanged(GameState.Instructions);
	}
}
