using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraDragMove : MonoBehaviour
{
    private Vector3 originalDragPosition;
	private Vector3 dragPointerPosition;
    private bool drag = false;

    public float xOffset = 25f,
	    yOffset = 5f;

    private bool isMoveEnabled = false;
    
    public void Start () 
    {
	    GameManagerBehaviour.Instance.GameStateAction += GameStateChanged;
    }

    private void GameStateChanged(GameState gameState)
    {
	    isMoveEnabled = gameState == GameState.CityBuild;
	    if (!isMoveEnabled)
	    {
		    Camera.main.transform.position = new Vector3(0, 0, -10f);
	    }
    }
	void LateUpdate()
	{
		if (!isMoveEnabled) return;
		
		if (Input.GetMouseButton(1))
		{
			dragPointerPosition = (Camera.main.ScreenToWorldPoint(Input.mousePosition)) - Camera.main.transform.position;
			if (drag == false)
			{
				drag = true;
				originalDragPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			}
		}
		else
		{
			drag = false;
		}
		if (drag == true)
		{
			Vector3 _newPos =  originalDragPosition - dragPointerPosition;
			Camera.main.transform.position = new Vector3(Mathf.Clamp(_newPos.x, -xOffset, xOffset), Mathf.Clamp(_newPos.y, -yOffset, yOffset), _newPos.z);
		}
		
	}
}
