using UnityEngine;
using UnityEngine.UI;

public class LeaderbordBehaviour : UIBehaviour
{
    [SerializeField] 
    private GameObject leaderboardPanel,
        leaderboardItem,
        leaderboardItemRoot;
        

    [SerializeField] 
    private Text fistUserName, secondUserName, thirdUserName;

    private bool isValuesUpdated = false;

    public override void Start()
    {
        base.Start();
        isValuesUpdated = false;
    }
    
    public override void GameStateChanged(GameState gameState)
    {
        if (gameState == GameState.Leadeboard)
        {
            leaderboardPanel.SetActive(true);
            SetValues();
        }
        else
        {
            leaderboardPanel.SetActive(false);
        }
    }
    
    private void SetValues()
    {
        if (!isValuesUpdated)
        {
            Leaderboard _leaderboard = GameManagerBehaviour.Instance.UserDetails.GetLeaderBoard();
            isValuesUpdated = true;

            for (int i = 0; i < _leaderboard.Users.Count; i++)
            {
                
            }

            fistUserName.text = _leaderboard.Users.Find(x => x.Ranking == "1").Name.Split(' ')[0];
            secondUserName.text = _leaderboard.Users.Find(x => x.Ranking == "2").Name.Split(' ')[0];
            thirdUserName.text = _leaderboard.Users.Find(x => x.Ranking == "3").Name.Split(' ')[0];
            
            foreach(LeaderboardUser _user in _leaderboard.Users)
            {
                GameObject _go = Instantiate(leaderboardItem, leaderboardItemRoot.transform);
                _go.SetActive(true);
                LeaderboardItemBehaviour _behaviour = _go.GetComponent<LeaderboardItemBehaviour>();
                _behaviour.SetValues(_user);
            }
        }
    }
}
