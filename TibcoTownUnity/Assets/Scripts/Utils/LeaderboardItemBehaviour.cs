using UnityEngine;
using UnityEngine.UI;

public class LeaderboardItemBehaviour : MonoBehaviour
{
    [SerializeField] private Text name, rank, coin;
    [SerializeField] private Image profile;
    [SerializeField] 
    private GameObject reward1,
        reward2,
        reward3;

    public void SetValues(LeaderboardUser user)
    {
        name.text = user.Name;
        rank.text = user.Ranking;
        coin.text = user.Coins.ToString();
        profile.sprite = GameManagerBehaviour.Instance.GetAvatar(user.Avatar);
        reward1.SetActive(user.Ranking == "1");
        reward2.SetActive(user.Ranking == "2");
        reward3.SetActive(user.Ranking == "3");;
    }
}
