using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Image = UnityEngine.UI.Image;

public class ProfileBehaviour : UIBehaviour
{
    [SerializeField] 
    private Text userName,
        coins;

    [SerializeField] 
    private Image avatar, 
        badgeImage;

    [SerializeField] 
    private GameObject profilePanel;

    [SerializeField] 
    private List<Sprite> avatars, 
        badges;

    public override void GameStateChanged(GameState gameState)
    {
        if (gameState == GameState.Profile)
        {
            profilePanel.SetActive(true);
            SetValues();
        }
        else
        {
            profilePanel.SetActive(false);
        }
    }

    private void SetValues()
    {
        GetKPIDetailsResponse _player = GameManagerBehaviour.Instance.UserDetails.GetKPIDetailsResponse;
        userName.text = _player.Name;
        coins.text = _player.TotalAvailableCoins.ToString();
        avatar.sprite = GameManagerBehaviour.Instance.GetAvatar(_player.ProfilePic);
        switch (_player.OverallBadge)
        {
            case "Gold":
            case "GOLD":
            case "gold":
                badgeImage.sprite = badges[0];    
                break;
            
            case "Silver":
            case "SILVER":
            case "silver":
                badgeImage.sprite = badges[1];  
                break;
            
            case "Copper":
            case "COPPER":
            case "copper":
                badgeImage.sprite = badges[2];  
                break;
        }
    }
}
