using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ObjectBehaviour : MonoBehaviour {

    protected bool isPause;
	
	public virtual void Start () {

		GameManagerBehaviour.Instance.PauseAction += OnPause;
	}

	public virtual void OnPause(bool pauseValue)
	{
		isPause = pauseValue;
	}
	
}
