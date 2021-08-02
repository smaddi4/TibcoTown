using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CityBehaviour : UIBehaviour {

    [SerializeField]
    private GameObject cityObject;

    public static bool isDragBuilding;

    public override void GameStateChanged(GameState gameState)
    {
	    cityObject.SetActive(gameState == GameState.CityBuild);
    }
}
