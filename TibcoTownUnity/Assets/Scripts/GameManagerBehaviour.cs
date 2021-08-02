using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

public enum GameState
{
    Login = 1,
    LoginError = 2,
    Game = 3,
    Pause = 4,
    Instructions = 5,
    Quiz = 6,
    Outlets = 7,
    CityBuild = 8,
    Menu = 9,
    Profile = 10,
    Leadeboard = 11
}

public enum ApiState
{
   None,
   Request,
   Response
}

public class GameManagerBehaviour : MonoBehaviour {

    public static GameManagerBehaviour Instance;
    public Action<GameState> GameStateAction;
    public Action<int> PlayerMoneyAction;
    public Action<bool> PauseAction;
    public Action<ApiState> ApiStateAction;

    public string BaseURL = "http://20.81.91.157:9000/restservice";
    [Space,SerializeField] private List<Avatar> avatars;
    [SerializeField] private List<Avatar> quizImages;

    public int UserID { get; private set; }
    public UserDetails UserDetails;
    
    public Player player
    {
        get;
        private set;
    }

    private GameState gameState;
    private ApiState apiState;

    void Awake ()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(Instance);
        } else
        {
            Destroy(Instance);
        }
    }

	void Start () {

        if (player == null)
        {
            GameStateChanged(GameState.Login);
        }
	}

    public void GameStateChanged (GameState state)
    {
        gameState = state;
        PauseAction?.Invoke(gameState == GameState.Pause ? true : false);
        GameStateAction?.Invoke(gameState);
    }
    
    public void ApiStateChanged (ApiState state)
    {
        apiState = state;
        ApiStateAction?.Invoke(apiState);
    }

    public void RemoveMoney (int price)
    {
		UserDetails.GetKPIDetailsResponse.TotalAvailableCoins -= price;
        PlayerMoneyAction?.Invoke(UserDetails.GetKPIDetailsResponse.TotalAvailableCoins);
    }

    public void AddMoney (int profit)
    {
        UserDetails.GetKPIDetailsResponse.TotalAvailableCoins += profit;
        PlayerMoneyAction?.Invoke(UserDetails.GetKPIDetailsResponse.TotalAvailableCoins);
    }
    
    public bool IsEnoughMoney (int price)
    {
        return price <= UserDetails.GetKPIDetailsResponse.TotalAvailableCoins;
    }

    public void TryLogIn (Login login)
    {
        ApiStateChanged(ApiState.Request);

        LoginHelper helper = new LoginHelper();
        StartCoroutine(helper.LoginRequest(login, resp =>
        {
            LoginResponseData _data = JsonConvert.DeserializeObject<LoginResponseData>(resp);
            
            if (_data.Status.StatusCode == GameConstants.Success)
            {
                UserID = _data.LoginResponse.UserId;
                GetKPIDetails _details = new GetKPIDetails(new GetKPIDetailsRequest(UserID));
                ApiManager.Instance.Post(_details, OnKPIDetails, OnFailure);
                
            } else
            {
                GameStateChanged(GameState.LoginError);
                ApiStateChanged(ApiState.Response);
            }
        }, (statusCode, response) =>
        {
            Debug.LogFormat("Failed - {0}", statusCode);
            GameStateChanged(GameState.LoginError);
            ApiStateChanged(ApiState.Response);
            string msg = string.Empty;
            switch (statusCode)
            {
                case GameConstants.InvalidPin:
                    msg = "You have entered invalid pin.";
                    break;
                case GameConstants.UserNotFound:
                    msg = "User not found!, please enter valid username.";
                    break;
                case GameConstants.SystemError:
                    msg = "Something went wrong, please try again.";
                    break;
                default:
                    msg = "Something went wrong, please try again.";
                    break;
            }
            GenericPopupBehaviour.Instance.SetValues("Login Failed!", msg,  "OKAY");
        }) );
    }

    private void OnFailure(int statusCode, string resp)
    {
        ApiStateChanged(ApiState.Response);
    }

    private void OnKPIDetails(string resp)
    {
        Debug.LogFormat("KPI details {0}", resp);
        
        UserDetails _userDetails = JsonConvert.DeserializeObject<UserDetails>(resp);
        this.UserDetails = _userDetails;
        GameStateChanged(GameState.Game);
        ApiStateChanged(ApiState.Response);
    }

    public Sprite GetAvatar(string profilePicID)
    {
        return avatars.Find(x => x.id.Contains(profilePicID)).pic;
    }
    
    public Sprite getSprite(string id)
    {
        return quizImages.Find(x => x.id.Contains(id)).pic;
    }

    [Serializable]
    class Avatar
    {
        public string id;
        public Sprite pic;
    }
}


