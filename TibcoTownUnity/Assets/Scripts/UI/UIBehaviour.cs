using UnityEngine;

public abstract class UIBehaviour : MonoBehaviour 
{
	public virtual void Start () 
	{
		GameManagerBehaviour.Instance.GameStateAction += GameStateChanged;
	}

	/// <summary>
	/// On game state change this method will be called.
	/// </summary>
	/// <param name="gameState"></param>
    public abstract void GameStateChanged(GameState gameState);

	
}
