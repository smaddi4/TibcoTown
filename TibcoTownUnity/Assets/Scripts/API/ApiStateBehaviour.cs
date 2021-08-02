using UnityEngine;

public abstract class ApiStateBehaviour : MonoBehaviour
{
    public virtual void Start () {
        GameManagerBehaviour.Instance.ApiStateAction += ApiStateChanged;
    }

    public abstract void ApiStateChanged(ApiState apiState);
}
