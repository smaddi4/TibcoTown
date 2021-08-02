using System;

[Serializable]
public class Player
{
    public string nickname;
	public int money;

	public Player(string _nickname, int _money)
	{
        nickname = _nickname;
        money = _money;
	}
}

public class PlayerHelper {
    public void LoadRequest(int userId, Action<string> OnResult, Action<int, string> OnFailure = default)
    {
        GetKPIDetails _details = new GetKPIDetails(new GetKPIDetailsRequest(userId));
        ApiManager.Instance.Post(_details, OnResult, OnFailure);
    }
}
