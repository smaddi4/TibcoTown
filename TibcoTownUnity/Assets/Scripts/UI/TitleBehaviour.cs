using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleBehaviour : UIBehaviour {

	[SerializeField]
    private GameObject title;

	public override void GameStateChanged(GameState gameState)
	{
		switch (gameState)
		{
			case GameState.Login:
            case GameState.LoginError:
                title.gameObject.SetActive(true);
				break;
			case GameState.Game:
                title.gameObject.SetActive(false);
				break;
		}
	}
}
