using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerBehaviour : UIBehaviour {

    private const string MONEY_MASK = "000";

	[SerializeField]
	private Text nicknamePlayerText;
    [SerializeField]
    private GameObject moneyPanel;
	[SerializeField]
	private Text moneyText;

    private int lastMoney;

    public override void Start ()
    {
        base.Start();

        GameManagerBehaviour.Instance.PlayerMoneyAction += OnMoneyChanged;
    }

	public override void GameStateChanged(GameState gameState)
	{
		switch (gameState)
		{
			case GameState.Login:
			case GameState.LoginError:
				nicknamePlayerText.gameObject.SetActive(false);
                moneyPanel.SetActive(false);
				break;
			case GameState.Game:
				nicknamePlayerText.gameObject.SetActive(true);
				nicknamePlayerText.text = GameManagerBehaviour.Instance.player.nickname;
				moneyPanel.SetActive(true);
				moneyText.text = GameManagerBehaviour.Instance.player.money.ToString();
                lastMoney = GameManagerBehaviour.Instance.player.money;
				break;
		}
	}

    public void OnMoneyChanged (int money)
    {
        StopAllCoroutines();
        if(lastMoney > money)
        {
            StartCoroutine(DecreaseValue (money));
        } else
        {
            StartCoroutine(IncreaseValue (money));
        }
    }

	IEnumerator IncreaseValue(int money)
	{
		while (money > lastMoney)
		{
			lastMoney += Mathf.CeilToInt((money - lastMoney) * .2f);
			moneyText.text = lastMoney.ToString(MONEY_MASK);
			yield return null;
		}
        moneyText.text = GameManagerBehaviour.Instance.player.money.ToString();
	}

	IEnumerator DecreaseValue(int money)
	{
		while (money < lastMoney)
		{
			lastMoney -= Mathf.CeilToInt((lastMoney - money) * .2f);
            moneyText.text = lastMoney.ToString(MONEY_MASK);
			yield return null;
		}
        moneyText.text = GameManagerBehaviour.Instance.player.money.ToString();
	}
}
