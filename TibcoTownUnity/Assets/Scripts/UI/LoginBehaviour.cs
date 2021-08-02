using UnityEngine;
using UnityEngine.UI;

public class LoginBehaviour : UIBehaviour {

    [SerializeField]
    private InputField usernameField;
    [SerializeField]
    private InputField passwordField;
    [SerializeField]
    private GameObject button;
    [SerializeField]
    private GameObject loadingAnimator;
    [SerializeField]
    private GameObject errorMessage;

	public override void GameStateChanged(GameState gameState)
	{
        switch (gameState)
        {
            case GameState.Login:
                loadingAnimator.SetActive(false);
                usernameField.gameObject.SetActive(true);
                passwordField.gameObject.SetActive(true);
                button.SetActive(true);
                errorMessage.SetActive(false);
                break;
            case GameState.LoginError:
                usernameField.text = string.Empty;
                passwordField.text = string.Empty;
                break;
            case GameState.Game:
                loadingAnimator.SetActive(false);
				usernameField.gameObject.SetActive(false);
				passwordField.gameObject.SetActive(false);
                button.SetActive(false);
				errorMessage.SetActive(false);
                break;
        }
    }

    public void LoginButtonClicked ()
    {
        if (string.IsNullOrEmpty(usernameField.text) || string.IsNullOrEmpty(passwordField.text))
        {
            GenericPopupBehaviour.Instance.SetValues("Alert!", "Please enter username/pin.",  "OKAY");
            return;
        }
        
        Login login = new Login(usernameField.text, passwordField.text);
        GameManagerBehaviour.Instance.TryLogIn(login);
    }
}
